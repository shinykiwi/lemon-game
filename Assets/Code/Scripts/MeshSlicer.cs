using System;
using UnityEngine;
using System.Collections.Generic;

public class MeshSlicer : MonoBehaviour
{
    public GameObject originalObject; // Object to slice
    public float zValue;
    public GameObject tinySphere;
    
    private Vector3 planePoint;
    private Vector3 planeNormal;
    
    
    private int triangleCounter = 0;

    private void Start()
    {
        planePoint = new Vector3(0, 0, zValue);
        planeNormal = new Vector3(0, 0, 1);
    }

    public void SliceMesh()
    {
        // Get the original mesh
        Mesh originalMesh = originalObject.GetComponent<MeshFilter>().mesh;
        
        // Get the original vertices
        Vector3[] vertices = originalMesh.vertices;
        
        // Get the original triangles
        int[] triangles = originalMesh.triangles;

        // Create lists to hold the vertices of pos and neg sides
        List<Vector3> positiveVertices = new List<Vector3>();
        List<Vector3> negativeVertices = new List<Vector3>();
        
        // Create list to hold the triangles of pos and neg sides
        List<int> positiveTriangles = new List<int>();
        List<int> negativeTriangles = new List<int>();

        // Create a mapping to make sure there are no duplicates when adding tris and verts
        Dictionary<Vector3, int> positiveVertexMap = new Dictionary<Vector3, int>();
        Dictionary<Vector3, int> negativeVertexMap = new Dictionary<Vector3, int>();

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
                AddTriangle(v0, v1, v2, positiveVertices, positiveTriangles, positiveVertexMap);
            }
            
            // All vertices are on the negative side
            else if (positiveCount == 0)
            {
                AddTriangle(v0, v1, v2, negativeVertices, negativeTriangles, negativeVertexMap);
            }
            
            // The triangle is intersected by the slicing plane
            // In this case, we will need to split the triangle onto pos and neg side
            else
            {
                SplitTriangle(v0, v1, v2, v0Positive, v1Positive, v2Positive,
                    positiveVertices, negativeVertices, positiveTriangles, negativeTriangles,
                    positiveVertexMap, negativeVertexMap);
            }
        }

        // Create new meshes
        CreateMesh(positiveVertices, positiveTriangles, "Positive Side");
        CreateMesh(negativeVertices, negativeTriangles, "Negative Side");
        
        // Hide original mesh
        originalObject.SetActive(false);
    }
    
    private float Distance(Vector3 vertex)
    {
        return Vector3.Dot(planeNormal, vertex - planePoint);
    }
    private void SplitTriangle(Vector3 v0, Vector3 v1, Vector3 v2, bool v0Positive, bool v1Positive, bool v2Positive,
        List<Vector3> positiveVertices, List<Vector3> negativeVertices,
        List<int> positiveTriangles, List<int> negativeTriangles,
        Dictionary<Vector3, int> positiveVertexMap, Dictionary<Vector3, int> negativeVertexMap)
    {
        
        Vector3 positiveA, positiveB, negativeA, intersection1, intersection2;

        // If the 1st vertex and 2nd vertex are on one side together
        if (v0Positive && v1Positive)
        {
            positiveA = v0; positiveB = v1; negativeA = v2;
        }
        
        // If the 2nd vertex and the 3rd vertex are on one side together
        else if (v1Positive && v2Positive)
        {
            positiveA = v1; positiveB = v2; negativeA = v0;
        }

        // If the 1st and 3rd vertex are on one side together
        else
        {
            positiveA = v2; positiveB = v0; negativeA = v1;
        }

        // Find the intersection point that lies on the two intersection edges
        intersection1 = FindIntersection(positiveA, negativeA, zValue);
        intersection2 = FindIntersection(positiveB, negativeA, zValue);
        
        // Add new triangles to each side
        AddTriangle(positiveA, positiveB, intersection1, positiveVertices, positiveTriangles, positiveVertexMap, true);
        AddTriangle(positiveB, intersection2, intersection1, positiveVertices, positiveTriangles, positiveVertexMap, true);
        AddTriangle(negativeA, intersection1, intersection2, negativeVertices, negativeTriangles, negativeVertexMap, true);
    }
    
    private Vector3 FindIntersection(Vector3 start, Vector3 end, float z)
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

    private Vector3 RoundVector(Vector3 v, int decimals = 3)
    {
        return new Vector3((float) Math.Round(v.x, decimals), (float) Math.Round(v.y, decimals), (float) Math.Round(v.z, decimals));
    }

    private void AddTriangle(Vector3 v0, Vector3 v1, Vector3 v2, 
        List<Vector3> vertices, List<int> triangles, Dictionary<Vector3, int> vertexMap, bool split = false)
    {
        
        triangleCounter++;

        int a = AddVertex(v0, vertices, vertexMap);
        int b = AddVertex(v1, vertices, vertexMap);
        int c = AddVertex(v2, vertices, vertexMap);
        
        triangles.Add(a);
        triangles.Add(b);
        triangles.Add(c);

        string triangleName = "Triangle: (" + a + ", " + b + ", " + c + ")";

        if (split)
        {
            Debug.Log("Adding split triangle # "+ triangleCounter + 
                      "\n"+ triangleName + 
                      "\nVertices: [" + 
                      "("+v0.x+ ", " + v0.y+ ", " + v0.z + ")"+ 
                      "("+v1.x+ ", " + v1.y+ ", " + v1.z + ")"+ 
                      "("+v2.x+ ", " + v2.y+ ", " + v2.z + ")"+"]");
            
            AddRealVertex(v0, triangleName);
            AddRealVertex(v1, triangleName);
            AddRealVertex(v2, triangleName);
            
            
        }

    }

    private void AddRealVertex(Vector3 vertex, string vName)
    {
        GameObject v = Instantiate(tinySphere, originalObject.transform);
        v.transform.localPosition = vertex;
        v.name = vName;
    }

    private int AddVertex(Vector3 vertex, List<Vector3> vertices, Dictionary<Vector3, int> vertexMap)
    {
        float threshold = 0.001f;
        // Search through existing vertices
        for (int i = 0; i < vertices.Count; i++)
        {
            // Check if the vertex is "close enough" to an existing vertex
            if (Vector3.Distance(vertex, vertices[i]) < threshold)
            {
                return i; // Return the index of the close vertex
            }
        }

        // If no close vertex is found, add the new vertex
        int index = vertices.Count;
        vertices.Add(vertex);
        return index;
    }

    private void CreateMesh(List<Vector3> vertices, List<int> triangles, string name)
    {
        Mesh mesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray()
        };
        
        mesh.RecalculateNormals();

        GameObject meshObject = new GameObject(name);
        meshObject.AddComponent<MeshFilter>().mesh = mesh;
        meshObject.AddComponent<MeshRenderer>().material = originalObject.GetComponent<MeshRenderer>().material;
        meshObject.transform.position = originalObject.transform.position;
    }
}
