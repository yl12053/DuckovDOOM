using System;
using UnityEngine;

namespace DuckovDOOM.Minigame;

public class AudioBehaviour: MonoBehaviour
{
    private AudioSource audioSource;
    public Action<float[]>? func = null;
    
    void Awake()
    {
        Debug.Log("Awake==");
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 1;
        AudioClip audioClip = AudioClip.Create(
            "DOOM",
            2048,
            2,
            44100,
            true,
            (reader) =>
            {
                Debug.Log("Generate");
                if (func == null)
                {
                    for (int i = 0; i < reader.Length; i++)
                    {
                        reader[i] = 0;
                    }
                }
                else
                {
                    Debug.Log("Generate--");
                    func(reader);
                }
            }
        );
        audioSource.clip = audioClip;
        
        audioSource.loop = true;
        audioSource.Play();
    }

    public void Play()
    {
        Debug.Log("Called play");
        audioSource.volume = 1;
        audioSource.loop = true;
        audioSource.Play();
    }
}