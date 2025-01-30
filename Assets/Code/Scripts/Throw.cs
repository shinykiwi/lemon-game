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
        Rigidbody rigidbody = interactable.GetComponent<Rigidbody>();
        interactable.gameObject.transform.SetParent(null);
        rigidbody.AddForce(Vector3.forward * 1000, ForceMode.Impulse);
    }
}
