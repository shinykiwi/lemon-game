using System;
using System.Collections;
using UnityEngine;

public class LemonDeliverer : MonoBehaviour
{
    [SerializeField] private GameObject lemonBoxPrefab;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(DeliveryCoroutine(10));
    }

    private void DeliverLemons()
    {
        // Spawn a box of lemons here
        GameObject box = Instantiate(lemonBoxPrefab);
        box.transform.position = transform.position;
        
        // Play doorbell sound effect
        audioSource.Play();
    }
    
    IEnumerator DeliveryCoroutine(float x)
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(x);
        
        DeliverLemons();
    }
}
