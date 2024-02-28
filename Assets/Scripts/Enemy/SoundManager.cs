using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    public static SoundManager Instance => instance;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;


        audioSource = GetComponent<AudioSource>();  
    }


    public AudioSource GetAudioSource() { return audioSource; }

}
