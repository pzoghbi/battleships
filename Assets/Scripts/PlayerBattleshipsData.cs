using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerBattleshipsData
{
    public List<BattleshipData> battleshipsData = new List<BattleshipData>();
    public bool HasBattleshipsLeft => battleshipsData.Any(battleship => !battleship.IsWrecked);

    [JsonConstructor] public PlayerBattleshipsData() { }

    public PlayerBattleshipsData(BattleshipGameSettingsSO gameSettings)
    {
        CreateBattleships(gameSettings.battleshipsBlueprintData);
        RandomGridBattleshipPlacer.RandomlyArrangeBattleships(battleshipsData);
    }

    private void CreateBattleships(List<BattleshipDataSO> blueprints)
    {
        foreach (var blueprint in blueprints)
        {
            var newBattleshipData = new BattleshipData(blueprint);
            battleshipsData.Add(newBattleshipData);
        }
    }
}