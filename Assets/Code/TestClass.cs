using UnityEngine;

namespace Code
{
    public class TestClass : MonoBehaviour
    {

        private Vector3 pointA;
        private Vector3 pointB;

        private float z = 0.20f;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            pointA = new Vector3(-0.1919145f, -0.09545122f, 0.45f);
            pointA = new Vector3(-0.2777856f, 3.14321311f, 0.4157349f);
        }
        
        private Vector3 FindIntersection(Vector3 start, Vector3 end)
        {
            // Calculate the interpolation factor t
            float t = (z - start.z) / (end.z - start.z);
            
            // Interpolate to find the intersection point
            return new Vector3(
                Mathf.Lerp(start.x, end.x, t), // Interpolated X
                Mathf.Lerp(start.y, end.y, t), // Interpolated Y
                z                             // Fixed Z
            );
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log(FindIntersection(pointA, pointB).ToString("F8"));
            }
        
        }
    }
}
