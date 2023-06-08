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
        Normal,
        Flag
    }

    private void Awake()
    {
        InitializeBoard();
    }

    internal void InitializeBoard()
    {
        grid = new int[boardSize, boardSize];

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
}