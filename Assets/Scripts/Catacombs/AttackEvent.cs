using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HorrorVR.Catacombs.Sword;

namespace HorrorVR.Catacombs
{
    public class AttackEvent : HackAndSlashEvent
    {
        [SerializeField]
        private float minimumDistance = 0.25f;

        [SerializeField, Range(0f, 90f), Tooltip("0 = exact, 45 = 90 degree safezone (-45 deg to 45 deg offset), 90 = any rotation")]
        private float minimumRotation = 30f;

        private void Update()
        {
            // Detect swing
            // Check if swing is correct orientation and position
            // Call Complete() if it is correct

            SwordSwipe swipe = Sword.Instance.GetVelocityVector();

            if (swipe.isFastEnough)
            {
                // Get the distance from the swipe point to the closest point on the event projection
                Mathfx.ClosestPointOnLineSegment(Camera.main.transform.position, transform.position, swipe.midPoint, out Vector3 pointOnLine, false);
                float distance = Vector3.Distance(swipe.midPoint, pointOnLine);

                // Get the rotation dot product from the swipe velocity and the event rotation
                Vector3 swipeVelocityNormalised = swipe.velocity;
                Quaternion swipeRotation = Quaternion.LookRotation(swipeVelocityNormalised, Vector2.up);
                float dot = Mathf.Abs(Quaternion.Dot(swipeRotation, transform.rotation)); // Absolute allows for the swipe to be in either direction, as long as it's parralell

                // If we are within the min distance and min rotation, then we have fulfilled the requirements
                if (distance <= minimumDistance && dot > (90f - minimumRotation) / 90f)
                {
                    Complete();
                }
            }
        }
    }
}
