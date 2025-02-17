using System;
using Code.Scripts;
using UnityEngine;
using UnityEngine.VFX;

public class Sink : Interactable
{

    [SerializeField] private VisualEffect vfx;
    private bool tapOn = false;

    private void Start()
    {
        TurnOffTap();
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

    public void TurnOnTap()
    {
        vfx.Play();
    }
    
    public void TurnOffTap()
    {
        vfx.Stop();
    }

    public bool IsTapOn()
    {
        return tapOn;
    }
}
