using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR
{
    public class LineSegmentTest : MonoBehaviour
    {
        [SerializeField]
        private Transform point;
        [SerializeField]
        private bool isClamped = false;

        private void OnDrawGizmos()
        {
            if (point != null)
            {
                Vector3 from = transform.position - transform.forward * 10f;
                Vector3 to = transform.position + transform.forward * 10f;

                Mathfx.ClosestPointOnLineSegment(from, to, point.position, out Vector3 closestPointOnLine, isClamped);

                Gizmos.color = Color.white;
                Gizmos.DrawLine(from, to);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(point.position, closestPointOnLine);
            }
        }
    }
}
