# Battleships
Battleships-2-player-local-with-replay  

```console
git checkout 7days # (7-day challenge)
```


### Description:
A two-player, turn-based game played locally on a 10x10 grid. The game features randomly generated fleets of seven ships in various sizes: two 1x1, two 2x1, one 3x1, one 4x1, and one 5x1. Players take turns selecting coordinates to launch attacks, aiming to sink their opponent's fleet. The first player to sink all of their opponent's ships wins.

The game records replays after each match, allowing players to review and playback their moves.
### Info
Created with Unity 2021.3.13f1

### Installation
Clone/download repository and open up the project with Unity.

### Replay module features (WIP)
#### Recording:  
It is currently recording the entire state of the board  
Replays are saved in `.json` format in `Application.persistentDataPath` directory
i. e. (`Appdata/LocalLow/DefaultCompany/Battleships`)

#### Playback:  
It is only injecting the player actions and not reading the entire state.
Replay module will play the most recent recorded replay. It can be accessed from the main menu.

#### Pausing/Playing on-demand
At the current state of affairs, it's possible to play/pause replay on demand. Note that if the action has already been processed, it will be passed to the injector, and replay will be paused after the particular action. This might be changed in the future.

#### Partial modularity (WIP)
Although not fully featured, it is designed with flexibility in mind and could potentially support usage across similar projects in the future

#### Screenshots
![image](https://github.com/pzoghbi/battleships/assets/10575726/807c0786-e882-4452-8a0e-dbef17a3b178)
![image](https://github.com/pzoghbi/battleships/assets/10575726/ebad391b-259e-4de4-b304-72617381e666)

---
# Documentation
## Replay module
Replay module consists of various interfaces and classes that implement them. For replay to be successfully recorded and saved to the disk, we rely on [`ReplayData`](https://github.com/pzoghbi/battleships/blob/main/Assets/Scripts/Replay%20Module/ReplayData.cs) class, which implements `IReplayData` interface.

`IReplayData`
```csharp
public Task<bool> SaveToFile();
public void UpdateState(List<IReplayStateData> stateToUpdate, IReplayStateData stateData);
```

`IReplayStateData` Empty abstraction for storing data objects in `ReplayData`.  
Example: 
```csharp
public class PlayerData : IReplayStateData
{
    // ... some player data
}
```

`IReplayRecorder` (partial modularity) Note: `IPlayerAction` will be abstracted to `IReplayDataCapsule`. Inject a concrete `ReplayDataCapsule` to persist data to the replay state.
```csharp
public void PersistReplayDataCapsule(IPlayerAction action);
public Task<bool> SaveReplay();
```
Example implementation (from current version):
```csharp
public void PersistReplayDataCapsule(IPlayerAction playerAction) {
    var replayData = new ReplayData(); // creates an empty static state
    replayData.staticData = new List<IReplayStateData>() { 
        /* some stateful data (i. e. player data) */ 
    }; 
    var myDataCapsule = new ReplayDataCapsule() 
    {
        playerAction = (IReplayStateData) playerAction
        // ... desired state
    }
    replayData.UpdateState(replayData.stateHistory, myDataCapsule); // push state
}
```
