using System;
using UnityEngine;

public class ReticleController : MonoBehaviour
{
    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
    }

    public void ToggleReticle()
    {
        canvas.enabled = !canvas.enabled;
    }

    public void ShowReticle()
    {
        canvas.enabled = true;
    }

    public void HideReticle()
    {
        canvas.enabled = false;
    }
}
