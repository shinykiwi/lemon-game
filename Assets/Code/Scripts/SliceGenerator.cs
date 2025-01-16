using System.Numerics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class SliceGenerator : MonoBehaviour
{
    // Define world size
    public int width = 50;
    public int height = 50;
    
    // Create mesh
    private Mesh mesh;
    private MeshFilter meshFilter;
    
    // Define tris and verts
    private int[] triangles;
    private Vector3[] vertices;
    private Vector2[] uv;

    public GameObject sphere;
    public GameObject tinySphere;

    void Start()
    {
        SplitSphere();
       
    }
    
    private void GenerateMesh()
    {
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

        meshFilter = gameObject.AddComponent<MeshFilter>();
        
        mesh = new Mesh();
        
        // Setting up the vertices
        vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0), // bottom left (0)
            new Vector3(width, 0, 0), // bottom right (1)
            new Vector3(0, height, 0), // top left (2)
            new Vector3(width, height, 0) // top right (3)
        };

        mesh.vertices = vertices;
        
        // Setting up the triangles
        triangles = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = triangles;
        
        // Calculating the normals
        mesh.RecalculateNormals();
        
        // UVs
        uv = new Vector2[4]{
            new Vector2(0, 0), // bottom left
            new Vector2(1, 0), // bottom right
            new Vector2(0, 1), // top left
            new Vector2(1, 1) // top right
        };

        mesh.uv = uv;

        meshFilter.mesh = mesh;
    }

    private void SplitSphere()
    {
        Mesh spMesh = sphere.GetComponent<MeshFilter>().mesh;
        triangles = spMesh.triangles;
        vertices = spMesh.vertices;
        Debug.Log(spMesh);

        // Initial vertex calculation
        Vector3[] newVerts = new Vector3[vertices.Length];
        int vertexCount = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i].z >= 0.2)
            {
                newVerts[i] = vertices[i];
                vertexCount++;
                
            }
        }

        // Copying values from other array to fit to a smaller array
        Vector3[] sliceVerts = new Vector3[vertexCount];
        int index = 0;
        foreach (Vector3 vert in newVerts)
        {
            if (!vert.Equals(Vector3.zero))
            {
                sliceVerts[index] = vert;
                GameObject vertSphere = Instantiate(tinySphere, sphere.transform);
                vertSphere.transform.localPosition = sliceVerts[index];
                index++;
                
            }
        }

        foreach (Vector3 v in sliceVerts)
        {
            Debug.Log(v.ToString());
        }


        // int triCount = vertexCount / vertices.Length;
        // // Creating triangles
        // int[] sliceTris = new int[triCount];
        // for (int j = 0; j < triCount; j++)
        // {
        //     sliceTris[j] = triangles[j];
        // }
        //
        // Debug.Log("TriCount: " +triCount);
        // Debug.Log("There are " + sliceVerts.Length + " vertices.");
        // Debug.Log("There are " + sliceTris.Length + " triangles");
        //
        // spMesh.vertices = sliceVerts;
        // spMesh.triangles = sliceTris;
        // spMesh.RecalculateNormals();
        
        
    }
}
