using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ReplayPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var replayData = ReplayData.LoadLastReplayFile();
        Debug.Log(replayData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator PlayMove()
    {
        yield return null;
    }
}
