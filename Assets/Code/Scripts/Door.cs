using System;
using DG.Tweening;
using UnityEngine;

public class Door : Interactable
{
    private bool isOpen = false;

    private void Open()
    {
        transform.DORotate(new Vector3(0f, -180f, 0f), 1f);
    }

    private void Close()
    {
        transform.DORotate(new Vector3(0f, -90f, 0f), 1f);
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
