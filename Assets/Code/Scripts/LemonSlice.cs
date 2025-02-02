using System;
using UnityEngine;

public class LemonSlice : Interactable
{
    private MeshCollider meshCollider;
    private Rigidbody rigidbody;
    private GameObject gameObject;

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
    }
}
