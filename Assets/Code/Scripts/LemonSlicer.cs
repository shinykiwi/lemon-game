using System.Collections;
using Code.Scripts.Holdables;
using EzySlice;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Code.Scripts
{
    public class LemonSlicer : Interactable
    {
    
        [Header("Parameters")]
        [SerializeField] GameObject circle;
        [SerializeField] private float limit = 0.5f;
        [SerializeField] private float speed = 1;
        [SerializeField] private float scaleFactor = 2f;

        [Header("Slicing")]
        [SerializeField] private GameObject objectToSlice;
        [SerializeField] private Knife knife;
        [SerializeField] private Transform objectSpawn;
    
        // For controlling the movement of the slice
        private float x = 0;
        private float y = 0;
        private float scale = 0;
        private bool knifeOn = false;
        private float initialCircleScale;
        private Vector3 localCirclePos;

        private CinemachineCamera camera;
        private PlayerAudio playerAudio;

        private void Start()
        {
            // Set the initial scale
            initialCircleScale = circle.transform.localScale.x;
            
            // Find the camera
            camera = GetComponentInChildren<CinemachineCamera>();
            camera.enabled = false;
            
            // Find player audio object
            playerAudio = FindFirstObjectByType<PlayerAudio>();
            
            HideSlicer();
        }

        void Update()
        {
            // If the knife is on, allow slicing operations
            if (knifeOn)
            {
                ShowSlicer();
                MoveSlicer();
            }
        }

        public bool HasLemon()
        {
            return objectToSlice;
        }

        public void InitiateSlice()
        {
            KnifeCutSequence();
            Slice();
            playerAudio.SliceLemon();
        }
        
        /// <summary>
        /// Gets the location where objects will be spawned onto the cutting board.
        /// </summary>
        /// <returns></returns>
        public Transform GetObjectSpawn()
        {
            return objectSpawn;
        }
        
        /// <summary>
        /// Performs the slicing mesh operation.
        /// </summary>
        /// <param name="planeWorldPosition"></param>
        /// <param name="planeWorldDirection"></param>
        /// <returns></returns>
        private GameObject[] SliceMesh(Vector3 planeWorldPosition, Vector3 planeWorldDirection) {
            return objectToSlice.SliceInstantiate(planeWorldPosition, planeWorldDirection);
        }
        

        /// <summary>
        /// Changes to the slicing view and enables slicing. Hides reticle.
        /// </summary>
        public void EnterSliceMode()
        {
            // Only enter slice mode if there is an object to slice there
            if (objectToSlice)
            {
                Debug.Log(objectToSlice.name);
                DisableInteract();
                camera.enabled = true;
                knifeOn = true;
                FindFirstObjectByType<ReticleController>().HideReticle();
            }
        }
        

        /// <summary>
        /// Exits the slicing view and returns to normal. Shows the reticle.
        /// </summary>
        private void ExitSliceMode()
        {
            EnableInteract();
            camera.enabled = false;
            knifeOn = false;
            FindFirstObjectByType<ReticleController>().ShowReticle();
        }


        // TODO: Finish this 
        private void KnifeCutSequence()
        {
            
        }
        
        
        /// <summary>
        /// Slices the lemon into two if it exists, then adds the LemonSlice component.
        /// </summary>
        private void Slice()
        {
            if (objectToSlice)
            {
                objectToSlice.GetComponent<Interactable>().RemoveOutline();
            
                GameObject[] slices = SliceMesh(circle.transform.position, circle.transform.up);
                GameObject upper = slices[0];
                GameObject lower = slices[1];
            
                upper.AddComponent<LemonSlice>().Setup(objectToSlice.transform.position);
                lower.AddComponent<LemonSlice>().Setup(objectToSlice.transform.position);
            
                HideSlicer();

                ExitSliceMode();

                StartCoroutine(DestroyOriginal(objectToSlice));
            }
        }
        

        /// <summary>
        /// Destroys the original object after waiting until the next frame.
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        private IEnumerator DestroyOriginal(GameObject original)
        {
            yield return null;
            
            Destroy(original);
            
        }
        

        /// <summary>
        /// Hides the slicer plane (white circle).
        /// </summary>
        private void HideSlicer()
        {
            circle.SetActive(false);
           
        }
        

        /// <summary>
        /// Shows the slicer plane (white circle).
        /// </summary>
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
        }
        

        /// <summary>
        /// Sets the active object that is on the cutting board.
        /// </summary>
        /// <param name="interactable"></param>
        public void SetObjectToSlice(Interactable interactable)
        {
            objectToSlice = interactable.gameObject;
        }

        public override string ToString()
        {
            return HasLemon() ? "Slice" : "Place";
        }
    }
}
