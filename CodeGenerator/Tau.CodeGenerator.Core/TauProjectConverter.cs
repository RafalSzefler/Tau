using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Tau.CodeGenerator.Abstractions;
using Tau.CodeGenerator.Abstractions.IR;
using Tau.CodeGenerator.Abstractions.Models;

namespace Tau.CodeGenerator.Core;

internal sealed class TauProjectConverter : ITauProjectConverter
{
    private readonly Regex _identifierRegex;
    private readonly Dictionary<string, Func<IRObject, Dictionary<TauIdentifierPath, TauType>, TauType>> _mappers;
    private readonly TauIdentifierPath _empty = new TauIdentifierPath(null);

    public TauProjectConverter()
    {
        _identifierRegex = new Regex("^[a-zA-Z][a-zA-Z0-9_]*$", RegexOptions.Compiled);
        _mappers = new Dictionary<string, Func<IRObject, Dictionary<TauIdentifierPath, TauType>, TauType>>
        {
            {"message", ParseMessageType},
        };
    }

    public TauProject Convert(IRNode? node)
    {
        if (node == null)
        {
            throw new ArgumentException($"[{nameof(node)}] cannot be null");
        }

        if (node is IRObject obj)
        {
            return ConvertFromObject(obj);
        }
        throw new ArgumentException($"[{nameof(node)}] has to be [{nameof(IRObject)}]");
    }

    private TauProject ConvertFromObject(IRObject node)
    {
        var props = node.Properties;
        var project = Load<IRObject>(node, "Project");
        var projectName = ReadIdentifier(project, "Name");
        var projectVersion = ReadVersion(project, "Version");
        var irTypes = Load<IRArray>(project, "MessageTypes");
        var irServices = Load<IRArray>(project, "Services");
        var additionalSettings = Load<IRObject>(project, "AdditionalSettings");

        var messageTypes = ParseIrTypes(irTypes);

        return new TauProject(
            projectName,
            projectVersion,
            messageTypes,
            ParseIrServices(irServices, messageTypes),
            additionalSettings);
    }

    private IReadOnlyList<TauType> ParseIrTypes(IRArray array)
    {
        // TODO: topologically sort this array first.
        var elements = array.Elements;
        var count = elements.Count;
        if (count == 0)
        {
            return Array.Empty<TauType>();
        }

        var result = new TauType[count];

        // TODO: fill with primitive types.
        var cachedTypes = new Dictionary<TauIdentifierPath, TauType>();

        for (var i = 0; i < count; i++)
        {
            var element = elements[i];
            if (!(element is IRObject messageTypeObj))
            {
                throw new ArgumentException($"[{nameof(TauType)}] has to be represented by [{nameof(IRObject)}] type.");
            }

            var kind = ReadString(messageTypeObj, "Kind").ToLower();
            if (!_mappers.TryGetValue(kind, out var mapper))
            {
                var availableKinds = string.Join(", ", _mappers.Keys);
                throw new ArgumentException($"Invalid [{nameof(TauType)}] - unknown kind [{kind}]. Available kinds are: [{availableKinds}]");
            }
            var mapped = mapper(messageTypeObj, cachedTypes);
            cachedTypes[mapped.FullName] = mapped;
            result[i] = mapped;
        }
        return result;
    }

    private TauType ParseMessageType(IRObject irType, Dictionary<TauIdentifierPath, TauType> cachedTypes)
    {
        var messageName = ReadIdentifier(irType, "Name");
        var @namespace = _empty;
        const string namespaceKey = "Namespace";
        if (irType.Properties.ContainsKey(namespaceKey))
        {
            @namespace = ReadIdentifierPath(irType, namespaceKey);
        }

        IRObject? additionalSettings = null;
        const string additionalSettingsKey = "AdditionalSettings";
        if (irType.Properties.ContainsKey(additionalSettingsKey))
        {
            additionalSettings = Load<IRObject>(irType, additionalSettingsKey);
        }

        IReadOnlyCollection<TauMessageTypeField> fields = Array.Empty<TauMessageTypeField>();
        const string fieldsKey = "Fields";
        if (irType.Properties.ContainsKey(fieldsKey))
        {
            var irFields = Load<IRArray>(irType, fieldsKey).Elements;
            var length = irFields.Count;
            var fieldsArray = new TauMessageTypeField[length];
            for (var i = 0; i < length; i++)
            {
                var irField = irFields[i];
                if (!(irField is IRObject irFieldObject))
                {
                    throw new ArgumentException($"[{nameof(TauMessageType)}] contains field that is not [{nameof(IRObject)}].");
                }
                var id = Load<IRInteger>(irFieldObject, "Id").Value;
                var name = ReadIdentifier(irFieldObject, "Name");
                var fullFieldTypeName = ReadIdentifierPath(irFieldObject, "FieldType");
                if (!cachedTypes.TryGetValue(fullFieldTypeName, out var fieldType))
                {
                    throw new ArgumentException($"[{nameof(TauMessageType)}] of name [{messageName.ToString()}] has field refering to unknown type [{fullFieldTypeName.ToString()}]");
                }
                fieldsArray[i] = new TauMessageTypeField(id, name, fieldType);
            }
        }

        return new TauMessageType(
            messageName,
            @namespace,
            additionalSettings,
            fields);
    }

    private IReadOnlyList<TauService> ParseIrServices(IRArray array, IReadOnlyList<TauType> messageTypes)
    {
        var cache = messageTypes.ToDictionary(static x => x.FullName, static x => x);
        // TODO
        throw new NotImplementedException();
    }

    private static TauVersion ReadVersion(IRObject obj, string key)
    {
        var value = ReadString(obj, key);
        var pieces = value.Split('.');

        const string excMessage = "Project version has to be of the form \"(MAJOR).(MINOR)\", e.g. 1.1";
        if (pieces.Length != 2)
        {
            throw new ArgumentException(excMessage);
        }
        if (!int.TryParse(pieces[0], out var major))
        {
            throw new ArgumentException(excMessage);
        }
        if (!int.TryParse(pieces[1], out var minor))
        {
            throw new ArgumentException(excMessage);
        }
        return new TauVersion(major, minor);
    }

    private TauIdentifier ReadIdentifier(IRObject obj, string key)
    {
        var value = ReadString(obj, key);
        if (!_identifierRegex.IsMatch(value))
        {
            throw new ArgumentException($"Identifer has to contain letters, digits and \"_\" sign, and can start with a letter only. Received: [{value}]");
        }
        return new TauIdentifier(value);
    }

    private TauIdentifierPath ReadIdentifierPath(IRObject obj, string key)
    {
        var value = ReadString(obj, key);
        var pieces = value.Split(Globals.IdentifierSeparator);
        var l = pieces.Length;
        var ids = new TauIdentifier[l];
        for (var i = 0; i < l; i++)
        {
            var piece = pieces[i];
            if (!_identifierRegex.IsMatch(piece))
            {
                throw new ArgumentException($"Identifer path has to contain letters, digits, \"_\" and \".\" signs. Each piece separated by \".\" can start with a letter only. Received: [{value}]");
            }
            ids[i] = new TauIdentifier(piece);
        }
        return new TauIdentifierPath(ids);
    }

    private static string ReadString(IRObject obj, string key)
    {
        var value = Load<IRString>(obj, "Name").Value;
        if (value != null)
        {
            value = value.Trim();
        }

        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException($"Value for key [{key}] cannot be an empty string.");
        }

        return value;
    }

    private static T Load<T>(IRObject obj, string key) where T : IRNode
    {
        if (!obj.Properties.TryGetValue(key, out var node))
        {
            throw new KeyNotFoundException(key);
        }

        if (node == null || !(node is T result))
        {
            throw new ArgumentException($"Key [{key}] is not of correct type [{typeof(T)}]");
        }

        return result;
    }
}
