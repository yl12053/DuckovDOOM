using System;
using UnityEngine;

namespace DuckovDOOM.Minigame;

public class AudioBehaviour: MonoBehaviour
{
    private AudioSource audioSource;
    public Action<float[], int>? func = null;
    
    void Awake()
    {
        Debug.Log("Awake");
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 1;
        audioSource.clip = null;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void Play()
    {
        Debug.Log("Called play");
        audioSource.volume = 1;
        audioSource.clip = null;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        Debug.Log("Audio Filter");
        func?.Invoke(data, channels);
    }
}