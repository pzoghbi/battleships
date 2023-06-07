using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleBoard : MonoBehaviour
{
    [SerializeField] private BoardTile boardTilePrefab;
    [SerializeField] private Transform rootPoint;

    private BoardData tileBoardData;
    private Dictionary<int, BoardTile> boardTiles = new Dictionary<int, BoardTile>();

    private void Awake()
    {
        tileBoardData = ScriptableObject.CreateInstance<BoardData>();
        tileBoardData.InitializeBoard();
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateInteractableTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateInteractableTiles()
    {
        BoardData.WalkGrid((byte col, byte row) => { 
            var tile = Instantiate(boardTilePrefab, rootPoint);
            tile.tileType = (int) BoardData.BoardTileType.Normal;
            tile.gridPosition = new Vector2Int(col, row);

            var pivotOffset = new Vector3(0.5f, 0, 0.5f);
            tile.transform.localPosition = new Vector3(col, 0, row) + pivotOffset;

            var tileId = tile.GetInstanceID();

            tileBoardData.grid[col, row] = tileId;
            boardTiles.Add(tileId, tile);
        });
    }

    internal void LoadBoardData(BattleBoardData battleBoardData)
    {

    }

    internal void UpdateBoard()
    {
        BoardData.WalkGrid((byte col, byte row) => {
            var gridCell = tileBoardData.grid[col, row];
            if (boardTiles.ContainsKey(gridCell))
            {
                var tile = boardTiles.GetValueOrDefault(gridCell);
                tile.tileType = BattleManager.instance.ActivePlayer.battleBoardData.grid[col, row];
                tile.UpdateMaterial();
            }
        });
    }
}
