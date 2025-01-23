using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Code.Scripts
{
    public class MeshSlicer : MonoBehaviour
    {
        public GameObject originalObject; // Object to slice
        public float zValue;
        public GameObject tinySphere;
    
        private Vector3 planePoint;
        private Vector3 planeNormal;

        Mesh originalMesh;
        Vector3[] vertices;
        int[] triangles;
    
        List<Vector3> positiveVertices;
        List<Vector3> negativeVertices;
    
        List<int> positiveTriangles;
        List<int> negativeTriangles;

        private int triangleCounter;

        public TextMeshProUGUI triText;
        public TextMeshProUGUI vertText;

        private void Start()
        {
            planePoint = new Vector3(0, 0, zValue);
            planeNormal = new Vector3(0, 0, 1);
        
            // Get the original mesh
            originalMesh = originalObject.GetComponent<MeshFilter>().mesh;
        
            // Get the original vertices
            vertices = originalMesh.vertices;
        
            // Get the original triangles
            triangles = originalMesh.triangles;

            // Create lists to hold the vertices of pos and neg sides
            positiveVertices = new List<Vector3>();
            negativeVertices = new List<Vector3>();
        
            // Create list to hold the triangles of pos and neg sides
            positiveTriangles = new List<int>();
            negativeTriangles = new List<int>();
        
        }

        public void SliceMesh()
        {
            // For each triangle
            for (int i = 0; i < triangles.Length; i += 3)
            {
                // Get the 3 vertices of the triangle
                Vector3 v0 = vertices[triangles[i]];
                Vector3 v1 = vertices[triangles[i + 1]];
                Vector3 v2 = vertices[triangles[i + 2]];

                // Classify vertices, if d > 0, then positive, else negative
                float d0 = Distance(v0);
                float d1 = Distance(v1);
                float d2 = Distance(v2);
                
                bool v0Positive = d0 > 0;
                bool v1Positive = d1 > 0;
                bool v2Positive = d2 > 0;

                // Count how many were on the positive side
                int positiveCount = (v0Positive ? 1 : 0) + (v1Positive ? 1 : 0) + (v2Positive ? 1 : 0);

                // All vertices are on the positive side
                if (positiveCount == 3)
                {
                    AddTriangle(v0, v1, v2, true);
                }
        
                // All vertices are on the negative side
                else if (positiveCount == 0)
                {
                    AddTriangle(v0, v1, v2, false);
                }
        
                // The triangle is intersected by the slicing plane
                // In this case, we will need to split the triangle onto pos and neg side
                else
                {
                    SplitTriangle2(v0, v1, v2, v0Positive, v1Positive, v2Positive);
                }
            }

            // Create new meshes
            CreateMesh(positiveVertices, positiveTriangles, "Positive Side");
            CreateMesh(negativeVertices, negativeTriangles, "Negative Side");
        
            // Hide original mesh
            originalObject.GetComponent<MeshRenderer>().enabled = false;
            
            // Display total stats
            vertText.text = "PositiveVertices: " + positiveVertices.Count + ", NegativeVertices: " +
                           negativeVertices.Count;

            triText.text = "PositiveTriangles: " + positiveTriangles.Count / 3 + ", NegativeTriangles: " +
                           negativeTriangles.Count / 3;


        }
    
        private float Distance(Vector3 vertex)
        {
            return Vector3.Dot(planeNormal, vertex - planePoint);
        }
    
        private void SplitTriangle2(Vector3 A, Vector3 B, Vector3 C, bool vAPositive, bool vBPositive, bool vCPositive)
        {
            Vector3 E, F;

            float tolerance = 0.001f;
            
            // Edge case: what if a vector lies on the z-value exactly
            if (Math.Abs(A.z - zValue) < tolerance)
            {
                Debug.Log("We got an edge case!");

                E = FindIntersection(C, B);
                
                AddTriangle(A,B,E,true,true);
                AddTriangle(A,E,C,false,true);
            }
        
            // If A and C are negative, and B is positive
            else if (!vAPositive && !vCPositive && vBPositive)
            {
                E = FindIntersection(A, B);
                F = FindIntersection(C, B);
            
                AddTriangle(E, B, F, true, true);
                AddTriangle(A,E,F,false,true);
                AddTriangle(A,F,C,false, true);
            }
        
            // If A and B are positive, and C is negative
            else if (vAPositive && vBPositive && !vCPositive)
            {
                E = FindIntersection(A, C);
                F = FindIntersection(C, B);
            
                AddTriangle(A, B, E, true, true);
                AddTriangle(E, B, F, true, true);
                AddTriangle(E,F,C, false, true);
            }
        
            // If B and C are positive and A is negative
            else if (!vAPositive && vBPositive && vCPositive)
            {
                E = FindIntersection(A, B);
                F = FindIntersection(A, C);
            
                AddTriangle(E,B,F,true, true);
                AddTriangle(F,B,C,true, true);
                AddTriangle(A,E,F,false, true);
            }

            else
            {
                Debug.Log("Not sure what to put here...");
                
            }
            
        
        }
    
        private Vector3 FindIntersection(Vector3 start, Vector3 end)
        {
            float z = zValue;
            // Calculate the interpolation factor t
            float t = (z - start.z) / (end.z - start.z);

            // Interpolate to find the intersection point
            return new Vector3(
                Mathf.Lerp(start.x, end.x, t), // Interpolated X
                Mathf.Lerp(start.y, end.y, t), // Interpolated Y
                z                             // Fixed Z
            );
        }

        private Vector3 RoundVector(Vector3 v, int decimals = 3)
        {
            return new Vector3((float) Math.Round(v.x, decimals), (float) Math.Round(v.y, decimals), (float) Math.Round(v.z, decimals));
        }
        
        private bool IsValidTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            return Vector3.Cross(v2 - v1, v3 - v1).sqrMagnitude > 0.0001f;
        }


        private void AddTriangle(Vector3 v0, Vector3 v1, Vector3 v2, 
            bool positive, bool split = false)
        {
            List<int> tris = positive ? positiveTriangles : negativeTriangles;
            List<Vector3> verts = positive ? positiveVertices : negativeVertices;
        
            triangleCounter++;
            
            // Checking for possible degenerate triangles
            if (v0 == v1 || v1 == v2 || v0 == v2)
            {
                Debug.Log("We got a degenerate!");
            }

            if (!IsValidTriangle(v0, v1, v2))
            {
                Debug.Log("Not a valid triangle!");
            }

            int a = AddVertex(v0, verts, split);
            int b = AddVertex(v1, verts, split);
            int c = AddVertex(v2, verts, split);
            
        
            tris.Add(a);
            tris.Add(b);
            tris.Add(c);

            string triangleName = "Triangle: (" + a + ", " + b + ", " + c + ")";

            if (split)
            {
                // Debug.Log("Adding split triangle # "+ triangleCounter + 
                //           "\n"+ triangleName + 
                //           "\nVertices: [" + 
                //           "("+v0.x+ ", " + v0.y+ ", " + v0.z + ")"+ 
                //           "("+v1.x+ ", " + v1.y+ ", " + v1.z + ")"+ 
                //           "("+v2.x+ ", " + v2.y+ ", " + v2.z + ")"+"]");
            
            }
        }

        private void AddRealVertex(Vector3 vertex, string vName)
        {
            GameObject v = Instantiate(tinySphere, originalObject.transform);
            v.transform.localPosition = vertex;
            v.name = vName;
        }

        private int AddVertex(Vector3 vertex, List<Vector3> verts, bool split)
        {
            float threshold = split ? 0.001f : 0.0001f;
            // Search through existing vertices
            for (int i = 0; i < verts.Count; i++)
            {
                // Check if the vertex is "close enough" to an existing vertex
                if (Vector3.Distance(vertex, verts[i]) < threshold)
                {
                    return i; // Return the index of the close vertex
                }
            }

            // If no close vertex is found, add the new vertex
            int index = verts.Count;
            verts.Add(vertex);
            if (split)
            {
                AddRealVertex(vertex, "Index " + index);
            }
            
            return index;
        }

        private void CreateMesh(List<Vector3> verts, List<int> tris, string mName)
        {
            Mesh mesh = new Mesh
            {
                vertices = verts.ToArray(),
                triangles = tris.ToArray()
            };
        
            mesh.RecalculateNormals();

            GameObject meshObject = new GameObject(mName);
            meshObject.AddComponent<MeshFilter>().mesh = mesh;
            meshObject.AddComponent<MeshRenderer>().material = originalObject.GetComponent<MeshRenderer>().material;
            meshObject.transform.position = originalObject.transform.position;
            meshObject.AddComponent<TestClass>();
        }
    }
}
