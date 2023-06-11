using System;
using UnityEngine;

[Serializable]
public class PlayerData : IReplayStateData
{
    public BoardData playerMovesData;
    public PlayerBattleshipsData playerBattleshipsData;
    public uint score = 0;

    public PlayerData(BattleshipGameSettingsSO gameSettings)
    {
        playerBattleshipsData = new PlayerBattleshipsData(gameSettings);
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

    public string typeName => GetType().Name;
}