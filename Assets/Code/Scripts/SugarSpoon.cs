using System;
using UnityEngine;

public class SugarSpoon : Interactable
{
    private bool hasSugar = false;
    [SerializeField] private GameObject sugar;
    private float defaultSugar = 5f;

    private void Start()
    {
        sugar.SetActive(false);
    }

    public void AddSugar()
    {
        hasSugar = true;
        sugar.SetActive(true);
    }

    public float RemoveSugar()
    {
        hasSugar = false;
        sugar.SetActive(false);
        return defaultSugar;
    }
    
    

    public bool HasSugar()
    {
        return hasSugar;
    }
}
