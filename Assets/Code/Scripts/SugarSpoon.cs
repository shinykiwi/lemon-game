using System;
using UnityEngine;

public class SugarSpoon : Interactable
{
    private bool hasSugar = false;
    [SerializeField] private GameObject sugar;

    private void Start()
    {
        sugar.SetActive(false);
    }

    public void AddSugar()
    {
        hasSugar = true;
        sugar.SetActive(true);
    }

    public void RemoveSugar()
    {
        hasSugar = false;
        sugar.SetActive(false);
    }

    public bool HasSugar()
    {
        return hasSugar;
    }
}
