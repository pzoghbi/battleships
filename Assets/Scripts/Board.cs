using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] internal Transform boardRoot;
    internal BoardData tileBoardData;
    internal Dictionary<int, BoardTile> boardTiles = new Dictionary<int, BoardTile>();

    [SerializeField] private BoardTile boardTilePrefab;
    [SerializeField] private bool interactable;

    internal void Awake()
    {
        tileBoardData = ScriptableObject.CreateInstance<BoardData>();
        CreateAndPlaceTilesOnGrid();
    }

    internal void LoadBoardData(BoardData boardData)
    {
        BoardData.WalkGrid((byte col, byte row) =>
        {
            var tileInstance = boardTiles[tileBoardData.grid[col, row]];
            tileInstance.TileType = boardData.grid[col, row];
        });
    }

    private void CreateAndPlaceTilesOnGrid()
    {
        BoardData.WalkGrid((byte col, byte row) =>
        {
            var boardTile = Instantiate(boardTilePrefab, boardRoot);
            boardTile.interactable = interactable;
            boardTile.gridPosition = new Vector2Int(col, row);

            int boardTileId = boardTile.GetInstanceID();
            tileBoardData.grid[col, row] = boardTileId;
            boardTiles.Add(boardTileId, boardTile);
        });
    }
}
