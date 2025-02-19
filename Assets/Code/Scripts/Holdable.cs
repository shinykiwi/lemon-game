using UnityEngine;

namespace Code.Scripts
{
    //[RequireComponent(typeof(Rigidbody))]
    public class Holdable : Interactable
    {
        [SerializeField] private AudioClip dropSound;
        private PlayerAudio playerAudio;

        protected override void OnEnable()
        {
            base.OnEnable();
            playerAudio = FindFirstObjectByType<PlayerAudio>();
        }

        public void DropSound()
        {
            if (dropSound)
            {
                playerAudio.Play(dropSound);
            }
            
        }
    }
}
