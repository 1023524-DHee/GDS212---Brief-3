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
            if (!isFinished)
            {
                // Detect swing
                // Check if swing is correct orientation and position
                // Call Complete() if it is correct

                SwordSwipe swipe = Sword.Instance.GetVelocityVector();

                if (swipe.isFastEnough)
                {
                    // Get the distance from the swipe point to the closest point on the event projection
                    //Mathfx.ClosestPointOnLineSegment(Camera.main.transform.position, transform.position, swipe.midPoint, out Vector3 pointOnLine, false);
                    Mathfx.ClosestPointOnLineSegment(swipe.start, swipe.end, transform.position, out Vector3 closestSwipePointToEvent);
                    Mathfx.ClosestPointOnLineSegment(Camera.main.transform.position, transform.position, closestSwipePointToEvent, out Vector3 pointOnLine, false);
                    float distance = Vector3.Distance(swipe.midPoint, pointOnLine);

                    // Get the angle from the swipe and the event rotation
                    // We do both normal and inverse as my parallel thing wasn't working >:|
                    Vector3 swipeVelocityNormalised = (swipe.end - swipe.start).normalized;
                    Vector3 inverseSwipeVelocityNormalised = (swipe.start - swipe.end).normalized;
                    Quaternion swipeRotation = Quaternion.LookRotation(Camera.main.transform.forward, swipeVelocityNormalised);
                    Quaternion inverseSwipeRotation = Quaternion.LookRotation(Camera.main.transform.forward, inverseSwipeVelocityNormalised);
                    float angle = Mathf.Abs(Quaternion.Angle(swipeRotation, transform.rotation));
                    float inverseAngle = Mathf.Abs(Quaternion.Angle(inverseSwipeRotation, transform.rotation));

                    // If we are within the min distance and min rotation, then we have fulfilled the requirements
                    if (distance <= minimumDistance && (angle < minimumRotation || inverseAngle < minimumRotation))// > (90f - minimumRotation) / 90f)
                    {
                        perpetrator.Damage(1);
                        Complete();
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (Sword.Instance)
            {
                SwordSwipe swipe = Sword.Instance.GetVelocityVector();

                Mathfx.ClosestPointOnLineSegment(Camera.main.transform.position, transform.position, swipe.midPoint, out Vector3 pointOnLine, false);

                Gizmos.color = Color.blue;
                Gizmos.DrawLine(swipe.midPoint, pointOnLine);

                //Vector3 swipeVelocityNormalised = swipe.velocity.normalized;
                //Quaternion swipeRotation = Quaternion.LookRotation(swipeVelocityNormalised, Vector2.up);
                //Gizmos.color = Color.yellow;
                //Gizmos.DrawLine(swipe.midPoint, );
            }
        }
    }
}