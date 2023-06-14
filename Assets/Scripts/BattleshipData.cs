using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
    public BattleshipData() { }

    [OnDeserialized]
    public void OnDeserialized(StreamingContext ctx)
    {
        foreach(var battleshipPart in battleshipParts) 
        {
            battleshipPart.battleshipData = this;    
        }
    }

    public BattleshipData(BattleshipDataSO blueprint)
    {
        this.gridWidth = blueprint.gridWidth;
        this.gridHeight = blueprint.gridHeight;
        this.prefab = blueprint.prefab;

        CreateBattleshipParts();
    }

    // todo move to battleship utility
    internal void BindBattleshipPrefab()
    {
        var settings = GameManager.instance.gameSettings;
        this.prefab = settings.battleshipsBlueprintData.FirstOrDefault(battleship => {
            var gridWidthAndHeightMatch = 
                battleship.gridWidth == this.gridWidth
                && battleship.gridHeight == this.gridHeight;

            var gridWidthAndHeightMatchReversed =
                battleship.gridHeight == this.gridWidth
                && battleship.gridWidth == this.gridHeight;

            return gridWidthAndHeightMatch || gridWidthAndHeightMatchReversed;
        })?.prefab;
    }

    internal void Flip()
    {
        (gridWidth, gridHeight) = (gridHeight, gridWidth);
        isFlipped = !isFlipped;
    }

    // todo move to battleship utility
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