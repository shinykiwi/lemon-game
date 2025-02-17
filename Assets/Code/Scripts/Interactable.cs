using UnityEngine;

namespace Code.Scripts
{
    /// <summary>
    /// General class for an object that can have an outline and be interacted with.
    /// </summary>
    public class Interactable : MonoBehaviour
    {
    
        private Outline outline;
        [SerializeField] bool canInteract = true;

        /// <summary>
        /// Gets the outline or creates one if there isn't. Hides the outline by default.
        /// </summary>
        private void OnEnable()
        {
            outline = gameObject.GetComponent<Outline>();

            if (!outline)
            {
                outline = gameObject.GetComponentInChildren<Outline>();
            }
        
            if (!outline)
            {
                AddOutline();
            }
        
            HideOutline();
        }
    
        /// <summary>
        /// Toggles the outline on and off.
        /// </summary>
        private void ToggleOutline()
        {
            outline.enabled = !outline.enabled;
        }

        /// <summary>
        /// Shows the outline.
        /// </summary>
        public void ShowOutline()
        {
            outline.enabled = true;
        
        }

        /// <summary>
        /// Hides the outline.
        /// </summary>
        public void HideOutline()
        {
            if (outline) 
                outline.enabled = false;
       
        }

        /// <summary>
        /// Deletes the outline entirely from the object.
        /// </summary>
        public void RemoveOutline()
        {
            Destroy(outline);
        }

        /// <summary>
        /// Adds an outline if there isn't one already.
        /// </summary>
        private void AddOutline()
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

        /// <summary>
        /// Disables interaction for the object and hides the outline.
        /// </summary>
        public void DisableInteract()
        {
            canInteract = false;
            HideOutline();
        }

        /// <summary>
        /// Enables interaction for the object.
        /// </summary>
        public void EnableInteract()
        {
            canInteract = true;
        }

        /// <summary>
        /// Whether one can interact with the object or not.
        /// </summary>
        /// <returns></returns>
        public bool CanInteract()
        {
            return canInteract;
        }
    
    }
}
