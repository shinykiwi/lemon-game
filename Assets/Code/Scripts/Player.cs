using System.Collections;
using Code.Scripts;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Interactable lastInteractable;
    bool currentlySlicing = false;
    private Interactable itemInHand = null;

    [SerializeField] private Transform hand;

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
        
        // Make certain objects uninteractable
        LemonSlicer slicer = FindFirstObjectByType<LemonSlicer>();
        slicer.DisableInteract();
        
        itemInHand = interactable;
    }

    private void DropFromHand()
    {
        hand.DetachChildren();
        itemInHand.GetComponent<Rigidbody>().isKinematic = false;
        itemInHand.EnableInteract();
        itemInHand = null;
        
        StartCoroutine(nameof(EnableGeneralInteraction));
        
    }

    private IEnumerator EnableGeneralInteraction()
    {
        yield return null;
        
        // Make certain objects interactable again
        LemonSlicer slicer = FindFirstObjectByType<LemonSlicer>();
        slicer.EnableInteract();
    }

    private void SearchByRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
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
                    
                    // Assign it for later
                    lastInteractable = interactable;
                    
                    if (!itemInHand)
                    {
                        // Press E to interact
                        if (Input.GetMouseButtonDown(1))
                        {
                            // If it's a lemon slicer, then go into slice mode
                            if (interactable.GetComponent<LemonSlicer>() is { } lemonSlicer)
                            {
                                lemonSlicer.EnterSliceMode();
                            }

                            // If it's a lemon, pick it up if you have an empty hand
                            else if (interactable.GetComponent<LemonSlice>() is { } lemonSlice)
                            {
                                AddToHand(lemonSlice);
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
