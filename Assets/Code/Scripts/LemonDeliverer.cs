using System.Collections;
using UnityEngine;

namespace Code.Scripts
{
    public class LemonDeliverer : MonoBehaviour
    {
        [SerializeField] private GameObject lemonBoxPrefab;
        [SerializeField] private int deliveryDelayTime = 5;
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();

            StartCoroutine(DeliveryCoroutine(deliveryDelayTime));
        }

        private void DeliverLemons()
        {
            // Spawn a box of lemons here
            GameObject box = Instantiate(lemonBoxPrefab);
            box.transform.position = transform.position;
        
            // Play doorbell sound effect
            audioSource.Play();
        }
    
        IEnumerator DeliveryCoroutine(int x)
        {
            //yield on a new YieldInstruction that waits for 5 seconds.
            yield return new WaitForSeconds(x);
        
            DeliverLemons();
        }
    }
}
