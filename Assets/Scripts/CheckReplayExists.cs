using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckReplayExists : MonoBehaviour
{
    [SerializeField] private Button watchReplayButton;

    // Start is called before the first frame update
    void Start()
    {
        watchReplayButton.interactable = new ReplayFileManager().ReplayExists;
    }
}
