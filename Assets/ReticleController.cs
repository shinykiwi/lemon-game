using System;
using UnityEngine;
using UnityEngine.UI;

public class ReticleController : MonoBehaviour
{
    private Canvas canvas;
    
    [SerializeField] private Image reticle;
    [SerializeField] private Slider radialSlider;

    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
    }

    public void ToggleReticle()
    {
        reticle.enabled = !canvas.enabled;
    }

    public void ShowReticle()
    {
        reticle.enabled = true;
    }

    public void HideReticle()
    {
        reticle.enabled = false;
    }

    public void ShowRadialSlider()
    {
        radialSlider.gameObject.SetActive(true);
    }

    public void HideRadialSlider()
    {
        radialSlider.gameObject.SetActive(false);
    }

    public void SetRadialSliderValue(float value)
    {
        radialSlider.value = value;
    }

    public float GetRadialSliderValue()
    {
        return radialSlider.value;
    }
}
