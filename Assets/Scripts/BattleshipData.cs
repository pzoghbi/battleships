using System.Collections.Generic;
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

    internal bool IsWrecked
    {
        get
        {
            foreach (var part in battleshipParts)
            {
                if (!part.isHit) return false;
            }

            return true;
        }
    }

    public void Flip()
    {
        byte flip = gridWidth;
        gridWidth = gridHeight;
        gridHeight = flip;
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
            var battleshipPart = CreateInstance<BattleshipPartData>();
            battleshipPart.battleshipData = this;
            battleshipParts.Add(battleshipPart);
        }
    }

    internal void RevealBattleshipData(BoardData boardData)
    {
        foreach (var battleshipPart in battleshipParts)
        {
            var extrude = RandomGridBattleshipPlacer.extrude;

            var minCol = (byte) Mathf.Clamp(battleshipPart.gridPosition.x - extrude, 0, BoardData.boardSize - 1);
            var maxCol = (byte) Mathf.Clamp(battleshipPart.gridPosition.x + extrude, 0, BoardData.boardSize - 1);
            var minRow = (byte) Mathf.Clamp(battleshipPart.gridPosition.y - extrude, 0, BoardData.boardSize - 1);
            var maxRow = (byte) Mathf.Clamp(battleshipPart.gridPosition.y + extrude, 0, BoardData.boardSize - 1);

            for (byte row = minRow; row <= maxRow; row++)
                for (byte col = minCol; col <= maxCol; col++)
                    if (boardData.grid[col, row] == (int) BoardData.BoardTileType.Empty)
                        boardData.grid[col, row] = (int) BoardData.BoardTileType.Flag;
        }
    }
}
