using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EzySlice {

    /**
     * The final generated data structure from a slice operation. This provides easy access
     * to utility functions and the final Mesh data for each section of the HULL.
     */
    public sealed class SlicedMesh {
        private Mesh upperMesh;
        private Mesh lowerMesh;

        public SlicedMesh(Mesh upperMesh, Mesh lowerMesh) {
            this.upperMesh = upperMesh;
            this.lowerMesh = lowerMesh;
        }

        public GameObject CreateUpperMesh(GameObject originalObject, Material slicedSectionMat) {
            GameObject newObject = CreateUpperMesh();

            if (newObject != null) {
                newObject.transform.localPosition = originalObject.transform.localPosition;
                newObject.transform.localRotation = originalObject.transform.localRotation;
                newObject.transform.localScale = originalObject.transform.localScale;

                Material[] sharedMaterials = originalObject.GetComponent<MeshRenderer>().sharedMaterials;
                Mesh mesh = originalObject.GetComponent<MeshFilter>().sharedMesh;

                // nothing changed in the hierarchy, the cross section must have been batched
                // with the submeshes, return as is, no need for any changes
                if (mesh.subMeshCount == upperMesh.subMeshCount) {
                    // the the material information
                    newObject.GetComponent<Renderer>().sharedMaterials = sharedMaterials;

                    return newObject;
                }

                // otherwise the cross section was added to the back of the submesh array because
                // it uses a different material. We need to take this into account
                Material[] newSharedMat = new Material[sharedMaterials.Length + 1];

                // copy our material arrays across using native copy (should be faster than loop)
                System.Array.Copy(sharedMaterials, newSharedMat, sharedMaterials.Length);
                newSharedMat[sharedMaterials.Length] = slicedSectionMat;

                // the the material information
                newObject.GetComponent<Renderer>().sharedMaterials = newSharedMat;
            }

            return newObject;
        }

        public GameObject CreateLowerMesh(GameObject originalObject, Material crossSectionMat) {
            GameObject newObject = CreateLowerMesh();

            if (newObject != null) {
                newObject.transform.localPosition = originalObject.transform.localPosition;
                newObject.transform.localRotation = originalObject.transform.localRotation;
                newObject.transform.localScale = originalObject.transform.localScale;

                Material[] sharedMaterials = originalObject.GetComponent<MeshRenderer>().sharedMaterials;
                Mesh mesh = originalObject.GetComponent<MeshFilter>().sharedMesh;

                // nothing changed in the hierarchy, the cross section must have been batched
                // with the submeshes, return as is, no need for any changes
                if (mesh.subMeshCount == lowerMesh.subMeshCount) {
                    // the the material information
                    newObject.GetComponent<Renderer>().sharedMaterials = sharedMaterials;

                    return newObject;
                }

                // otherwise the cross section was added to the back of the submesh array because
                // it uses a different material. We need to take this into account
                Material[] newSharedMat = new Material[sharedMaterials.Length + 1];

                // copy our material arrays across using native copy (should be faster than loop)
                System.Array.Copy(sharedMaterials, newSharedMat, sharedMaterials.Length);
                newSharedMat[sharedMaterials.Length] = crossSectionMat;

                // the the material information
                newObject.GetComponent<Renderer>().sharedMaterials = newSharedMat;
            }

            return newObject;
        }

        /**
         * Generate a new GameObject from the upper hull of the mesh
         * This function will return null if upper hull does not exist
         */
        public GameObject CreateUpperMesh() {
            return CreateEmptyObject("Upper_Hull", upperMesh);
        }

        /**
         * Generate a new GameObject from the Lower hull of the mesh
         * This function will return null if lower hull does not exist
         */
        public GameObject CreateLowerMesh() {
            return CreateEmptyObject("Lower_Hull", lowerMesh);
        }

        /**
         * Helper function which will create a new GameObject to be able to add
         * a new mesh for rendering and return.
         */
        private static GameObject CreateEmptyObject(string name, Mesh hull) {
            if (hull == null) {
                return null;
            }

            GameObject newObject = new GameObject(name);

            newObject.AddComponent<MeshRenderer>();
            MeshFilter filter = newObject.AddComponent<MeshFilter>();

            filter.mesh = hull;

            return newObject;
        }
    }
}