using System;
using Code.Scripts;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Interactable lastInteractable;
    bool currentlySlicing = false;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            
            // If it's something that can have an outline
            if (hitObject.GetComponent<Interactable>() is { } interactable)
            {
                // If it's a lemon slice
                if (interactable.GetComponent<LemonSlice>())
                {
                    interactable.HideOutline();
                }
                else
                {
                    interactable.ShowOutline();
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (interactable.GetComponent<LemonSlicer>() is { } lemonSlicer)
                    {
                        lemonSlicer.BeginSlicing();
                    }
                }
                
                // Assign it for later
                lastInteractable = interactable;
            }
            else
            {
                if (lastInteractable) lastInteractable.HideOutline();
            }
        }
        else
        {
            if (lastInteractable) lastInteractable.HideOutline();
            
            
        }
    }
}
