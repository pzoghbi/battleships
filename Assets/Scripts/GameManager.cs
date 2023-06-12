using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] internal BattleshipGameSettingsSO gameSettings;
    [SerializeField] internal PlayerBoard playerBoard;
    [SerializeField] internal Board battleBoard;

    internal static GameManager instance;
    internal const byte playerCount = 2;
    internal uint turn = 0;
    internal uint absoluteTurn = 0;
    internal readonly float delayBetweenTurns = 2;
    internal bool isGameOver = false;
    internal PlayerData ActivePlayerData => playersData[(turn + 1) % playerCount];
    internal PlayerData OtherPlayerData => playersData[turn % playerCount];
    internal bool AllowInput {
        get => !isGameOver && allowInput;
        private set => allowInput = value && !ReplayPlayer.isReplaying;
    }

    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private ReplayRecorder replayRecorder;
    [SerializeField] private DisplayGameMessage displayGameMessage;
    [SerializeField] private string[] destroyShipTexts;
    [SerializeField] private bool allowInput = true;

    private CinemachineImpulseSource impulseSource;
    private Coroutine endTurnCoroutine;
    private PlayerData[] playersData;
    private float gameOverDelay = 2;
    private float restartGameDelay = 3;
    private byte currentPlayerIndex = 0;
    private string DestroyShipText => destroyShipTexts[Random.Range(0, destroyShipTexts.Length)] ?? "";
    private string WinText => "Player " + (currentPlayerIndex + 1).ToString() + " wins";
    private string TurnText => "Player " + (currentPlayerIndex + 1).ToString() + "'s turn";

    // Start is called before the first frame update
    void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        instance = this;
    }

    private void Start()
    {
        playerManager.InitializePlayers(gameSettings);
        playersData = playerManager.PlayersData;
        BindBattleshipPrefabs();
        if (replayRecorder && replayRecorder.recordGameplay) {
            replayRecorder.InitializeReplayData();
        }
        AdvanceTurn();
    }

    internal void BindBattleshipPrefabs()
    {
        foreach(var player in playersData)
        {
            foreach(var battleshipData in player.playerBattleshipsData.battleshipsData)
            {
                battleshipData.BindBattleshipPrefab();
            }
        }
    }

    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            await Task.Run(async () => await replayRecorder.SaveReplay());
        }
    }

    // todo move to playermanager
    public void ProcessPlayerAction(IPlayerAction playerAction)
    {
        playerAction.Execute();
        replayRecorder?.PlayerActionComplete?.Invoke(playerAction);
    }

    // todo move to playermanager
    internal async void ProcessTileSelection(Vector2Int gridPosition)
    {
        if (!AllowInput && !ReplayPlayer.isReplaying) return;

        var hitBattleshipPartData = CheckForTargetsHit(gridPosition);

        if (hitBattleshipPartData)
        {
            hitBattleshipPartData.isHit = true;

            if (hitBattleshipPartData.battleshipData.IsWrecked)
                DestroyBattleship(hitBattleshipPartData.battleshipData);

            SetMoveBoardAtGridPosition(gridPosition, BoardData.BoardTileType.Hit);

            if (OtherPlayerData.CheckGameOver())
            {
                isGameOver = true;
                StartCoroutine(GameOverRoutine());
                if (replayRecorder)
                {
                    if (replayRecorder.recordGameplay)
                        await Task.Run(async () => await replayRecorder.SaveReplay());
                }
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

        absoluteTurn += 1;

        if (hitBattleshipPartData) return;
        
        if (endTurnCoroutine != null) StopCoroutine(endTurnCoroutine);
        endTurnCoroutine = StartCoroutine(AdvanceTurnRoutine());
    }

    private void SetMoveBoardAtGridPosition(Vector2Int gridPosition, BoardData.BoardTileType typeToSet)
    {
        ActivePlayerData.playerMovesData.grid[gridPosition.x, gridPosition.y] = (int) typeToSet;
    }

    private void DestroyBattleship(BattleshipData battleshipData)
    {
        battleshipData.RevealBattleshipData(ActivePlayerData.playerMovesData);
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
        return OtherPlayerData.playerBattleshipsData.battleshipsData
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

    internal void UpdateBoards()
    {
        // show my ships and opponent's moves
        playerBoard.LoadBoardData(OtherPlayerData.playerMovesData, ActivePlayerData.playerBattleshipsData);

        // show my moves on the interactable board
        battleBoard.LoadBoardData(ActivePlayerData.playerMovesData);
    }

    internal void RestartBattle()
    {
        if (LevelManager.instance)
            LevelManager.instance.LoadMainMenu();
        else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void ShakeCamera()
    {
        var impulseVelocity = new Vector2(
            Random.Range(0f, 0.5f), 
            Random.Range(0f, 0.5f));
        impulseSource.GenerateImpulseWithVelocity(impulseVelocity);
    }
}
