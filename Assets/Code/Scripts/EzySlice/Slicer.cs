using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EzySlice {
    
    public sealed class Slicer {
        
        /// <summary>
        /// Internal class for storing submeshes.
        /// </summary>
        internal class SlicedSubmesh {
            public readonly List<Triangle> upperMesh = new List<Triangle>();
            public readonly List<Triangle> lowerMesh = new List<Triangle>();
            
            /// <summary>
            /// Checks if submesh has any UVs.
            /// </summary>
            public bool hasUV {
                get {
                    return upperMesh.Count > 0 ? upperMesh[0].hasUV : lowerMesh.Count > 0 && lowerMesh[0].hasUV;
                }
            }

            /// <summary>
            /// Checks if submesh has any normals.
            /// </summary>
            public bool hasNormal {
                get {
                    return upperMesh.Count > 0 ? upperMesh[0].hasNormal : lowerMesh.Count > 0 && lowerMesh[0].hasNormal;
                }
            }

            /// <summary>
            /// Checks if submesh has any tangents.
            /// </summary>
            public bool hasTangent {
                get {
                    return upperMesh.Count > 0 ? upperMesh[0].hasTangent : lowerMesh.Count > 0 && lowerMesh[0].hasTangent;
                }
            }

            /// <summary>
            /// Checks if slicing has occured. Will be true if both upper and lower meshes have triangles.
            /// </summary>
            public bool isValid {
                get {
                    return upperMesh.Count > 0 && lowerMesh.Count > 0;
                }
            }
        }

        /**
         * Helper function to accept a gameobject which will transform the plane
         * approprietly before the slice occurs
         * See -> Slice(Mesh, Plane) for more info
         */
        public static SlicedMesh Slice(GameObject obj, Plane pl, TextureRegion sliceRegion, Material sliceMaterial) {
            
            // Need a mesh filter to continue
            // Checks for mesh filter.
            if (!obj.TryGetComponent<MeshFilter>(out var filter)) {
                Debug.LogWarning("Error slicing: No MeshRenderer component found.");
                return null;
            }
            
            // Need a mesh renderer to continue.
            // Checks for mesh renderer.
            if (!obj.TryGetComponent<MeshRenderer>(out var renderer)) {
                Debug.LogWarning("Error slicing: No MeshRenderer component found.");
                return null;
            }
            
            // ... At this point, a mesh renderer and mesh filter have been found.

            // Get the mesh associated with the MeshFilter
            Mesh mesh = filter.sharedMesh;

            // Checks that the mesh does indeed exist
            if (mesh == null) {
                Debug.LogWarning("Error slicing: Provided GameObject must have a Mesh that is not NULL.");
                return null;
            }

            // Find index of the material for the slice section.
            int sliceIndex = 0;
            
            return Slice(mesh, pl, sliceRegion, sliceIndex);
        }

        /**
         * Slice the gameobject mesh (if any) using the Plane, which will generate
         * a maximum of 2 other Meshes.
         * This function will recalculate new UV coordinates to ensure textures are applied
         * properly.
         * Returns null if no intersection has been found or the GameObject does not contain
         * a valid mesh to cut.
         */
        public static SlicedMesh Slice(Mesh meshToCut, Plane pl, TextureRegion region, int crossIndex) {
            
            // If there's no mesh to cut then return null
            if (meshToCut == null) {
                return null;
            }

            // Getting the values of the original mesh
            Vector3[] verts = meshToCut.vertices;
            Vector2[] uv = meshToCut.uv;
            Vector3[] norm = meshToCut.normals;
            Vector4[] tan = meshToCut.tangents;

            // each submesh will be sliced and placed in its own array structure
            SlicedSubmesh mesh = new SlicedSubmesh();
            // the cross section hull is common across all submeshes
            List<Vector3> crossHull = new List<Vector3>();

            // we reuse this object for all intersection tests
            IntersectionResult result = new IntersectionResult();

            // see if we would like to split the mesh using uv, normals and tangents
            bool genUV = verts.Length == uv.Length;
            bool genNorm = verts.Length == norm.Length;
            bool genTan = verts.Length == tan.Length;
           
            // For the whole mesh
            int[] indices = meshToCut.GetTriangles(0);
            int indicesCount = indices.Length;

            

            // loop through all the mesh vertices, generating upper and lower hulls
            // and all intersection points
            for (int index = 0; index < indicesCount; index += 3) {
                int i0 = indices[index + 0];
                int i1 = indices[index + 1];
                int i2 = indices[index + 2];

                Triangle newTri = new Triangle(verts[i0], verts[i1], verts[i2]);

                // generate UV if available
                if (genUV) {
                    newTri.SetUV(uv[i0], uv[i1], uv[i2]);
                }

                // generate normals if available
                if (genNorm) {
                    newTri.SetNormal(norm[i0], norm[i1], norm[i2]);
                }

                // generate tangents if available
                if (genTan) {
                    newTri.SetTangent(tan[i0], tan[i1], tan[i2]);
                }

                // slice this particular triangle with the provided
                // plane
                if (newTri.Split(pl, result)) {
                    int upperHullCount = result.UpperMeshCount;
                    int lowerHullCount = result.LowerMeshCount;
                    int interHullCount = result.intersectionPointCount;

                    for (int i = 0; i < upperHullCount; i++) {
                        mesh.upperMesh.Add(result.UpperMesh[i]);
                    }

                    for (int i = 0; i < lowerHullCount; i++) {
                        mesh.lowerMesh.Add(result.LowerMesh[i]);
                    }

                    for (int i = 0; i < interHullCount; i++) {
                        crossHull.Add(result.intersectionPoints[i]);
                    }
                } else {
                    SideOfPlane sa = pl.SideOf(verts[i0]);
                    SideOfPlane sb = pl.SideOf(verts[i1]);
                    SideOfPlane sc = pl.SideOf(verts[i2]);

                    SideOfPlane side = SideOfPlane.ON;
                    if (sa != SideOfPlane.ON)
                    {
                        side = sa;
                    }
                    
                    if (sb != SideOfPlane.ON)
                    {
                        Debug.Assert(side == SideOfPlane.ON || side == sb);
                        side = sb;
                    }
                    
                    if (sc != SideOfPlane.ON)
                    {
                        Debug.Assert(side == SideOfPlane.ON || side == sc);
                        side = sc;
                    }

                    if (side == SideOfPlane.UP || side == SideOfPlane.ON) {
                        mesh.upperMesh.Add(newTri);
                    } else {
                        mesh.lowerMesh.Add(newTri);
                    }
                }
            }
            
            // Generation step
            if (mesh != null && mesh.isValid) {
                return CreateFrom(mesh, CreateFrom(crossHull, pl.normal, region), crossIndex);
            }

            // No slicing occured, just return null to signify
            return null;
        }

        /**
         * Generates a single SlicedMesh from a set of cut submeshes 
         */
        private static SlicedMesh CreateFrom(SlicedSubmesh mesh, List<Triangle> cross, int crossSectionIndex) {

            Mesh upperHull = CreateUpperMesh(mesh, mesh.upperMesh.Count, cross, crossSectionIndex);
            Mesh lowerHull = CreateLowerMesh(mesh, mesh.lowerMesh.Count, cross, crossSectionIndex);

            return new SlicedMesh(upperHull, lowerHull);
        }

        private static Mesh CreateUpperMesh(SlicedSubmesh mesh, int total, List<Triangle> crossSection, int crossSectionIndex) {
            return CreateMesh(mesh, total, crossSection, crossSectionIndex, true);
        }

        private static Mesh CreateLowerMesh(SlicedSubmesh mesh, int total, List<Triangle> crossSection, int crossSectionIndex) {
            return CreateMesh(mesh, total, crossSection, crossSectionIndex, false);
        }

        /**
         * Generate a single Mesh HULL of either the UPPER or LOWER hulls. 
         */
        private static Mesh CreateMesh(SlicedSubmesh mesh, int total, List<Triangle> crossSection, int crossIndex, bool isUpper) {
            if (total <= 0) {
                return null;
            }
            
            int crossCount = crossSection != null ? crossSection.Count : 0;

            Mesh newMesh = new Mesh();
            newMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            
            int arrayLen = (total + crossCount) * 3;

            bool hasUV = mesh.hasUV;
            bool hasNormal = mesh.hasNormal;
            bool hasTangent = mesh.hasTangent;

            // vertices and uv's are common for all submeshes
            Vector3[] newVertices = new Vector3[arrayLen];
            Vector2[] newUvs = hasUV ? new Vector2[arrayLen] : null;
            Vector3[] newNormals = hasNormal ? new Vector3[arrayLen] : null;
            Vector4[] newTangents = hasTangent ? new Vector4[arrayLen] : null;

            // each index refers to our submesh triangles
            List<int[]> triangles = new List<int[]>(1);

            int vIndex = 0;

            // first we generate all our vertices, uv's and triangles
         
            // pick the hull we will be playing around with
            List<Triangle> hull = isUpper ? mesh.upperMesh : mesh.lowerMesh;
            int hullCount = hull.Count;

            int[] indices = new int[hullCount * 3];

            // fill our mesh arrays
            for (int i = 0, triIndex = 0; i < hullCount; i++, triIndex += 3) {
                Triangle newTri = hull[i];

                int i0 = vIndex + 0;
                int i1 = vIndex + 1;
                int i2 = vIndex + 2;

                // add the vertices
                newVertices[i0] = newTri.positionA;
                newVertices[i1] = newTri.positionB;
                newVertices[i2] = newTri.positionC;

                // add the UV coordinates if any
                if (hasUV) {
                    newUvs[i0] = newTri.uvA;
                    newUvs[i1] = newTri.uvB;
                    newUvs[i2] = newTri.uvC;
                }

                // add the Normals if any
                if (hasNormal) {
                    newNormals[i0] = newTri.normalA;
                    newNormals[i1] = newTri.normalB;
                    newNormals[i2] = newTri.normalC;
                }

                // add the Tangents if any
                if (hasTangent) {
                    newTangents[i0] = newTri.tangentA;
                    newTangents[i1] = newTri.tangentB;
                    newTangents[i2] = newTri.tangentC;
                }

                // triangles are returned in clocwise order from the
                // intersector, no need to sort these
                indices[triIndex] = i0;
                indices[triIndex + 1] = i1;
                indices[triIndex + 2] = i2;

                vIndex += 3;
            }

            // add triangles to the index for later generation
            triangles.Add(indices);

            // generate the cross section required for this particular hull
            if (crossSection != null && crossCount > 0) {
                int[] crossIndices = new int[crossCount * 3];

                for (int i = 0, triIndex = 0; i < crossCount; i++, triIndex += 3) {
                    Triangle newTri = crossSection[i];

                    int i0 = vIndex + 0;
                    int i1 = vIndex + 1;
                    int i2 = vIndex + 2;

                    // add the vertices
                    newVertices[i0] = newTri.positionA;
                    newVertices[i1] = newTri.positionB;
                    newVertices[i2] = newTri.positionC;

                    // add the UV coordinates if any
                    if (hasUV) {
                        newUvs[i0] = newTri.uvA;
                        newUvs[i1] = newTri.uvB;
                        newUvs[i2] = newTri.uvC;
                    }

                    // add the Normals if any
                    if (hasNormal) {
                        // invert the normals dependiong on upper or lower hull
                        if (isUpper) {
                            newNormals[i0] = -newTri.normalA;
                            newNormals[i1] = -newTri.normalB;
                            newNormals[i2] = -newTri.normalC;
                        } else {
                            newNormals[i0] = newTri.normalA;
                            newNormals[i1] = newTri.normalB;
                            newNormals[i2] = newTri.normalC;
                        }
                    }

                    // add the Tangents if any
                    if (hasTangent) {
                        newTangents[i0] = newTri.tangentA;
                        newTangents[i1] = newTri.tangentB;
                        newTangents[i2] = newTri.tangentC;
                    }

                    // add triangles in clockwise for upper
                    // and reversed for lower hulls, to ensure the mesh
                    // is facing the right direction
                    if (isUpper) {
                        crossIndices[triIndex] = i0;
                        crossIndices[triIndex + 1] = i1;
                        crossIndices[triIndex + 2] = i2;
                    } else {
                        crossIndices[triIndex] = i0;
                        crossIndices[triIndex + 1] = i2;
                        crossIndices[triIndex + 2] = i1;
                    }

                    vIndex += 3;
                }

                // add triangles to the index for later generation
                if (triangles.Count <= crossIndex) {
                    triangles.Add(crossIndices);
                } else {
                    // otherwise, we need to merge the triangles for the provided subsection
                    int[] prevTriangles = triangles[crossIndex];
                    int[] merged = new int[prevTriangles.Length + crossIndices.Length];

                    System.Array.Copy(prevTriangles, merged, prevTriangles.Length);
                    System.Array.Copy(crossIndices, 0, merged, prevTriangles.Length, crossIndices.Length);

                    // replace the previous array with the new merged array
                    triangles[crossIndex] = merged;
                }
            }

            int totalTriangles = triangles.Count;

            newMesh.subMeshCount = totalTriangles;
            // fill the mesh structure
            newMesh.vertices = newVertices;

            if (hasUV) {
                newMesh.uv = newUvs;
            }

            if (hasNormal) {
                newMesh.normals = newNormals;
            }

            if (hasTangent) {
                newMesh.tangents = newTangents;
            }

            // add the submeshes
            for (int i = 0; i < totalTriangles; i++) {
                newMesh.SetTriangles(triangles[i], i, false);
            }

            return newMesh;
        }

        /**
         * Generate Two Meshes (an upper and lower) cross section from a set of intersection
         * points and a plane normal. Intersection Points do not have to be in order.
         */
        private static List<Triangle> CreateFrom(List<Vector3> intPoints, Vector3 planeNormal, TextureRegion region) {
            return Triangulator.MonotoneChain(intPoints, planeNormal, out List<Triangle> tris, region) ? tris : null;
        }
    }
}
