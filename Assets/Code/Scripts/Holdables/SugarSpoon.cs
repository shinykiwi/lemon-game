using UnityEngine;
using static Code.Scripts.InteractHelper;

namespace Code.Scripts.Holdables
{
    public class SugarSpoon : Holdable
    {
        private bool hasSugar = false;
        [SerializeField] private GameObject sugar;
        private float defaultSugar = 5f;

        private void Start()
        {
            sugar.SetActive(false);
        }

        public void AddSugar()
        {
            hasSugar = true;
            sugar.SetActive(true);
            EnableInteractablesByType<LemonadePitcher>();
        }

        public float RemoveSugar()
        {
            hasSugar = false;
            sugar.SetActive(false);
            HideInteractablesByType<LemonadePitcher>();
            return defaultSugar;
        }
    
        public bool HasSugar()
        {
            return hasSugar;
        }

        public override void PickUp()
        {
            base.PickUp();
        
            EnableInteractablesByType<SugarJar>();
        
        }
    }
}
