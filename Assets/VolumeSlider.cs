using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    [SerializeField] private string parameterName;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        audioMixerGroup.audioMixer.GetFloat(parameterName, out var initialVolume);
        slider.value = initialVolume;
    }

    public void OnValueChange(float value)
    {
        audioMixerGroup.audioMixer.SetFloat(parameterName, value);
    }
}
