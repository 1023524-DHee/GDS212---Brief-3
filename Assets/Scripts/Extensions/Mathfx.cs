using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace HorrorVR.Extensions
//{
    public static class Mathfx
    {
    /// <summary>
    /// Find the closest Vector3 position to a given point on a line segment
    /// </summary>
    /// <param name="segmentStart">Start of the segment</param>
    /// <param name="segmentEnd">End of the segment</param>
    /// <param name="point">The point to find the closest point on the segment</param>
    /// <param name="isClamped">Whether the point is retricted to the line segment, or if it goes on to infinite</param>
    /// <returns>True if the point is on the segment (always true if clamped)</returns>
    public static bool ClosestPointOnLineSegment(Vector3 segmentStart, Vector3 segmentEnd, Vector3 point, out Vector3 pointOnLine, bool isClamped = false)
        {
            // Shift the problem to the origin to simplify the math.    
            var wander = point - segmentStart;
            var span = segmentEnd - segmentStart;

            // Compute how far along the line is the closest approach to our point.
            float distanceOnLine = Vector3.Dot(wander, span) / span.sqrMagnitude;

            if (isClamped)
            {
                // Restrict this point to within the line segment from start to end.
                distanceOnLine = Mathf.Clamp01(distanceOnLine);
            }

            pointOnLine = segmentStart + distanceOnLine * span;

            // Return zero if we are not on the line, this is so we can not play the sound if the vector is zero
            if (!isClamped && (distanceOnLine < 0f || distanceOnLine > 1f))
            {
                return false;// Vector3.zero;
            }
            else
            {
                return true;
            }
        }
    }
//}
