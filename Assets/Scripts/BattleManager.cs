using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class BattleManager : MonoBehaviour
{
    [SerializeField] internal BattleshipGameSettings battleSettings;
    [SerializeField] internal PlayerBoard playerBoard;
    [SerializeField] internal Board battleBoard;

    internal static BattleManager instance;
    internal int turn = 0;
    internal bool isGameOver = false;
    internal bool AllowInput {
        get => !isGameOver && allowInput;
        private set => allowInput = value;
    }

    [SerializeField] private DisplayGameMessage displayGameMessage;
    [SerializeField] private string[] destroyShipTexts;

    private CinemachineImpulseSource impulseSource;
    private Coroutine endTurnCoroutine;
    private PlayerData[] players;
    private PlayerData ActivePlayer => players[(turn + 1) % playerCount];
    private PlayerData OtherPlayer => players[turn % playerCount];
    private string DestroyShipText => destroyShipTexts[Random.Range(0, destroyShipTexts.Length)] ?? "";
    private string WinText => "Player " + (currentPlayerIndex + 1).ToString() + " wins";
    private string TurnText => "Player " + (currentPlayerIndex + 1).ToString() + "'s turn";
    private bool allowInput = true;
    private float delayBetweenTurns = 2;
    private float gameOverDelay = 2;
    private float restartGameDelay = 3;
    private byte currentPlayerIndex = 0;
    private const byte playerCount = 2;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        InitializePlayers();
        AdvanceTurn();
        AudioManager.Play(AudioManager.instance.music);
    }

    private void InitializePlayers()
    {
        players = new PlayerData[playerCount];

        for (byte playerIndex = 0; playerIndex < playerCount; playerIndex++)
            players[playerIndex] = ScriptableObject.CreateInstance<PlayerData>();
    }

    public void ProcessPlayerAction(IPlayerAction playerAction)
    {
        playerAction.Execute();
    }
    
    internal void ProcessTileSelection(Vector2Int gridPosition)
    {
        if (!AllowInput) return;

        bool endTurn = true;

        var targetBattleshipPartData = CheckForTargetsHit(gridPosition);

        if (targetBattleshipPartData)
        {
            targetBattleshipPartData.isHit = true;

            if (targetBattleshipPartData.battleshipData.IsWrecked)
            {
                targetBattleshipPartData.battleshipData.RevealBattleshipData(ActivePlayer.playerMovesData);
                displayGameMessage.ShowText(DestroyShipText);
                AudioManager.Play(AudioManager.instance.sunkTargetSound);
            }

            ActivePlayer.playerMovesData.grid[gridPosition.x, gridPosition.y] 
                = (int) BoardData.BoardTileType.Hit;

            if (OtherPlayer.CheckGameOver())
            {
                isGameOver = true;
                StartCoroutine(GameOverRoutine());
            }

            ShakeCamera();
            var tileId = battleBoard.tileBoardData.grid[gridPosition.x, gridPosition.y];
            battleBoard.boardTiles[tileId].PlayExplosionParticles();
            AudioManager.Play(AudioManager.instance.hitTargetSound);

            endTurn = false;
        }
        else
        {
            ActivePlayer.playerMovesData.grid[gridPosition.x, gridPosition.y] 
                = (int) BoardData.BoardTileType.Miss;
            AudioManager.Play(AudioManager.instance.missTargetSound);
        }

        UpdateBoards();

        if (endTurn)
        {
            if (endTurnCoroutine != null) StopCoroutine(endTurnCoroutine);
            StartCoroutine(AdvanceTurnRoutine());
        }
    }

    private IEnumerator AdvanceTurnRoutine()
    {
        AllowInput = false;
        yield return new WaitForSeconds(delayBetweenTurns);
        AllowInput = true;
        AdvanceTurn();
    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(gameOverDelay);
        displayGameMessage.ShowText(WinText);
        AudioManager.Play(AudioManager.instance.victorySound);
        yield return new WaitForSeconds(restartGameDelay);
        RestartBattle();
    }

    private BattleshipPartData CheckForTargetsHit(Vector2Int gridPosition)
    {
        // todo this can be improved by caching battleshipparts
        // instance ids in separate grid + dictionary to access those
        var opponentBattleships = OtherPlayer.playerBattleshipsData.battleshipsData;
        foreach (var battleshipData in opponentBattleships)
        {
            foreach (var battleshipPart in battleshipData.battleshipParts)
            {
                if (battleshipPart.gridPosition == gridPosition)
                {
                    return battleshipPart;
                }
            }
        }

        return null;
    }

    private void AdvanceTurn()
    {
        turn += 1;
        currentPlayerIndex = (byte) ((turn + 1) % 2);
        displayGameMessage.ShowText(TurnText);
        UpdateBoards();
    }

    private void UpdateBoards()
    {
        // show my ships and opponent's moves
        playerBoard.LoadBoardData(OtherPlayer.playerMovesData, ActivePlayer.playerBattleshipsData);

        // show my moves on the interactable board
        battleBoard.LoadBoardData(ActivePlayer.playerMovesData);
    }

    internal void RestartBattle()
    {
        if (LevelManager.instance)
            LevelManager.instance.LoadMainMenu();
    }

    private void ShakeCamera()
    {
        var impulseVelocity = new Vector2(
            Random.Range(0f, 0.5f), 
            Random.Range(0f, 0.5f));
        impulseSource.GenerateImpulseWithVelocity(impulseVelocity);
    }
}
