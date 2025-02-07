using System;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Player Sounds")]
    [SerializeField] private AudioClip pickUpSound;
    [SerializeField] private AudioClip putBackSound;
    [SerializeField] private AudioClip takeFromBoxSound;
    
    [Header("Lemon Sounds")]
    [SerializeField] private AudioClip[] lemonSqueezeSounds;
    [SerializeField] private AudioClip lemonSliceSound;
    

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

    private void Play(AudioClip[] clips)
    {
        if (!audioSource.isPlaying)
        {
            int randomIndex = UnityEngine.Random.Range(0, clips.Length);
            audioSource.clip = clips[randomIndex];
            audioSource.Play();
        }
    }

    private void PlayInterrupt(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void PlayInterrupt(AudioClip[] clips)
    {
        int randomIndex = UnityEngine.Random.Range(0, clips.Length);
        audioSource.clip = clips[randomIndex];
        audioSource.Play();
    }

    public void PickUp()
    {
        Play(pickUpSound);
    }

    public void PutBack()
    {
        Play(putBackSound);
    }

    public void SqueezeLemon()
    {
        PlayInterrupt(lemonSqueezeSounds);
    }

    public void SliceLemon()
    {
        Play(lemonSliceSound);
    }

    public void TakeFromBox()
    {
        Play(takeFromBoxSound);
    }

    
}
