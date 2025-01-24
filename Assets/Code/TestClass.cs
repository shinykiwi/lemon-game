using System;
using EzySlice;
using UnityEngine;

namespace Code
{
    public class TestClass : MonoBehaviour
    {
        public GameObject objectToSlice; // non-null
        public GameObject slicer;

        private void Start()
        {
            Slice(slicer.transform.position, slicer.transform.up);
            objectToSlice.SetActive(false);
            slicer.SetActive(false);
        }

        /**
         * Example on how to slice a GameObject in world coordinates.
         */
        public GameObject[] Slice(Vector3 planeWorldPosition, Vector3 planeWorldDirection) {
            return objectToSlice.SliceInstantiate(planeWorldPosition, planeWorldDirection);
        }
    }
}
