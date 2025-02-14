using System;
using DG.Tweening;
using UnityEngine;

public class Door : Interactable
{
    private bool isOpen = false;
    [SerializeField] private AudioClip openDoor;
    [SerializeField] private AudioClip closeDoor;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Open()
    {
        transform.DORotate(new Vector3(0f, -180f, 0f), 1f);
        audioSource.clip = openDoor;
    }

    private void Close()
    {
        transform.DORotate(new Vector3(0f, -90f, 0f), 1f);
        audioSource.clip = closeDoor;
    }

    public void Use()
    {
        if (!isOpen)
        {
            Open();
            isOpen = true;
        }
        else
        {
            Close();
            isOpen = false;
        }
        
        if (!audioSource.isPlaying)
        {
            audioSource.Play(); 
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Open();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            Close();
        }
    }
}
