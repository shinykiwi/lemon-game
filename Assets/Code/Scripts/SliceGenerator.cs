using System;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class SliceGenerator : MonoBehaviour
{
    #region GenerateMesh
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
    #endregion
    
    // --------------------

    private MeshSlicer meshSlicer;

    private void Start()
    {
        meshSlicer = GetComponent<MeshSlicer>();
        
        meshSlicer.SliceMesh();
    }
}
