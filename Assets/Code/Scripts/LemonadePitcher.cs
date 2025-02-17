using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.VFX;

public class LemonadePitcher : Interactable
{
    [SerializeField] private CinemachineCamera camera;
    [SerializeField] private Transform lemonSpawnPoint;
    [SerializeField] private Transform woodSpoonSpawnPoint;
    [SerializeField] private Transform waterPitcherSpawnPoint;
    [SerializeField] private Transform sugarSpoonSpawnPoint;

    [SerializeField] private GameObject[] liquidLayers;
    [SerializeField] private VisualEffect vfx;

    private Material liquidMaterial;
    private int layerCount = 0;

    private float lemonJuice = 0f;
    private float sugar = 0f;
    private float water = 0f;
    private float maxLiquid = 100f;

    private Color yellowColor = new Color(255, 202, 73);
    private Color waterColor = new Color(174, 219, 255);


    private void Start()
    {
        liquidMaterial = liquidLayers[0].GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void EnterStirringMode()
    {
        camera.enabled = true;
    }

    public void AddLemonJuice(float juice)
    {
        lemonJuice += juice;
        vfx.Play();
        UpdateLiquid();
    }
    
    public void AddSugar(float s)
    {
        sugar += s;
    }

    public void AddWater(float w)
    {
        water += w;
        UpdateLiquid();
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

    public void Exit()
    {
        camera.enabled = false;
        EnableInteract();
    }

    
    private void UpdateLiquid()
    {
        // This function is called when there is an update to the liquid content
        // So we can assume that there's been a change and thus a need to update the visuals

        // Get the total liquid amount that includes both water and lemon juice
        float totalLiquid = water + lemonJuice;
        
        // Get the interval value. (Ex. if there's 4 layers, max liquid is 100, this value would be 0.25)
        float interval = (maxLiquid / liquidLayers.Length) / 100;

         // If there's any liquid at all
         if (totalLiquid > 0)
         {
             // A loop for each layer number
             for (int i = 1; i <= liquidLayers.Length; i++)
             {
                 // Value will get bigger each layer you climb
                 // Ex. Layer 1 will be 0.16, layer 2 will be 0.32 etc.
                 float x = interval * (i);
        
                 // If the current liquid content is less than the liquid mandate for this interval
                 // Right interval to be in
                 if (totalLiquid < (maxLiquid * x))
                 {
                     if (i != layerCount)
                     {
                         AddLayer();
                         
                     }

                     break;
                 }
                 
                 // Otherwise, continue the loop till it finds the right interval
        
             }
        }
         
        UpdateLiquidColour();
        
    }

    private void UpdateLiquidColour()
    {
        foreach (GameObject layer in liquidLayers)
        {
            layer.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_BaseColor", yellowColor);
        }
    }

    private void AddLayer()
    {
        layerCount++;
        if ((layerCount-1) < liquidLayers.Length)
        {
            liquidLayers[layerCount-1].SetActive(true);
        }
    }
}
