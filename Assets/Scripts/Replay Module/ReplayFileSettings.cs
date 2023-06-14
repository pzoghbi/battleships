using System;
using System.Runtime.Serialization;

public class ReplayFileSettings
{
    internal string Extension { get; private set; } = "json";
    /// <summary>
    /// Generates a prefixed name followed by UTC DateTime
    /// </summary>
    internal string FileName 
    { 
        get => $"{fileName}-{dateTime}";
        private set => fileName = $"{value}-{dateTime}";
    }

    private string fileName = "replay";
    private string dateTime => DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

    public ReplayFileSettings() { }

    /// <param name="fileName">Prefix for file(s) before DateTime</param>
    /// <param name="extension">File extension name</param>
    public ReplayFileSettings(string fileName, string extension)
    {
        FileName = fileName;
        this.Extension = extension;
    }
}
