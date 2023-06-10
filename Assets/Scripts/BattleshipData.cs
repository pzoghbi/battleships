using System.Collections.Generic;
using System.Linq;
using static BoardData;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battleship", menuName = "Battleships/New battleship")]
public class BattleshipData : ScriptableObject
{
    [SerializeField] public Battleship prefab;
    [SerializeField] internal byte gridWidth;
    [SerializeField] internal byte gridHeight;

    internal Vector2Int gridPosition;
    internal List<BattleshipPartData> battleshipParts = new List<BattleshipPartData>();
    internal bool isFlipped = false;

    internal bool IsWrecked => battleshipParts.All(battleshippart => battleshippart.isHit);

    public void Flip()
    {
        (gridWidth, gridHeight) = (gridHeight, gridWidth);
        isFlipped = !isFlipped;
    }

    private void Awake()
    {
        CreateBattleshipParts();
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
}
