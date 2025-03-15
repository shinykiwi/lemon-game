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

        public static void EnableAllEmptyHand()
        {
            EnableInteractablesByType<Holdable>();
            EnableInteractablesByType<Door>();
            EnableInteractablesByType<Sink>();
            KnifeSet knifeSet = FindFirstObjectByType<KnifeSet>();
            if (!knifeSet.HasBeenUsed())
            {
                knifeSet.EnableInteract();
            }
            
        }

        public static void ResetAll()
        {
            EnableAllEmptyHand();
            HideNonHoldables();
        }

        public static void HideNonHoldables()
        {
            HideInteractablesByType<TrashCan>();
            HideInteractablesByType<SugarJar>();
            HideInteractablesByType<LemonadePitcher>();
        }

        public static void HideAllInteractables()
        {
            HideInteractablesByType<Interactable>();
        }
    }
}
