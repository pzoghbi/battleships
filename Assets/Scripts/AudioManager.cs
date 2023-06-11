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
        instance = this;    
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
    
    private void Start()
    {
        if (!audioSource.isPlaying)
            Play(music);
    }

    internal static void Play(AudioClip clip)
    {
        instance.audioSource.PlayOneShot(clip);
    }
}
