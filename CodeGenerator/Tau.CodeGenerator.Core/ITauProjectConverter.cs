using Tau.CodeGenerator.Abstractions.IR;
using Tau.CodeGenerator.Abstractions.Models;

namespace Tau.CodeGenerator.Core;

internal interface ITauProjectConverter
{
    public TauProject Convert(IRNode? node);
}
