using System.Collections.Generic;
using UnityEngine;
using static BoardData;

public static class RandomGridBattleshipPlacer
{
    internal const byte extrude = 1;

    private static BoardData abstractBoard;

    private static BoardData CleanBoard
    {
        get
        {
            if (abstractBoard == null)
            {
                abstractBoard = new BoardData();
            }
            else
            {
                abstractBoard.ClearBoard();
            }

            return abstractBoard;
        }
    }

    internal static void RandomlyArrangeBattleships(List<BattleshipData> battleshipsData)
    {
        abstractBoard = CleanBoard;

        battleshipsData.ForEach((battleshipData) =>
        {
            if (Random.Range(0, 2) == 0) battleshipData.Flip();

            if (!TryPlaceBattleshipOnGrid(battleshipData))
            {
                battleshipData.Flip();

                if (!TryPlaceBattleshipOnGrid(battleshipData))
                {
                    RandomlyArrangeBattleships(battleshipsData);
                }
            }
        });

        //PrintBoard();
    }

    private static bool TryPlaceBattleshipOnGrid(BattleshipData battleshipData)
    {
        Vector2Int[] freePoints = GetAvailableGridPointsFromArea(battleshipData.gridWidth, battleshipData.gridHeight);

        if (freePoints.Length > 0)
        {
            Vector2Int randomPointOnGrid = freePoints[Random.Range(0, freePoints.Length)];
            battleshipData.gridPosition = new Vector2Int(randomPointOnGrid.x, randomPointOnGrid.y);
            PlaceBattleshipPartsOnGrid(battleshipData, (byte) randomPointOnGrid.x, (byte) randomPointOnGrid.y);
            return true;
        }
        else
        {
            return false;
        }
    }

    private static void PlaceBattleshipPartsOnGrid(BattleshipData battleshipData, byte startCol, byte startRow)
    {
        int partIndex = 0;
        for (byte j = startRow; j < startRow + battleshipData.gridHeight; j++)
        {
            for (byte i = startCol; i < startCol + battleshipData.gridWidth; i++)
            {
                var battleshipPart = battleshipData.battleshipParts[partIndex++];
                battleshipPart.gridPosition = new SerializableVector2Int(i, j);

                // mark this part of the grid as occupied
                abstractBoard.grid[i, j] = (int) BoardTileType.Ship;
            }
        }
    }

    private static Vector2Int[] GetAvailableGridPointsFromArea(byte width, byte height)
    {
        List<Vector2Int> points = new List<Vector2Int>();

        for (byte row = 0; row <= boardSize - height; row++)
        {
            for (byte col = 0; col <= boardSize - width; col++)
            {
                if (CanPlaceShipHere(col, row, width, height))
                {
                    points.Add(new Vector2Int(col, row));
                }
            }
        }

        return points.ToArray();
    }

    private static bool CanPlaceShipHere(byte col, byte row, byte width, byte height)
    {
        if (col + width > boardSize || row + height > boardSize) return false; // skip calculations

        byte minCol = (byte) Mathf.Clamp(col - extrude, 0, boardSize);
        byte maxCol = (byte) Mathf.Clamp(col + width + extrude, 0, boardSize);
        byte minRow = (byte) Mathf.Clamp(row - extrude, 0, boardSize);
        byte maxRow = (byte) Mathf.Clamp(row + height + extrude, 0, boardSize);

        byte spacesOccupied = 0;

        for (byte j = minRow; j < maxRow; j++)
        {
            for (byte i = minCol; i < maxCol; i++)
            {
                if (abstractBoard.grid[i, j] != (int) BoardTileType.Empty)
                {
                    spacesOccupied++;
                }
            }
        }

        return spacesOccupied == 0;
    }

    private static void PrintBoard()
    {
        string board_grid = "";
        for (int j = boardSize - 1; j >= 0; j--)
        {
            for (int i = 0; i < boardSize; i++)
                board_grid += (abstractBoard.grid[i, j] != (int) BoardTileType.Empty) ? "L" : "E";

            board_grid += "\n";
        }

        Debug.Log(board_grid);
    }
}