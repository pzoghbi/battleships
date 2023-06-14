using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ReplayData : IReplayData
{
    // todo segregate some of these responsibilities
    public List<IReplayStateData> staticData;   // player data
    public List<IReplayStateData> stateHistory = new List<IReplayStateData>(); // board states
    internal static bool ReplayExists => dir.GetFiles(regexPattern).Length > 0;
    private static readonly string directoryPath = Application.persistentDataPath;
    private static DirectoryInfo dir = new DirectoryInfo(directoryPath);
    private static FileInfo[] DirFiles => dir.GetFiles(regexPattern);
    private string NewFileName => $"replay-{Guid.NewGuid()}";
    private string FilePath => directoryPath + $"/{NewFileName}.{newFileExtension}"; 
    private const string newFileExtension = "json";
    private static JsonSerializerSettings settings = 
        new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
    private static string regexPattern = "replay-*.json";

    public ReplayData()
    {
        staticData = new List<IReplayStateData>();
    }

    public ReplayData(List<IReplayStateData> playersData)
    {
        // this.gameSettings = gameSettings; todo decouple gamesettingsSO and data
        // to save gameSettings in replay to allow more game modes to be replayed
        this.staticData = playersData;
    }

    public void UpdateState(List<IReplayStateData> stateToUpdate, IReplayStateData stateData)
        => stateToUpdate.Add(stateData);

    public async Task<bool> SaveToFile()
    {
        return await Task.Run(async () =>
        {
            try
            {
                var json = JsonConvert.SerializeObject(this, settings);
                await File.WriteAllTextAsync(FilePath, json);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        });
    }

    public static ReplayData LoadLastReplayFile()
    {
        try
        {
            var lastFile = DirFiles.OrderBy(file => file.CreationTimeUtc).Last();
            var file = File.ReadAllText(lastFile.FullName);
            var instance = JsonConvert.DeserializeObject<ReplayData>(file, settings);
            return instance;
        }
        catch(Exception e)
        {
            Debug.LogError(e);
            return null;
        }
    }
}