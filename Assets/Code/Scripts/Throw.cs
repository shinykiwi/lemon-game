using System;
using UnityEngine;

public class Throw : MonoBehaviour
{
    ReticleController reticleController;

    private void Start()
    {
        reticleController = FindFirstObjectByType<ReticleController>();
    }

    public void ThrowObject(Interactable interactable)
    {
        Debug.Log("Throwing " + interactable.name);
        interactable.gameObject.transform.SetParent(null);
        
        Rigidbody rigidbody = interactable.GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        
        rigidbody.AddForce(interactable.gameObject.transform.right * 100, ForceMode.Impulse);
        
        interactable.EnableInteract();
    }
}
