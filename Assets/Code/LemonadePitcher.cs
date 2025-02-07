using System;
using Unity.Cinemachine;
using UnityEngine;

public class LemonadePitcher : Interactable
{
    [SerializeField] private CinemachineCamera camera;
    [SerializeField] private Transform lemonSpawnPoint;
    [SerializeField] private Transform woodSpoonSpawnPoint;
    [SerializeField] private Transform waterPitcherSpawnPoint;

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

    public Transform EnterPouringMode()
    {
        camera.enabled = true;
        DisableInteract();

        return waterPitcherSpawnPoint;
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
