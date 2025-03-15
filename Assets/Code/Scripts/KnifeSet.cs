using UnityEngine;

namespace Code.Scripts
{
    public class KnifeSet : Interactable
    {
        [SerializeField] private GameObject knife;

        private bool used = false;
        public Interactable GetKnife()
        {
            knife = Instantiate(knife);
            used = true;
            return knife.GetComponent<Interactable>();
        }

        public bool HasBeenUsed()
        {
            return used;
        }
    }
}
