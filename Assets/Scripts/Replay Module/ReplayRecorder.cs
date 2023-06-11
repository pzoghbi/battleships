using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Profiling;

public class ReplayRecorder : MonoBehaviour
{
    [SerializeField] internal bool recordGameplay = true;
    [SerializeField] private GameManager gameManager;
    internal UnityAction<IPlayerAction> PlayerActionComplete;
    private ReplayData replayData;

    private void Awake()
    {
        if (!gameManager) {
            gameManager = FindObjectOfType<GameManager>();
            if (!gameManager) return;
        }

        if (!recordGameplay) 
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        var playersDataCopy = new List<IReplayStateData> {
            gameManager.ActivePlayerData,
            gameManager.OtherPlayerData
        };

        // todo grid position (0, 0)
        playersDataCopy = playersDataCopy.DeepCopy();

        replayData = new ReplayData(playersDataCopy);

        PlayerActionComplete += PersistReplayDataCapsule;
    }

    public void PersistReplayDataCapsule(IPlayerAction playerAction)
    {
        var replayStateData = new ReplayDataCapsule()
        {
            timestamp = Time.fixedTime,
            turn = gameManager.absoluteTurn,
            actionPlayed = (IReplayStateData) playerAction,
            boardsState = new List<IReplayStateData>
            {
                gameManager.ActivePlayerData.playerMovesData,
                gameManager.OtherPlayerData.playerMovesData
            }.DeepCopy()
        };

        replayData.UpdateState(ref replayData.stateHistory, replayStateData);
    }

    public async Task<bool> SaveReplay()
    {
        return await replayData.SaveToFile();
    }

    private void OnDestroy()
    {
        if (recordGameplay)
            PlayerActionComplete -= PersistReplayDataCapsule;
    }

}
