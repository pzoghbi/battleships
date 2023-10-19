using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Exposed
    [SerializeField] internal bool autoPlay = false;

    internal PlayerData[] PlayersData => playersData;

    // Private
    [SerializeField] private ReplayPlayer replayPlayer;
    private PlayerData[] playersData;
    private List<BoardTile> boardTiles = new List<BoardTile>();

    private void Start()
    {
        boardTiles = CacheInteractableBoardTiles();
    }

    private void Update()
    {
        ResetBoardTiles();
        HandleBoardTileSelection();
    }

    private void HandleBoardTileSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            BoardTile tileHit = hit.collider.gameObject.GetComponent<BoardTile>();

            if (!tileHit) return;
            if (!tileHit.Interactable) return;

            tileHit.MarkSelected();

            if (Input.GetMouseButtonDown(0))
            {
                var playerAction = new GridSelectionPlayerAction(tileHit.gridPosition);
                ProcessPlayerAction(playerAction);
            }
        }
    }

    private PlayerData[] InitializePlayersData(byte playerCount, BattleshipGameSettingsSO settings)
    {
        var playersData = new PlayerData[playerCount];

        for (byte playerIndex = 0; playerIndex < playerCount; playerIndex++)
            playersData[playerIndex] = new PlayerData(settings);

        return playersData;
    }

    private PlayerData[] InitializePlayersDataFromReplay() {
        return replayPlayer.replayData.staticData
                .Select(data => (PlayerData) data).ToArray();
    }

    private List<BoardTile> CacheInteractableBoardTiles()
    {
        var boardTileList = new List<BoardTile>();
        foreach (var boardTile in FindObjectsOfType<BoardTile>())
        {
            if (boardTile.Interactable)
            {
                boardTileList.Add(boardTile);
            }
        }
        return boardTileList;
    }

    private void ResetBoardTiles()
    {
        boardTiles.ForEach(boardTile => boardTile.ResetMaterial());
    }

    internal void InitializePlayers(BattleshipGameSettingsSO gameSettings)
    {
        if (replayPlayer)
        {
            playersData = InitializePlayersDataFromReplay();
        }
        else
        {
            playersData = InitializePlayersData(
                GameManager.playerCount, gameSettings);
        }
    }

    internal void ProcessPlayerAction(IPlayerAction playerAction)
    {
        playerAction.Execute();

        FindObjectOfType<ReplayRecorder>()?
            .PlayerActionComplete?
                .Invoke(playerAction);
    }
}