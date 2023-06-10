using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBattleshipsData : ScriptableObject
{
    internal List<BattleshipData> battleshipsData = new List<BattleshipData>();
    internal bool HasBattleshipsLeft => battleshipsData.Any(battleship => !battleship.IsWrecked);

    private void Awake()
    {
        CreateBattleships();
        RandomGridBattleshipPlacer.RandomlyArrangeBattleships(battleshipsData);
    }

    private void CreateBattleships()
    {
        foreach (var blueprint in GameManager.instance.battleSettings.battleshipsBlueprintData)
        {
            var newBattleshipData = Instantiate(blueprint);
            battleshipsData.Add(newBattleshipData);
        }
    }

}