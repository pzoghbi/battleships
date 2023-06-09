using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Sound FX")]
    [SerializeField] internal AudioClip hitTargetSound;
    [SerializeField] internal AudioClip missTargetSound;
    [SerializeField] internal AudioClip sunkTargetSound;
    [SerializeField] internal AudioClip victorySound;
    [SerializeField] internal AudioClip music;

    internal static AudioManager instance;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;    
    }

    internal static void Play(AudioClip clip)
    {
        instance.audioSource.PlayOneShot(clip);
    }
}
