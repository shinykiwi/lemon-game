using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class LemonSlice : Interactable
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
}
