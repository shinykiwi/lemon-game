using System.Collections;
using UnityEngine;

namespace EzySlice {
    /**
     * Define Extension methods for easy access to slicer functionality
     */
    public static class SlicerExtensions {
        public static GameObject[] SliceInstantiate(this GameObject obj, Vector3 position, Vector3 direction) {
            return SliceInstantiate(obj, position, direction, null);
        }

        public static GameObject[] SliceInstantiate(this GameObject obj, Vector3 position, Vector3 direction, Material crossSectionMat) {
            return SliceInstantiate(obj, position, direction, new TextureRegion(0.0f, 0.0f, 1.0f, 1.0f), crossSectionMat);
        }

        public static GameObject[] SliceInstantiate(this GameObject obj, Vector3 position, Vector3 direction, TextureRegion cuttingRegion, Material crossSectionMaterial = null) {
            EzySlice.Plane cuttingPlane = new EzySlice.Plane();

            Matrix4x4 mat = obj.transform.worldToLocalMatrix;
            Matrix4x4 transpose = mat.transpose;
            Matrix4x4 inv = transpose.inverse;

            Vector3 refUp = inv.MultiplyVector(direction).normalized;
            Vector3 refPt = obj.transform.InverseTransformPoint(position);

            cuttingPlane.Compute(refPt, refUp);

            return SliceInstantiate(obj, cuttingPlane, cuttingRegion, crossSectionMaterial);
        }

        public static GameObject[] SliceInstantiate(this GameObject obj, Plane pl, TextureRegion cuttingRegion, Material crossSectionMaterial = null) {
            SlicedMesh slice = Slicer.Slice(obj, pl, cuttingRegion, crossSectionMaterial);

            if (slice == null) {
                return null;
            }

            GameObject upperHull = slice.CreateUpperMesh(obj, crossSectionMaterial);
            GameObject lowerHull = slice.CreateLowerMesh(obj, crossSectionMaterial);
            
            upperHull.AddComponent<Rigidbody>();
            lowerHull.AddComponent<Rigidbody>();

            

            if (upperHull != null && lowerHull != null) {
                return new GameObject[] { upperHull, lowerHull };
            }

            // otherwise return only the upper hull
            if (upperHull != null) {
                return new GameObject[] { upperHull };
            }

            // otherwise return only the lower hull
            if (lowerHull != null) {
                return new GameObject[] { lowerHull };
            }

            // nothing to return, so return nothing!
            return null;
        }
    }
}