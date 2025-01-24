using EzySlice;
using TMPro;
using UnityEngine;

namespace Code.Scripts
{
    public class LemonSlicer : MonoBehaviour
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
        private bool knifeOn = true;
        private float initialCircleScale;
        private Vector3 localCirclePos;

        private void Start()
        {
            initialCircleScale = circle.transform.localScale.x;
        }

        void Update()
        {
            MoveSlicer();
        }
        
        public GameObject[] Slice(Vector3 planeWorldPosition, Vector3 planeWorldDirection) {
            return objectToSlice.SliceInstantiate(planeWorldPosition, planeWorldDirection);
        }

        /// <summary>
        /// Moves the slicing guide circle back and forth (sine).
        /// </summary>
        void MoveSlicer()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                knifeOn = !knifeOn;
                objectToSlice = Slice(circle.transform.position, circle.transform.up)[0];
                objectToSlice.SetActive(false);
                //circle.SetActive(false);
            }

            if (knifeOn)
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
}
