using System;
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

        public void ImpactSound()
        {
            if (impactSound)
            {
                playerAudio.Play(impactSound);
            }
            
        }

        private void OnCollisionEnter(Collision other)
        {
            ImpactSound();
        }
    }
}
