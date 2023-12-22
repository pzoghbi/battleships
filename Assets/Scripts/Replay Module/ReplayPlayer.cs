using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ReplayPlayer : MonoBehaviour
{
    internal static bool isReplaying = false;
    internal uint replayTurn = 0;

    [SerializeField] private TextMeshProUGUI pauseButtonTextMesh;
    [SerializeField] private bool playReplay = false;
    private ReplayFileManager fileManager = new ReplayFileManager();
    private List<ReplayDataCapsule> stateHistory;
    private bool isPaused = false;

    internal ReplayData replayData { get; private set; }
    private string PauseButtonText => isPaused ? "Play" : "Pause";

    void Awake()
    {
        isReplaying = playReplay;

        replayData = fileManager.LoadLastReplayFile();
        stateHistory = StateHistoryFromReplay();
    }

    private void Start()
    {
        StartCoroutine(PlayMove());
    }

    private IEnumerator PlayMove()
    {
        var maxTurns = stateHistory.Count;

        while (replayTurn < maxTurns)
        {
            yield return new WaitForSeconds(1);

            var playerAction = GetPlayerAction(replayTurn);
            GameManager.instance.ProcessPlayerAction(playerAction);

            replayTurn += 1;

            yield return new WaitForSeconds(GameManager.instance.delayBetweenTurns);
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

    private void HandleGamePaused()
    {
        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void Pause()
    {
        isPaused = !isPaused;
        pauseButtonTextMesh.text = PauseButtonText;

        HandleGamePaused();
    }
}
