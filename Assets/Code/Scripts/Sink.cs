using System;
using System.Collections;
using Code.Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.VFX;

public class Sink : Interactable
{
  
    private bool tapOn = false;

    [SerializeField] private VisualEffect vfx;
    
    [Header("Sounds")]
    [SerializeField] private AudioClip tapOnSound;
    [SerializeField] private AudioClip tapOffSound;
    [SerializeField] private AudioClip tapLoopSound;
    
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.Log("Audio source is null");
        }
        
        TurnOffTap();

    }
    
    private IEnumerator StartWaterLoopAfter(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        audioSource.clip = tapLoopSound;
        audioSource.loop = true;
        audioSource.Play();

    }

    public void ToggleTap()
    {
        if (tapOn)
        {
            TurnOffTap();
            tapOn = false;
        }
        else
        {
            TurnOnTap();
            tapOn = true;
        }
    }

    private void TurnOnTap()
    {
        vfx.Play();
        audioSource.loop = false;
        audioSource.clip = tapOnSound;
        audioSource.Play();
        StartCoroutine(StartWaterLoopAfter(tapOnSound.length));
    }

    private void TurnOffTap()
    {
        vfx.Stop();
        audioSource.loop = false;
        audioSource.clip = tapOffSound;
        audioSource.Play();
    }

    public bool IsTapOn()
    {
        return tapOn;
    }
}
