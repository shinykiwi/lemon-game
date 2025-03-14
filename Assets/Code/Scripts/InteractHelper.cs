using UnityEngine;

namespace Code.Scripts
{
    public class InteractHelper : MonoBehaviour
    {
        public static void HideInteractablesByType<T>() where T : Interactable
        {
            T[] objects = FindObjectsByType<T>(FindObjectsSortMode.None);

            foreach (Interactable i in objects)
            {
                i.DisableInteract();
                Debug.Log("Disabling" + i.name);
            }
        }

        public static void EnableInteractablesByType<T>() where T : Interactable
        {
            T[] objects = FindObjectsByType<T>(FindObjectsSortMode.None);

            foreach (Interactable i in objects)
            {
                i.EnableInteract();
                Debug.Log("Enabling" + i.name);
            }
        }

        public static void EnableAllHoldables()
        {
            EnableInteractablesByType<Holdable>();
            EnableInteractablesByType<Door>();
            EnableInteractablesByType<Sink>();
        }

        public static void ResetAll()
        {
            EnableAllHoldables();
            HideNonHoldables();
        }

        public static void HideNonHoldables()
        {
            HideInteractablesByType<TrashCan>();
            HideInteractablesByType<SugarJar>();
            if (! (bool) FindFirstObjectByType<LemonSlicer>()?.HasLemon())
            {
                HideInteractablesByType<LemonSlicer>();
            }
            
            HideInteractablesByType<LemonadePitcher>();
        }

        public static void HideAllInteractables()
        {
            HideInteractablesByType<Interactable>();
        }
    }
}
