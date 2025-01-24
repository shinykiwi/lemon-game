using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EzySlice {

    /**
     * A Basic Structure which contains intersection information
     * for Plane->Triangle Intersection Tests
     * TO-DO -> This structure can be optimized to hold less data
     * via an optional indices array. Could lead for a faster
     * intersection test aswell.
     */
    public sealed class IntersectionResult {

        // general tag to check if this structure is valid
        private bool isSuccess;

        // our intersection points/triangles
        private readonly Triangle[] upperMesh;
        private readonly Triangle[] lowerMesh;
        private readonly Vector3[] intersectionPoint;

        // our counters. We use raw arrays for performance reasons
        private int upperMeshCount;
        private int lowerMeshCount;
        private int intersectionPtCount;

        public IntersectionResult() {
            this.isSuccess = false;

            this.upperMesh = new Triangle[2];
            this.lowerMesh = new Triangle[2];
            this.intersectionPoint = new Vector3[2];

            this.upperMeshCount = 0;
            this.lowerMeshCount = 0;
            this.intersectionPtCount = 0;
        }

        public Triangle[] UpperMesh {
            get { return upperMesh; }
        }

        public Triangle[] LowerMesh {
            get { return lowerMesh; }
        }

        public Vector3[] intersectionPoints {
            get { return intersectionPoint; }
        }

        public int UpperMeshCount {
            get { return upperMeshCount; }
        }

        public int LowerMeshCount {
            get { return lowerMeshCount; }
        }

        public int intersectionPointCount {
            get { return intersectionPtCount; }
        }

        public bool isValid {
            get { return isSuccess; }
        }

        /**
         * Used by the intersector, adds a new triangle to the
         * upper hull section
         */
        public IntersectionResult AddUpperHull(Triangle tri) {
            upperMesh[upperMeshCount++] = tri;

            isSuccess = true;

            return this;
        }

        /**
         * Used by the intersector, adds a new triangle to the
         * lower gull section
         */
        public IntersectionResult AddLowerHull(Triangle tri) {
            lowerMesh[lowerMeshCount++] = tri;

            isSuccess = true;

            return this;
        }

        /**
         * Used by the intersector, adds a new intersection point
         * which is shared by both upper->lower hulls
         */
        public void AddIntersectionPoint(Vector3 pt) {
            intersectionPoint[intersectionPtCount++] = pt;
        }

        /**
         * Clear the current state of this object 
         */
        public void Clear() {
            isSuccess = false;
            upperMeshCount = 0;
            lowerMeshCount = 0;
            intersectionPtCount = 0;
        }
    }
}