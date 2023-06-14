using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ReplayData : IReplayData
{
    public List<IReplayStateData> staticData = new List<IReplayStateData>();
    public List<IReplayStateData> stateHistory = new List<IReplayStateData>();

    private ReplayFileManager fileManager;

    [JsonConstructor]
    public ReplayData() { }

    public ReplayData(ReplayFileManager fileManager)
    {
        // this.gameSettings = gameSettings; todo decouple gamesettingsSO and data
        // to save gameSettings in replay to allow more game modes to be replayed
        this.fileManager = fileManager;
    }

    public void UpdateState(List<IReplayStateData> stateToUpdate, IReplayStateData stateData)
        => stateToUpdate.Add(stateData);

    public async Task<bool> SaveToFile()
    {
        return await fileManager.SaveToFile(this);
    }

    public ReplayData LoadLastReplayFile()
    {
        return fileManager.LoadLastReplayFile();
    }
}