using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSoundOnAwake : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    void Start()
    {
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)], 1);
    }

}
