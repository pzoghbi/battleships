using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BoardData;

[Serializable]
public class BattleshipData
{
    [NonSerialized] public Battleship prefab;
    public SerializableVector2Int gridPosition;
    public List<BattleshipPartData> battleshipParts = new List<BattleshipPartData>();
    public byte gridWidth;
    public byte gridHeight;
    public bool IsWrecked => battleshipParts.All(part => part.isHit);
    public bool isFlipped = false;

    [JsonConstructor]
    public BattleshipData(byte gridWidth, byte gridHeight, bool isFlipped, SerializableVector2Int gridPosition)
    {
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
        this.gridPosition = new Vector2Int(gridPosition.x, gridPosition.y);
        this.isFlipped = isFlipped;

        CreateBattleshipParts();
    }

    public BattleshipData(BattleshipDataSO blueprint)
    {
        this.gridWidth = blueprint.gridWidth;
        this.gridHeight = blueprint.gridHeight;
        this.prefab = blueprint.prefab;


        CreateBattleshipParts();
    }

    internal void Flip()
    {
        (gridWidth, gridHeight) = (gridHeight, gridWidth);
        isFlipped = !isFlipped;
    }

    internal void RevealBattleshipData(BoardData boardData)
    {
        var extrude = RandomGridBattleshipPlacer.extrude;
        var emptyTileType = (int) BoardTileType.Empty;
        var flagTileType = (int) BoardTileType.Flag;

        foreach (var battleshipPart in battleshipParts)
        {
            var minCol = ClampToBoardSize(battleshipPart.gridPosition.x - extrude);
            var maxCol = ClampToBoardSize(battleshipPart.gridPosition.x + extrude);
            var minRow = ClampToBoardSize(battleshipPart.gridPosition.y - extrude);
            var maxRow = ClampToBoardSize(battleshipPart.gridPosition.y + extrude);

            for (byte row = minRow; row <= maxRow; row++)
                for (byte col = minCol; col <= maxCol; col++)
                    if (boardData.grid[col, row] == emptyTileType)
                        boardData.grid[col, row] = flagTileType;
        }
    }

    private void CreateBattleshipParts()
    {
        battleshipParts.Clear();
        var size = gridWidth * gridHeight;

        for (byte i = 0; i < size; i++)
        {
            var battleshipPart = new BattleshipPartData();
            battleshipPart.battleshipData = this;
            battleshipParts.Add(battleshipPart);
        }
    }

}