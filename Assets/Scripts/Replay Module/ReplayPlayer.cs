using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ReplayPlayer : MonoBehaviour
{

    [SerializeField] private bool playReplay = false;
    internal static bool isReplaying = false;
    internal uint replayTurn = 0;
    private List<ReplayDataCapsule> stateHistory;
    private bool isPaused = false;
    internal ReplayData replayData
    {
        get; private set;
    }

    private Coroutine playMoveCoroutine;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI pauseButtonTextMesh;
    string PauseButtonText => isPaused ? "Play" : "Pause";

    // Start is called before the first frame update
    void Awake()
    {
        isReplaying = playReplay;
        replayData = ReplayData.LoadLastReplayFile();
        stateHistory = StateHistoryFromReplay();
    }

    private void Start()
    {
        playMoveCoroutine = StartCoroutine(PlayMove());
    }

    private IEnumerator PlayMove()
    {
        var maxTurns = stateHistory.Count;

        while (replayTurn < maxTurns)
        {
            if (isPaused)
            {
                yield return null;
            } else
            {
                yield return new WaitForSeconds(1);

                var playerAction = GetPlayerAction(replayTurn);
                GameManager.instance.ProcessPlayerAction(playerAction);

                replayTurn += 1;

                yield return new WaitForSeconds(GameManager.instance.delayBetweenTurns);
            }
        }
    }

    private List<ReplayDataCapsule> StateHistoryFromReplay()
    {
        return replayData.stateHistory
            .Select(data => (ReplayDataCapsule) data).ToList();
    }

    private PlayerAction GetPlayerAction(uint move)
    {
        var actionPlayed = stateHistory[(int) move].actionPlayed;
        return (GridSelectionPlayerAction) actionPlayed;
    }

    public void Pause()
    {
        isPaused = !isPaused;
        pauseButtonTextMesh.text = PauseButtonText;
    }
}
