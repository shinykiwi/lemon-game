using System;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip pickUpSound;
    [SerializeField] private AudioClip putBackSound;
    [SerializeField] private AudioClip takeFromBoxSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Play(AudioClip clip)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void PickUp()
    {
        Play(pickUpSound);
    }

    public void PutBack()
    {
        Play(putBackSound);
    }

    public void TakeFromBox()
    {
        Play(takeFromBoxSound);
    }

    
}
