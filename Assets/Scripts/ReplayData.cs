using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class ReplayData
{
    //internal BattleshipGameSettingsDTO gameSettings;
    //internal List<PlayerDataDTO> playersData;
    //internal List<IPlayerAction> playerActions = new List<IPlayerAction>();

    //internal ReplayData(BattleshipGameSettings gameSettings, List<PlayerData> playersData)
    //{
    //    this.gameSettings = gameSettings;
    //    this.playersData = playersData;
    //}

    //internal bool SaveToFile()
    //{
    //    var path = Application.persistentDataPath;

    //    FileStream fileStream = new FileStream(path + "/replay.rdata", FileMode.Create);

    //    BinaryFormatter formatter = new BinaryFormatter();
    //    formatter.Serialize(fileStream, this);
        
    //    Debug.Log(fileStream);

    //    return false;
    //}
}