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
    
    // Pages
    [Header("Pages")] 
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject menu;
    
    
    // Sounds
    [Header("Sounds")]
    [SerializeField] private AudioSource soundEffects;
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip back;
    
    // Main menu config
    [Header("Settings")] 
    [Tooltip("Scene to load upon play, if any. Will hide the menu instead if no scene asset.")]
    [SerializeField] private GameObject scene;

    [SerializeField] private Color[] _color;
    
    // Main menu itself
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        
        // Hide the credits menu and the options menu to start with
        credits.SetActive(false);
        options.SetActive(false);
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

    private void Update()
    {
        // If the player is in a menu (other than main menu)
        if (credits.activeSelf || options.activeSelf)
        {
            // If the escape key is pressed
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnBackButton(); // do the same thing as if the back button was clicked
            }
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

    /// <summary>
    /// Called by the credits button in the UI.
    /// </summary>
    public void OnCreditsButton()
    {
        PlayClickSound();
        
        // Show the credits menu
        credits.SetActive(true);
        
        // Hide the options and credits menu
        options.SetActive(false);
        menu.SetActive(false);

    }

    /// <summary>
    /// Called be the options button in the UI.
    /// </summary>
    public void OnOptionsButton()
    {
        PlayClickSound();
        
        // Show the options menu
        options.SetActive(true);
        
        // Hide the credits and main menu
        credits.SetActive(false);
        menu.SetActive(false);
    }

    /// <summary>
    /// Called by the quit button in the UI, quits the game in a build version.
    /// </summary>
    public void OnQuitButton()
    {
        PlayClickSound();
        
        // Quits the game
        Application.Quit();
    }

    /// <summary>
    /// Hides every menu except for the main menu.
    /// </summary>
    private void HideAllButMain()
    {
        credits.SetActive(false);
        options.SetActive(false);
        menu.SetActive(true);
    }

    /// <summary>
    /// Called when any back button is pressed in the UI.
    /// </summary>
    public void OnBackButton()
    {
        PlayBackSound();
        HideAllButMain();
    }
    
}
