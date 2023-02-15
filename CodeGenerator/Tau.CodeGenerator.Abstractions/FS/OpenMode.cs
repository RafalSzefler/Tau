namespace Tau.CodeGenerator.Abstractions.FS;

public enum OpenMode : int
{
    Get = 0x01,
    Create = 0x10,
    GetOrCreate = 0x11,
}
