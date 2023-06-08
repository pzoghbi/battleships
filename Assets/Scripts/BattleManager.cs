using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    internal static BattleManager instance;
    [SerializeField] internal BattleshipGameSettings battleSettings;
    [SerializeField] internal PlayerBoard playerBoard;
    [SerializeField] internal Board battleBoard;
    [SerializeField] CinemachineImpulseSource impulseSource;

    internal int turn = 0;
    internal bool isGameOver = false;

    private PlayerData[] players;
    private PlayerData ActivePlayer => players[(turn + 1) % playerCount];
    private PlayerData OtherPlayer => players[turn % playerCount];
    private const byte playerCount = 2;

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
        {
            players[playerIndex] = ScriptableObject.CreateInstance<PlayerData>();
        }
    }

    internal void ProcessTileSelection(Vector2Int gridPosition)
    {
        var targetBattleshipPartData = CheckForTargetsHit(gridPosition);

        if (targetBattleshipPartData)
        {
            targetBattleshipPartData.isHit = true;

            if (targetBattleshipPartData.battleshipData.IsWrecked)
            {
                targetBattleshipPartData.battleshipData.RevealBattleshipData(ActivePlayer.playerMovesData);
            }

            ActivePlayer.playerMovesData.grid[gridPosition.x, gridPosition.y] = (int) BoardData.BoardTileType.Hit;

            if (OtherPlayer.CheckGameOver())
            {
                // Handle game over
                Debug.Log("Game over for player: " + turn % 2);
            }

            // play FX
            ShakeCamera();
            // play explosion particles
            // play sound

            UpdateBoards();
            return;
        }
        else
        {
            ActivePlayer.playerMovesData.grid[gridPosition.x, gridPosition.y] = (int) BoardData.BoardTileType.Miss;
        }

        AdvanceTurn();
    }

    private BattleshipPartData CheckForTargetsHit(Vector2Int gridPosition)
    {
        // todo this can be improved by caching battleshipparts instance ids in separate grid + dictionary to access those
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
