using System;
using UnityEngine;

public class LemonSlice : Interactable
{
    private MeshCollider meshCollider;
    private Rigidbody rigidbody;
    private GameObject gameObject;

    public void Setup(float y)
    {
        gameObject = this.transform.gameObject;
        meshCollider = gameObject.AddComponent<MeshCollider>();
        rigidbody = gameObject.AddComponent<Rigidbody>();

        rigidbody.mass = 2;
        meshCollider.convex = true;
        
        transform.localPosition = new Vector3(0, y, 0);
        
        name = "LemonSlice";
        
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.materials = new Material[] {meshRenderer.materials[0]};
        
    }
}
