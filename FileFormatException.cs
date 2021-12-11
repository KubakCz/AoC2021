using System;

class FileFormatException : Exception
{
    public string FileName { get; }
    public override string Message => $"In file '{FileName}': {base.Message}";

    public FileFormatException(string filePath) : base()
    {
        FileName = filePath;
    }

    public FileFormatException(string filePath, string message) : base(message)
    {
        FileName = filePath;
    }

    public FileFormatException(string filePath, string message, Exception innerException) : base(message, innerException)
    {
        FileName = filePath;
    }
}