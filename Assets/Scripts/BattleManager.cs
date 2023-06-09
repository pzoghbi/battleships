using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    internal static BattleManager instance;
    [SerializeField] CinemachineImpulseSource impulseSource;
    [SerializeField] private DisplayGameMessage displayGameMessage;
    [SerializeField] internal BattleshipGameSettings battleSettings;
    [SerializeField] internal PlayerBoard playerBoard;
    [SerializeField] internal Board battleBoard;
    [SerializeField] string[] destroyShipTexts ;

    internal int turn = 0;
    internal bool isGameOver = false;

    private PlayerData[] players;
    private PlayerData ActivePlayer => players[(turn + 1) % playerCount];
    private PlayerData OtherPlayer => players[turn % playerCount];
    private string TurnText => "Player " + (currentPlayerIndex + 1).ToString() + " wins";
    internal bool AllowInput {
        get => !isGameOver && allowInput;
        private set => allowInput = value;
    }

    private string DestroyShipText => destroyShipTexts[Random.Range(0, destroyShipTexts.Length)] ?? "";

    private bool allowInput = false;
    private float delayBetweenTurns = 2;
    private byte currentPlayerIndex = 0;
    private const byte playerCount = 2;
    private Coroutine endTurnCoroutine;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitializePlayers();
        AdvanceTurn();
    }

    private void InitializePlayers()
    {
        players = new PlayerData[playerCount];

        for (byte playerIndex = 0; playerIndex < playerCount; playerIndex++)
            players[playerIndex] = ScriptableObject.CreateInstance<PlayerData>();
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
            }

            ActivePlayer.playerMovesData.grid[gridPosition.x, gridPosition.y] 
                = (int) BoardData.BoardTileType.Hit;

            if (OtherPlayer.CheckGameOver())
            {
                displayGameMessage.ShowText(TurnText);
                isGameOver = true;
            }

            ShakeCamera();
            var tileId = battleBoard.tileBoardData.grid[gridPosition.x, gridPosition.y];
            battleBoard.boardTiles[tileId].PlayExplosionParticles();
            // play sound

            endTurn = false;
        }
        else
        {
            ActivePlayer.playerMovesData.grid[gridPosition.x, gridPosition.y] 
                = (int) BoardData.BoardTileType.Miss;
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
        AdvanceTurn();
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
        AllowInput = true;
        currentPlayerIndex = (byte) ((turn + 1) % 2);
        displayGameMessage.ShowText("Player " + (currentPlayerIndex + 1).ToString() + "'s turn");
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ShakeCamera()
    {
        var impulseVelocity = new Vector2(
            Random.Range(0f, 0.5f), 
            Random.Range(0f, 0.5f));
        impulseSource.GenerateImpulseWithVelocity(impulseVelocity);
    }
}
