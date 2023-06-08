using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleshipsData : ScriptableObject
{
    internal List<BattleshipData> battleshipsData = new List<BattleshipData>();

    private void Awake()
    {
        CreateBattleships();
        RandomGridBattleshipPlacer.RandomlyArrangeBattleships(battleshipsData);
    }

    private void CreateBattleships()
    {
        foreach (var blueprint in BattleManager.instance.battleSettings.battleshipsBlueprintData)
        {
            var newBattleshipData = Instantiate(blueprint);
            battleshipsData.Add(newBattleshipData);
        }
    }

    internal bool HasBattleshipsLeft()
    {
        bool hasBattleshipsLeft = false;

        foreach (var battleshipData in battleshipsData)
        {
            if (!battleshipData.IsWrecked)
            {
                hasBattleshipsLeft = true;
                break;
            }
        }

        return hasBattleshipsLeft;
    }
}