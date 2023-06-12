using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Game Scene";
    [SerializeField] private string mainMenuSceneName = "Main Menu";
    [SerializeField] private string watchReplaySceneName = "Replay Playback Scene";
    internal static LevelManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            // renew instance to keep references in the scene
            Destroy(instance.gameObject);
            instance = this;
        }
    }

    public void LoadStartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void LoadWatchReplayScene()
    {
        SceneManager.LoadScene(watchReplaySceneName);
    }
}
