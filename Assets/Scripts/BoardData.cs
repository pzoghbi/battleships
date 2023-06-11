using System;
using UnityEngine;

[Serializable]
public class BoardData : IReplayStateData
{
    public const byte boardSize = 10;
    public int[,] grid;

    public enum BoardTileType : byte
    {
        Empty,
        Ship,
        Miss,
        Hit,
        Normal,
        Flag
    }

    public BoardData()
    {
        this.grid = new int[boardSize, boardSize];
        ClearBoard();
    }

    internal void ClearBoard()
    {
        WalkGrid((byte col, byte row) =>
        {
            grid[col, row] = (int) BoardTileType.Empty;
        });
    }

    internal delegate void WalkAction(byte col, byte row);

    internal static void WalkGrid(WalkAction WalkAction)
    {
        for (byte row = 0; row < boardSize; row++)
        {
            for (byte col = 0; col < boardSize; col++)
            {
                WalkAction(col, row);
            }
        }
    }

    internal static byte ClampToBoardSize(int value)
    {
        return (byte) Mathf.Clamp(value, 0, boardSize - 1);
    }

    public string typeName => GetType().Name;
}