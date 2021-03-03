using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VoxelPlay
{
    public partial struct Rayd
    {
        private Vector3d m_Origin;
        private Vector3 m_Direction;

        // Creates a ray starting at /origin/ along /direction/.
        public Rayd (Vector3d origin, Vector3 direction)
        {
            m_Origin = origin;
            m_Direction = direction.normalized;
        }


        // The origin point of the ray.
        public Vector3d origin {
            get { return m_Origin; }
            set { m_Origin = value; }
        }

        // The direction of the ray.
        public Vector3 direction {
            get { return m_Direction; }
            set { m_Direction = value.normalized; }
        }

        // Returns a point at /distance/ units along the ray.
        public Vector3d GetPoint (float distance)
        {
            return m_Origin + m_Direction * distance;
        }

        public override string ToString ()
        {
            return ToString (null);
        }

        public string ToString (string format)
        {
            if (string.IsNullOrEmpty (format))
                format = "F1";
            return string.Format ("Origin: {0}, Dir: {1}", m_Origin.ToString (format), m_Direction.ToString (format));
        }


        [MethodImpl (256)]
        public static implicit operator Rayd (Ray worldSpaceRay)
        {
            return new Rayd (worldSpaceRay.origin, worldSpaceRay.direction);
        }
    }
}

