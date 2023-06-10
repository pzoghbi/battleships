using Cinemachine;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] internal BattleshipGameSettings battleSettings;
    [SerializeField] internal PlayerBoard playerBoard;
    [SerializeField] internal Board battleBoard;

    internal static GameManager instance;
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
    private float delayBetweenTurns = 2;
    private float gameOverDelay = 2;
    private float restartGameDelay = 3;
    private byte currentPlayerIndex = 0;
    private const byte playerCount = 2;
    private bool allowInput = true;

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

        var hitBattleshipPartData = CheckForTargetsHit(gridPosition);

        if (hitBattleshipPartData)
        {
            hitBattleshipPartData.isHit = true;

            if (hitBattleshipPartData.battleshipData.IsWrecked)
                DestroyBattleship(hitBattleshipPartData.battleshipData);

            SetMoveBoardAtGridPosition(gridPosition, BoardData.BoardTileType.Hit);

            if (OtherPlayer.CheckGameOver())
            {
                isGameOver = true;
                StartCoroutine(GameOverRoutine());
            }

            ShakeCamera();

            var tileId = battleBoard.tileBoardData.grid[gridPosition.x, gridPosition.y];
            battleBoard.boardTiles[tileId].PlayExplosionParticles();
        }
        else
        {
            SetMoveBoardAtGridPosition(gridPosition, BoardData.BoardTileType.Miss);
        }

        UpdateBoards();

        var clipToPlay = hitBattleshipPartData
            ? AudioManager.instance.hitTargetSound
            : AudioManager.instance.missTargetSound;
        AudioManager.Play(clipToPlay);

        if (hitBattleshipPartData) return;
        
        if (endTurnCoroutine != null) StopCoroutine(endTurnCoroutine);
        endTurnCoroutine = StartCoroutine(AdvanceTurnRoutine());
    }

    private void SetMoveBoardAtGridPosition(Vector2Int gridPosition, BoardData.BoardTileType typeToSet)
    {
        ActivePlayer.playerMovesData.grid[gridPosition.x, gridPosition.y] = (int) typeToSet;
    }

    private void DestroyBattleship(BattleshipData battleshipData)
    {
        battleshipData.RevealBattleshipData(ActivePlayer.playerMovesData);
        displayGameMessage.ShowText(DestroyShipText);
        AudioManager.Play(AudioManager.instance.sunkTargetSound);
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
        return OtherPlayer.playerBattleshipsData.battleshipsData
            .SelectMany(battleshipData => battleshipData.battleshipParts)
            .FirstOrDefault(battleshipPart => battleshipPart.gridPosition == gridPosition);
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