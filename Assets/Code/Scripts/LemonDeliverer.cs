using System;
using UnityEngine;

public class LemonDeliverer : MonoBehaviour
{
    [SerializeField] private GameObject lemonBoxPrefab;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void DeliverLemons()
    {
        // Spawn a box of lemons here
        GameObject box = Instantiate(lemonBoxPrefab);
        box.transform.position = transform.position;
        
        // Play doorbell sound effect
        audioSource.Play();
    }
}
