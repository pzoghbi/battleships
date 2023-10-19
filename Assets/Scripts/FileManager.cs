using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    public static FileManager Instance { get; private set; }
    public static string DirectoryPath;
    public static DirectoryInfo DirectoryInfo;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        DirectoryPath = Application.persistentDataPath;
        DirectoryInfo = new DirectoryInfo(DirectoryPath);
    }
}
