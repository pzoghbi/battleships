using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBattleshipsData
{
    internal List<BattleshipData> battleshipsData = new List<BattleshipData>();
    internal bool HasBattleshipsLeft => battleshipsData.Any(battleship => !battleship.IsWrecked);

    public PlayerBattleshipsData()
    {
        CreateBattleships();
        RandomGridBattleshipPlacer.RandomlyArrangeBattleships(battleshipsData);
    }

    private void CreateBattleships()
    {
        foreach (var blueprint in GameManager.instance.gameSettings.battleshipsBlueprintData)
        {
            var newBattleshipData = new BattleshipData(blueprint);
            battleshipsData.Add(newBattleshipData);
        }
    }
}