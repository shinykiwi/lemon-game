using System;
using System.Collections;
using Code.Scripts;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Interactable lastInteractable;
    bool currentlySlicing = false;
    private Interactable itemInHand = null;
    private ReticleController reticleController;

    [SerializeField] private Transform hand;

    private void Start()
    {
        reticleController = GetComponentInChildren<ReticleController>();
    }

    /// <summary>
    /// Snaps the object to the players hand and turns off physics so it stays in place. Disables object's interaction.
    /// </summary>
    /// <param name="interactable"></param>
    private void AddToHand(Interactable interactable)
    {
        // Disable interaction for the picked up object
        interactable.DisableInteract();
        
        // Reset transforms
        GameObject obj;
        (obj = interactable.gameObject).transform.SetParent(hand);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        
        // Turn off physics
        obj.GetComponent<Rigidbody>().isKinematic = true;
        
        // // Make certain objects uninteractable
        // LemonSlicer slicer = FindFirstObjectByType<LemonSlicer>();
        // slicer.DisableInteract();
        
        itemInHand = interactable;
    }

    /// <summary>
    /// Detaches the object from the player's hand, turns on physics again so it falls.
    /// </summary>
    private void DropFromHand()
    {
        hand.DetachChildren();
        itemInHand.GetComponent<Rigidbody>().isKinematic = false;
        itemInHand.EnableInteract();
        itemInHand = null;
        
        //StartCoroutine(nameof(EnableGeneralInteraction));
        
    }

    private void SnapToCuttingBoard(Interactable interactable, LemonSlicer slicer)
    {
        // Disable interaction for the object
        interactable.DisableInteract();
        
        // Reset transforms
        GameObject obj;
        (obj = interactable.gameObject).transform.SetParent(slicer.GetObjectSpawn());
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        
        // Turn off physics
        obj.GetComponent<Rigidbody>().isKinematic = true;

        slicer.SetObjectToCut(interactable);

        itemInHand = null;
    }

    private IEnumerator EnableGeneralInteraction()
    {
        yield return null;
        
        // Make certain objects interactable again
        LemonSlicer slicer = FindFirstObjectByType<LemonSlicer>();
        slicer.EnableInteract();
    }

    /// <summary>
    /// Sends out a raycast, hits various objects and decides where to direct the logic.
    /// </summary>
    private void SearchByRaycast()
    {
        // Sending a raycast from the mouse position (which is already locked to middle of screen)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        // If something was hit
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            
            // If it's something that can have an outline
            if (hitObject.GetComponent<Interactable>() is { } interactable)
            {
                // If you can interact with it
                if (interactable.CanInteract())
                {
                    interactable.ShowOutline();

                    // Keep it for later
                    lastInteractable = interactable;
                    
                    // Only continue if there's no item in your hand already
                    if (!itemInHand)
                    {
                        // Right click
                        if (Input.GetMouseButtonDown(1))
                        {
                            // If it's a lemon, pick it up
                            if (interactable.GetComponent<LemonSlice>() is { } lemonSlice)
                            {
                                AddToHand(lemonSlice);
                            }

                            if (interactable.GetComponent<LemonSlicer>() is { } lemonSlicer)
                            {
                                lemonSlicer.EnterSliceMode();
                                
                            }
                        }
                    }
                    
                    // If you do have something in your hand
                    else
                    {
                        
                       //Debug.Log("I HAVE SMTH IN MY HAND");
                       
                        // Then you right click
                        if (Input.GetMouseButtonDown(0))
                        {
                            Debug.Log("Clicking with something in my hand!");
                            
                            // If you're aiming at the lemon slicer
                            if (interactable.GetComponent<LemonSlicer>() is { } lemonSlicer)
                            {
                                Debug.Log("Aiming at lemon slicer!");
                                
                                // If the item you're holding is a lemon
                                if (itemInHand as LemonSlice)
                                {
                                    SnapToCuttingBoard(itemInHand, lemonSlicer);
                                    
                                }
                            }
                        }
                        
                    }
                }
                else
                {
                    //Debug.Log("can't interact");
                }
                
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

    private void Update()
    {
        if (itemInHand)
        {
            if (Input.GetMouseButtonDown(1))
            {
                DropFromHand();
            }
        }
        
        SearchByRaycast();

        
    }
}
