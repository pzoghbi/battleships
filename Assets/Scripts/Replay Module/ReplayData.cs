using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class ReplayData : IReplayData
{
    //internal BattleshipGameSettings gameSettings;
    public List<IReplayStateData> staticData;   // player data
    public List<IReplayStateData> stateHistory = new List<IReplayStateData>(); // board states
    private static readonly string directoryPath = Application.persistentDataPath;
    private static DirectoryInfo dir = new DirectoryInfo(directoryPath);
    private string NewFileName => $"replay{dir.GetFiles().Length}";
    private string FilePath => directoryPath + $"/{NewFileName}.{newFileExtension}";
    private const string newFileExtension = "json";

    public ReplayData(List<IReplayStateData> playersData)
    {
        //this.gameSettings = gameSettings; todo decouple gamesettingsSO and data
        this.staticData = playersData;
    }

    public void UpdateState(ref List<IReplayStateData> stateToUpdate, IReplayStateData stateData)
        => stateToUpdate.Add(stateData);

    public async Task<bool> SaveToFile()
    {        
        return await Task.Run(async () => {
            try
            {
                var json = JsonConvert.SerializeObject(this);
                await File.WriteAllTextAsync(FilePath, json);
                return true;
            } catch
            {
                return false;
            }
        });
    }
}