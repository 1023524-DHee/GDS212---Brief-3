using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR.Catacombs
{
    public class BlockEvent : HackAndSlashEvent
    {
        [SerializeField]
        private float minimumDistance;

        [SerializeField, Range(0f, 90f), Tooltip("0 = exact, 45 = 90 degree safezone (-45 deg to 45 deg offset), 90 = any rotation")]
        private float minimumRotation;

        protected override bool HasFulfilledRequirements()
        {
            // This gets us how close our rotations are to the correct orientation
            float dot = Quaternion.Dot(Sword.Instance.transform.rotation, transform.rotation);

            // This makes sure we can have the sword in either direction, no specific direction needed (parallel instead of exact)
            dot = Mathf.Abs(dot);

            // Distance... (we want to check the distance from the sword to the projected line from the camera to this event)
            Mathfx.ClosestPointOnLineSegment(Camera.main.transform.position, transform.position, Sword.Instance.midPoint.position, out Vector3 pointOnLine, false);
            float distance = Vector3.Distance(Sword.Instance.midPoint.position, pointOnLine);

            Debug.Log(distance);

            // If we are within the min distance and min rotation, then we have fulfilled the requirements
            if (distance <= minimumDistance && dot > (90f - minimumRotation) / 90f)
            {
                Debug.Log("Blocked attack");
                return true;
            }

            return false;
        }


        // Old Method (non-projected, yeah yucky yucky)
        //protected override bool HasFulfilledRequirements()
        //{
        //    // This gets us how close our rotations are to the correct orientation
        //    float dot = Quaternion.Dot(Sword.Instance.transform.rotation, transform.rotation);

        //    // This makes sure we can have the sword in either direction, no specific direction needed (parallel instead of exact)
        //    dot = Mathf.Abs(dot);

        //    // Distance...
        //    float distance = Vector3.Distance(Sword.Instance.transform.position, transform.position);

        //    // If we are within the min distance and min rotation, then we have fulfilled the requirements
        //    if (distance <= minimumDistance && dot <= minimumRotation)
        //    {
        //        Debug.Log("Blocked attack");
        //        return true;
        //    }

        //    return false;
        //}
    }
}
