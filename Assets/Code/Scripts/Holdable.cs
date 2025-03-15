using System;
using System.Collections;
using UnityEngine;
using static Code.Scripts.InteractHelper;

namespace Code.Scripts
{
    public class Holdable : Interactable
    {
        [SerializeField] private AudioClip impactSound;
        private PlayerAudio playerAudio;

        private bool canPlay = false;

        protected override void OnEnable()
        {
            base.OnEnable();
            playerAudio = FindFirstObjectByType<PlayerAudio>();
        }

        private void ImpactSound()
        {
            if (impactSound)
            {
                playerAudio.Play(impactSound);
            }
            
        }

        public virtual void PickUp()
        {
            // Play pick up sound
            playerAudio.PickUp();
            HideAllInteractables();
        }

        public virtual void Drop()
        {
            // Play drop sound
            canPlay = true;
            StartCoroutine(DisablePlay());
            
            EnableAllEmptyHand();
        }

        private IEnumerator DisablePlay()
        {
            yield return new WaitForSeconds(2);

            canPlay = false;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (canPlay)
            {
                ImpactSound();
            }
            
        }

        public override string ToString()
        {
            return "Pick Up";
        }
    }
}
