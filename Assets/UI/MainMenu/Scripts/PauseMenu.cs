using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
   private Canvas canvas;
   private MenuAudio audio;

   [SerializeField] private GameObject options;
   [SerializeField] private GameObject pause;

   private void Awake()
   {
      canvas = GetComponent<Canvas>();
      audio = GetComponentInChildren<MenuAudio>();
      
      options.SetActive(false);
   }

   private void Toggle()
   {
      canvas.enabled = !canvas.enabled;
      
   }
   
   public void OnResumeButtonClick()
   {
      Toggle();
      audio.PlayClickSound();
   }

   public void OnOptionsButtonClick()
   {
      // Show the options panel
      options.SetActive(true);
      
      // Hide the pause panel
      pause.SetActive(false);
      
      audio.PlayClickSound();
   }

   public void OnBackButtonClick()
   {
      // Hide the options panel
      options.SetActive(false);
      
      // Show the pause panel
      pause.SetActive(true);
      
      audio.PlayBackSound();
   }

   public void OnQuitButtonClick()
   {
      audio.PlayBackSound();
      Application.Quit();
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Escape))
      {
         Toggle();
      }
   }
}
