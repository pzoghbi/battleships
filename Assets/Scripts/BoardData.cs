using UnityEngine;

public class BoardData : ScriptableObject
{
    internal const byte boardSize = 10;

    internal int[,] grid;
    internal enum BoardTileType : byte
    {
        Empty,
        Ship,
        Miss,
        Hit,
        Normal
    }

    internal void InitializeBoard()
    {
        grid = new int[boardSize, boardSize];

        for (byte row = 0; row < boardSize; row++)
        {
            for (byte column = 0; column < boardSize; column++)
            {
                grid[column, row] = (int) BoardTileType.Empty;
            }
        }
    }

    internal delegate void WalkAction(byte col, byte row);
    internal static void WalkGrid(WalkAction WalkAction)
    {
        for(byte row = 0; row < boardSize; row++) {
            for(byte col = 0; col < boardSize; col++)
            {
                WalkAction(col, row);
            }
        }
    }
}