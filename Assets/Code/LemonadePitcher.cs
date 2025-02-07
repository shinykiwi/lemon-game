using System;
using Unity.Cinemachine;
using UnityEngine;

public class LemonadePitcher : Interactable
{
    [SerializeField] private CinemachineCamera camera;
    [SerializeField] private Transform lemonSpawnPoint;
    [SerializeField] private Transform woodSpoonSpawnPoint;
    

    public void EnterStirringMode()
    {
        camera.enabled = true;
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
    }
    
    public void ExitStirringMode()
    {
        camera.enabled = false;
    }
    
}
