using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace Code.Scripts
{
    public class Triangle : MonoBehaviour
    {
        private List<Vector3> vertices; // To hold the vertices
        private List<int> indices; // To hold the indexes of the triangle
        private bool splitProduct = false;

        Triangle(Vector3 v0, Vector3 v1, Vector3 v2, bool splitProduct)
        {
            vertices = new List<Vector3> { v0, v1, v2 };
            this.splitProduct = splitProduct;
        }
    
    }
}
