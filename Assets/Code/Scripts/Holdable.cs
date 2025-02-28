using System;
using System.Collections;
using UnityEngine;

namespace Code.Scripts
{
    //[RequireComponent(typeof(Rigidbody))]
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

        /// <summary>
        /// Lets impact sounds be played upon collision but only if triggered from a drop.
        /// </summary>
        public void PlayDropSound()
        {
            canPlay = true;
            StartCoroutine(DisablePlay());

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
    }
}
