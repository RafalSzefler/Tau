using System;
using System.Collections.Generic;
using Tau.CodeGenerator.Abstractions.IR;

namespace Tau.CodeGenerator.Abstractions.Models;

public sealed class TauProject
{
    public TauIdentifier Name { get; }
    public TauVersion Version { get; }
    public IReadOnlyList<TauType> MessageTypes { get; }
    public IReadOnlyDictionary<TauIdentifierPath, TauType> MessageTypesByFullName { get; }
    public IReadOnlyList<TauService> Services { get; }
    public IReadOnlyDictionary<TauIdentifierPath, TauService> ServicesByFullName { get; }
    public IRObject AdditionalSettings { get; }

    public TauProject(
        TauIdentifier name,
        TauVersion version,
        IReadOnlyList<TauType>? messageTypes,
        IReadOnlyList<TauService>? services,
        IRObject? additionalSettings)
    {
        Name = name;
        Version = version;
        MessageTypes = messageTypes ?? Array.Empty<TauType>();
        Services = services ?? Array.Empty<TauService>();

        var messageTypesByFullName = new Dictionary<TauIdentifierPath, TauType>();
        foreach (var messageType in MessageTypes)
        {
            messageTypesByFullName[messageType.FullName] = messageType;
        }
        messageTypesByFullName.TrimExcess();
        MessageTypesByFullName = messageTypesByFullName;

        var servicesByFullName = new Dictionary<TauIdentifierPath, TauService>();
        foreach (var service in Services)
        {
            servicesByFullName[service.FullName] = service;
        }
        servicesByFullName.TrimExcess();
        ServicesByFullName = servicesByFullName;
        AdditionalSettings = additionalSettings ?? IRObject.Create(null);
    }
}
