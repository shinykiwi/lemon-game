using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using static Code.Scripts.InteractHelper;

namespace Code.Scripts.Holdables
{
    public class LemonSlice : Holdable
    {
        private MeshCollider meshCollider;
        private Rigidbody rigidbody;
        private GameObject gameObject;
        private bool hasBeenSliced = false;
        private float squeeze = 0.35f;

        private float juice = 10f;

        public void Setup(Vector3 position)
        {
            gameObject = this.transform.gameObject;
            meshCollider = gameObject.AddComponent<MeshCollider>();
            rigidbody = gameObject.AddComponent<Rigidbody>();

            //rigidbody.linearDamping = 10f;
            //rigidbody.angularDamping = 2f;
            meshCollider.convex = true;
        
            transform.localPosition = position;
        
            name = "LemonSlice";
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.materials = new Material[] {meshRenderer.materials[0]};

            hasBeenSliced = true;
        }

        public bool IsSliced()
        {
            return hasBeenSliced;
        }

        public bool CanBeSqueezed()
        {
            return juice > 0;
        }
    
        public bool Squeeze()
        {
            if (juice > 0)
            {
                transform.DOShakeScale(.5f, squeeze);
                juice -= Random.value * 2;
                return true;
            }
            else
            {
                transform.DOShakeRotation(.5f, squeeze);
                return false;
            }
        }

        public override void PickUp()
        {
            base.PickUp();

            // If it's currently on a lemon slicer
            
            //Debug.Log(gameObject.transform);
            // if (gameObject.transform.root.gameObject.GetComponent<LemonSlicer>() is { } lemonSlicer)
            // {
            //     lemonSlicer.SetObjectToSlice(null);
            // }

            // Not been sliced, just a regular lemon
            if (!hasBeenSliced)
            {
                EnableInteractablesByType<LemonSlicer>();
            }
        
            // A slice of a lemon
            else
            {
                EnableInteractablesByType<LemonadePitcher>();
            }
        
            EnableInteractablesByType<TrashCan>();
        }
    }
}
