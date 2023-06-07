using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    [SerializeField] internal BattleshipGameSettings battleSettings;
    [SerializeField] internal PlayerBoard playerBoard;
    [SerializeField] internal BattleBoard battleBoard;

    internal PlayerData[] players;

    private byte playerCount = 2;
    internal int turn = 0;

    internal static BattleManager instance;

    internal PlayerData ActivePlayer
    {
        get
        {
            return players[turn % 2];
        }
    }

    private PlayerData ActivePlayerOpponent
    {
        get
        {
            return players[(turn+1) % 2];
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (!instance)
        {
            DontDestroyOnLoad(instance = this);
            SceneManager.sceneLoaded += OnSceneLoaded;
        } 
        else if (instance != this)
        {
            ReloadCache();
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartBattle();
    }

    private void StartBattle()
    {
        AdvanceTurn();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializePlayers();
        StartBattle();
    }

    private void InitializePlayers()
    {
        players = new PlayerData[playerCount];

        // set initial value to 0
        for (byte playerIndex = 0; playerIndex < playerCount; playerIndex++)
        {
            players[playerIndex] = ScriptableObject.CreateInstance<PlayerData>();
        }
    }

    internal void RestartBattle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ReloadCache()
    {
        instance.battleBoard = battleBoard;
        instance.playerBoard = playerBoard;
    }

    internal void ApplyMove(Vector2Int gridPosition)
    {
        var gridCell = ActivePlayerOpponent.playerBoardData.grid[gridPosition.x, gridPosition.y];
        if (ActivePlayerOpponent.playerBoardData.shipParts.ContainsKey(gridCell))
        {
            var shipPart = ActivePlayerOpponent.playerBoardData.shipParts.GetValueOrDefault(gridCell);
            if (shipPart != null)
            {
                shipPart.isHit = true;

                ActivePlayer.battleBoardData.grid[gridPosition.x, gridPosition.y] = 
                    (int) BoardData.BoardTileType.Hit;

                battleBoard.UpdateBoard();
                turn += 1;
                return;
            }
        }

        // todo store replay move

        AdvanceTurn();
    }

    private void AdvanceTurn()
    {
        instance.turn += 1;
        playerBoard.LoadBoardData(ActivePlayer.playerBoardData);
        battleBoard.LoadBoardData(ActivePlayer.battleBoardData);
    }
}
