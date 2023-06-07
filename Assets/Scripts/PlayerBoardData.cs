using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoardData : BoardData
{
    private const byte extrude = 1;
    internal List<BattleshipData> ships = new List<BattleshipData>();
    internal Dictionary<int, ShipPart> shipParts = new Dictionary<int, ShipPart>();

    private void Awake()
    {
        InitializeAndGenerateBoardGrid();
    }

    // Creates Player Board with Ships and Empty Spaces
    private void InitializeAndGenerateBoardGrid()
    {
        InitializeBoard();
        GenerateBoard(BattleManager.instance.battleSettings);
        //PrintBoard();
    }

    void GenerateBoard(BattleshipGameSettings gameSettings)
    {
        foreach (var shipBlueprint in gameSettings.ships)
        {
            var ship = Instantiate(shipBlueprint);

            if (Random.Range(0, 2) == 0) ship.Flip();

            if (!AttemptPlaceShip(ship))
            {
                ship.Flip();

                if (!AttemptPlaceShip(ship))
                {
                    Debug.LogError("Ship cannot be placed.. restarting");
                    BattleManager.instance.RestartBattle();
                }
            }
        }
    }

    private bool AttemptPlaceShip(BattleshipData ship)
    {
        Vector2Int[] freePoints = GetPointsFromFreeAreas(ship.gridWidth, ship.gridHeight);

        if (freePoints.Length > 0)
        {
            Vector2Int randomPoint = freePoints[Random.Range(0, freePoints.Length)];
            PlaceShipOnGrid(ship, (byte)randomPoint.x, (byte)randomPoint.y);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void PlaceShipOnGrid(BattleshipData ship, byte col, byte row)
    {
        ship.gridPosition = new Vector2Int(col, row);
        
        for (byte j = row; j < row + ship.gridHeight; j++)
        {
            for (byte i = col; i < col + ship.gridWidth; i++)
            {

                // create ship part data and store
                var shipPart = CreateInstance<ShipPart>();
                var shipPartID = shipPart.GetInstanceID();

                shipPart.gridPosition = new Vector2Int(i, j);
                shipPart.ship = ship;

                shipParts.Add(shipPartID, shipPart);
                ship.shipParts.Add(shipPart);

                grid[i, j] = shipPartID; // store ship part id directly in grid
            }
        }
        
        ships.Add(ship);
    }
    
    private Vector2Int[] GetPointsFromFreeAreas(byte width, byte height)
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

    private bool CanPlaceShipHere(byte col, byte row, byte width, byte height)
    {
        if (col + width > boardSize || row + height > boardSize) return false; // skip calculations

        byte minCol = (byte)Mathf.Clamp(col - extrude, 0, boardSize);
        byte maxCol = (byte)Mathf.Clamp(col + width + extrude, 0, boardSize);

        byte minRow = (byte)Mathf.Clamp(row - extrude, 0, boardSize);
        byte maxRow = (byte)Mathf.Clamp(row + height + extrude, 0, boardSize);

        byte spacesOccupied = 0;

        for (byte j = minRow; j < maxRow; j++)
        {
            for (byte i = minCol; i < maxCol; i++)
            {
                if (grid[i, j] != (int) BoardTileType.Empty)
                {
                    spacesOccupied++;
                }
            }
        }

        return spacesOccupied == 0;
    }

    internal void PrintBoard()
    {
        string board_grid = "";
        for (int j = boardSize - 1; j >= 0; j--)
        {
            for (int i = 0; i < boardSize; i++)
                board_grid += (grid[i, j] != (int) BoardTileType.Empty) ? "L" : "E";

            board_grid += "\n";
        }

        Debug.Log(board_grid);
    }
}
