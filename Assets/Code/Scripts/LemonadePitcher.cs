using System;
using Unity.Cinemachine;
using UnityEngine;

public class LemonadePitcher : Interactable
{
    [SerializeField] private CinemachineCamera camera;
    [SerializeField] private Transform lemonSpawnPoint;
    [SerializeField] private Transform woodSpoonSpawnPoint;
    [SerializeField] private Transform waterPitcherSpawnPoint;
    [SerializeField] private Transform sugarSpoonSpawnPoint;

    private float lemonJuice = 0f;
    private float sugar = 0f;
    

    public void EnterStirringMode()
    {
        camera.enabled = true;
    }

    public void AddLemonJuice(float juice)
    {
        lemonJuice += juice;
    }
    
    public void AddSugar(float s)
    {
        sugar += s;
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
    
    public Transform EnterSugarMode()
    {
        //camera.enabled = true;
        DisableInteract();
        return sugarSpoonSpawnPoint;
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

    public void Exit()
    {
        camera.enabled = false;
        EnableInteract();
    }
}
