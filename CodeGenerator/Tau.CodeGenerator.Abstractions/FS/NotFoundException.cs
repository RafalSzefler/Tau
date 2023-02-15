using System;

public sealed class NotFoundException : Exception
{
    public string FullPath { get; }

    public NotFoundException(string path)
        : base($"File [{path}] not found.")
    {
        FullPath = path;
    }
}
