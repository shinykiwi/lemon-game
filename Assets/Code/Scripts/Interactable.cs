using System;
using UnityEngine;

/// <summary>
/// General class for an object that can have an outline and be interacted with.
/// </summary>
public class Interactable : MonoBehaviour
{
    
    private Outline outline;
    private MeshRenderer meshRenderer;
    [SerializeField] bool canInteract = true;

    /// <summary>
    /// Gets the outline or creates one if there isn't. Hides the outline by default.
    /// </summary>
    private void OnEnable()
    {
        outline = gameObject.GetComponent<Outline>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        
        HideOutline();
    }
    
    /// <summary>
    /// Toggles the outline on and off.
    /// </summary>
    public void ToggleOutline()
    {
        outline.enabled = !outline.enabled;
    }

    /// <summary>
    /// Shows the outline.
    /// </summary>
    public void ShowOutline()
    {
        //f (meshRenderer && outline)
            outline.enabled = true;
        
    }

    /// <summary>
    /// Hides the outline.
    /// </summary>
    public void HideOutline()
    {
       //if (meshRenderer && outline) 
           outline.enabled = false;
       
    }

    public void RemoveOutline()
    {
        Destroy(outline);
    }

    public void AddOutline()
    {
        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>(); 
        }
    }

    public void ToggleInteract()
    {
        canInteract = !canInteract;
        ToggleOutline();
    }

    public void DisableInteract()
    {
        canInteract = false;
        HideOutline();
    }

    public void EnableInteract()
    {
        canInteract = true;
    }

    public bool CanInteract()
    {
        return canInteract;
    }
}
