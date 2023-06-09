using UnityEngine;

public class PlayerData : ScriptableObject
{
    internal BoardData playerMovesData;
    internal PlayerBattleshipsData playerBattleshipsData;
    internal uint score = 0;

    private void Awake()
    {
        // create ships
        playerBattleshipsData = CreateInstance<PlayerBattleshipsData>();
        // initialize move board
        playerMovesData = CreateInstance<BoardData>();
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