using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    internal BoardData playerMovesData;
    internal PlayerBattleshipsData playerBattleshipsData;
    internal uint score = 0;

    public PlayerData()
    {
        playerBattleshipsData = new PlayerBattleshipsData();
        playerMovesData = new BoardData();
    }

    internal bool CheckGameOver()
    {
        return !playerBattleshipsData.HasBattleshipsLeft;
    }

    internal void UpdateScore(uint scoreToAdd)
    {
        score += scoreToAdd;
    }
}