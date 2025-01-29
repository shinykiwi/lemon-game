using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using EzySlice;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Code.Scripts
{
    public class LemonSlicer : Interactable
    {
        [Header("Debug Only")] 
        [SerializeField] private bool debugOn = true;
        [SerializeField] private TextMeshProUGUI xText;
        [SerializeField] private TextMeshProUGUI zText;
        [SerializeField] private TextMeshProUGUI scaleText;
    
        [Header("Parameters")]
        [SerializeField] GameObject circle;
        [SerializeField] private float limit = 0.5f;
        [SerializeField] private float speed = 1;
        [SerializeField] private float scaleFactor = 2f;

        [Header("Slicing")]
        [SerializeField] private GameObject objectToSlice;

        [SerializeField] private Transform objectSpawn;
    
        // For controlling the movement of the slice
        private float x = 0;
        private float y = 0;
        private float scale = 0;
        private bool knifeOn = false;
        private float initialCircleScale;
        private Vector3 localCirclePos;

        private CinemachineCamera camera;

        private void Start()
        {
            // Set the initial scale
            initialCircleScale = circle.transform.localScale.x;
            
            // Find the camera
            camera = GetComponentInChildren<CinemachineCamera>();
            camera.enabled = false;
            
            Debug.Log(camera);
            
            HideSlicer();
        }

        void Update()
        {
            if (knifeOn)
            {
                ShowSlicer();
                MoveSlicer();

                // Left click to slice
                if (Input.GetMouseButtonDown(0))
                { 
                    Slice();
                }
            }
        }
        
        public GameObject[] SliceMesh(Vector3 planeWorldPosition, Vector3 planeWorldDirection) {
            return objectToSlice.SliceInstantiate(planeWorldPosition, planeWorldDirection);
        }

        public void EnterSliceMode()
        {
            Debug.Log(objectToSlice.name);
            // Only enter slice mode if there is an object to slice there
            if (objectToSlice)
            {
                DisableInteract();
                camera.enabled = true;
                knifeOn = true;
                FindFirstObjectByType<ReticleController>().HideReticle();
            }
        }

        private void ExitSliceMode()
        {
            EnableInteract();
            camera.enabled = false;
            knifeOn = false;
            FindFirstObjectByType<ReticleController>().ShowReticle();
        }

        public Transform GetObjectSpawn()
        {
            return objectSpawn;
        }

        private void Slice()
        {
            objectToSlice.GetComponent<Interactable>().RemoveOutline();
            
            GameObject[] slices = SliceMesh(circle.transform.position, circle.transform.up);
            GameObject upper = slices[0];
            GameObject lower = slices[1];
            
            upper.AddComponent<LemonSlice>().Setup(objectToSlice.transform.position.y);
            lower.AddComponent<LemonSlice>().Setup(objectToSlice.transform.position.y);
            
            HideSlicer();

            ExitSliceMode();

            StartCoroutine(DestroyOriginal(objectToSlice));

        }

        private IEnumerator DestroyOriginal(GameObject original)
        {
            yield return null;
            
            Destroy(original);
            
        }

        private void HideSlicer()
        {
            circle.SetActive(false);
           
        }

        private void ShowSlicer()
        {
            circle.SetActive(true);
        }

        /// <summary>
        /// Moves the slicing guide circle back and forth (sine).
        /// </summary>
        private void MoveSlicer()
        {
            // Moves the circle back and forth
            localCirclePos = circle.transform.localPosition; // current position
            x += Time.deltaTime;
            y = limit * Mathf.Sin(speed*x);
            circle.transform.localPosition = new Vector3(localCirclePos.x, localCirclePos.x, y);
        
            // Decreases the size of the circle as it moves further away from the center
            scale = (-scaleFactor * Mathf.Pow(y, 2)) + initialCircleScale;
            circle.transform.localScale = new Vector3(scale, scale, scale);

            if (debugOn)
            {
                xText.text = x.ToString("0.000");
                zText.text = y.ToString("0.000");
                scaleText.text = scale.ToString("0.000");
            }
        }

        public void SetObjectToCut(Interactable interactable)
        {
            objectToSlice = interactable.gameObject;
        }
    }
}
