using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    
    [SerializeField] private AudioSource audioData;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            PlaySound();
    }

    private void PlaySound()
    {
        if (!audioData.isPlaying)
        {
            audioData.Play();
        }
    }
}
