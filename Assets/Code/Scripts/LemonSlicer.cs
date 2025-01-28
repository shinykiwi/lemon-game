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
        [SerializeField] GameObject objectToSlice;
    
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
            
            HideSlicer();
        }

        void Update()
        {
            if (knifeOn)
            {
                ShowSlicer();
                MoveSlicer();

                if (Input.anyKeyDown)
                { 
                    Slice();
                }
            }
        }
        
        public GameObject[] SliceMesh(Vector3 planeWorldPosition, Vector3 planeWorldDirection) {
            return objectToSlice.SliceInstantiate(planeWorldPosition, planeWorldDirection);
        }

        public void BeginSlicing()
        {
            knifeOn = true;
        }

        private void Slice()
        {
            objectToSlice.GetComponent<Interactable>().RemoveOutline();
            
            GameObject[] slices = SliceMesh(circle.transform.position, circle.transform.up);
            GameObject upper = slices[0];
            GameObject lower = slices[1];
            
            upper.AddComponent<LemonSlice>().Setup(objectToSlice.transform.position.y);
            lower.AddComponent<LemonSlice>().Setup(objectToSlice.transform.position.y);
                
            Destroy(objectToSlice);
            ResetSlicer();
            HideSlicer();

            knifeOn = false;

        }

        private void HideSlicer()
        {
            circle.SetActive(false);
           
        }

        private void ShowSlicer()
        {
            circle.SetActive(true);
        }

        private void ResetSlicer()
        {
            //circle.transform.SetParent(objectToSlice.transform);
            circle.transform.localPosition = Vector3.zero;
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
    }
}
