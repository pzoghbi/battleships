using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ReplayFileManager
{
    internal bool ReplayExists => directoryInfo.GetFiles(regexPattern).Length > 0;

    private static DirectoryInfo directoryInfo => FileManager.DirectoryInfo;

    private ReplayFileSettings fileSettings = new ReplayFileSettings(); 
    private JsonSerializerSettings serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
    private string filePrefix = "replay";
    private string regexPattern;
    private string filePath;
    private FileInfo[] dirFiles => directoryInfo.GetFiles(regexPattern);

    public ReplayFileManager() {
        regexPattern = $"{filePrefix}-*.{fileSettings.Extension}";
        filePath = FileManager.DirectoryPath + $"/{fileSettings.FileName}.{fileSettings.Extension}";
    }

    public ReplayFileManager(ReplayFileSettings fileSettings)
    {
        this.fileSettings = fileSettings;
        regexPattern = $"{filePrefix}-*.{fileSettings.Extension}";
        filePath = FileManager.DirectoryPath + $"/{fileSettings.FileName}.{fileSettings.Extension}";
    }

    public async Task<bool> SaveToFile(ReplayData replayData)
    {
        return await Task.Run(async () =>
        {
            try
            {
                var json = JsonConvert.SerializeObject(replayData, serializerSettings);
                await File.WriteAllTextAsync(filePath, json);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        });
    }

    public ReplayData LoadLastReplayFile()
    {
        try
        {
            var lastFile = dirFiles.OrderBy(file => file.CreationTimeUtc).Last();
            var file = File.ReadAllText(lastFile.FullName);
            var instance = JsonConvert.DeserializeObject<ReplayData>(file, serializerSettings);
            return instance;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return null;
        }
    }
}
