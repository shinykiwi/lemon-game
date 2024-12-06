using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Buttons
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;
    
    
    // Sounds
    [Header("Sounds")]
    [SerializeField] private AudioSource soundEffects;
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip back;
    
    // Main menu config
    [Header("Settings")] 
    [Tooltip("Scene to load upon play, if any. Will hide the menu instead if no scene asset.")]
    [SerializeField] private SceneAsset scene;
    
    // Main menu itself
    private Canvas canvas;


    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        // If there's no audio source or there's no audio clip then skip this part
        if (music == null || music.clip == null)
        {
            return;
        }

        // If not already playing, then play the main menu music
        if (!music.isPlaying)
        {
            music.Play();
        }
    }

    /// <summary>
    /// Hides the MainMenu.
    /// </summary>
    private void Hide()
    {
        canvas.enabled = false;
    }

    /// <summary>
    /// Shows the MainMenu.
    /// </summary>
    private void Show()
    {
        canvas.enabled = true;
    }

    /// <summary>
    /// Plays the click sound.
    /// </summary>
    private void PlayClickSound()
    {
        soundEffects.clip = click;
        if (!soundEffects.isPlaying)
        {
            soundEffects.Play();
        }
    }

    /// <summary>
    /// Plays the back sound.
    /// </summary>
    private void PlayBackSound()
    {
        soundEffects.clip = back;
        if (!soundEffects.isPlaying)
        {
            soundEffects.Play();
        }
    }

    /// <summary>
    /// Executes when the play button is clicked, loads the specified scene (if any).
    /// </summary>
    public void OnPlayButton()
    {
        PlayClickSound();
        if (scene)
        {
            SceneManager.LoadScene(scene.name);
        }
        else
        {
            Hide();
        }
    }

    public void OnCreditsButton()
    {
        PlayClickSound();
    }

    public void OnOptionsButton()
    {
        PlayClickSound();
    }

    public void OnQuitButton()
    {
        PlayClickSound();
    }
}
