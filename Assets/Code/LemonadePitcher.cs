using System;
using Unity.Cinemachine;
using UnityEngine;

public class LemonadePitcher : Interactable
{
    [SerializeField] private CinemachineCamera camera;
    [SerializeField] private Transform lemonSpawnPoint;
    [SerializeField] private Transform woodSpoonSpawnPoint;

    private float lemonJuice = 0f;
    

    public void EnterStirringMode()
    {
        camera.enabled = true;
    }

    public void AddLemonJuice(float juice)
    {
        lemonJuice += juice;
    }
   

    public Transform EnterSqueezingMode()
    {
        camera.enabled = true;
        DisableInteract();
        
        return lemonSpawnPoint;
    }
    
    // ----- Exits -----

    public void ExitSqueezingMode()
    {
        camera.enabled = false;
        EnableInteract();
    }
    
    public void ExitStirringMode()
    {
        camera.enabled = false;
    }
    
}
