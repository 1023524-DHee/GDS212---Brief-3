using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace HorrorVR.Catacombs
{
    public class Sword : MonoBehaviour
    {
        public static Sword Instance;

        public Transform midPoint;
        public Transform tip;

        public float minimumVelocity = 5f;

        private Queue<Line> velocities = new Queue<Line>();
        private struct Line
        {
            public Vector3 from;
            public Vector3 to;
            public float velocity;
        }

        public struct SwordSwipe
        {
            public Vector3 midPoint;
            public Vector3 velocity;
            public bool isFastEnough;
        }

        private Vector3 previousPosition;

        private void Start()
        {
            Instance = this;

            previousPosition = tip.position;
        }

        private void FixedUpdate()
        {
            // Keep track of 4 data points for the velocities/swipe
            if (velocities.Count > 4)
            {
                velocities.Dequeue();
            }

            // Add the velocity to the queue
            velocities.Enqueue(new Line { from = previousPosition, to = tip.position, velocity = Vector3.Distance(tip.position, previousPosition) / Time.deltaTime });
            previousPosition = tip.position;
        }

        public SwordSwipe GetVelocityVector()
        {
            // Get the start and end of the swipe
            Vector3 starting = velocities.First().from;
            Vector3 ending = velocities.Last().to;

            // Set the value to be 0 = none, >1 = is fast enough XD
            float averageVelocity = velocities.Average(a => a.velocity);
            float value = averageVelocity / minimumVelocity;

            // Pass these values into a SwordSwipe
            return new SwordSwipe
            {
                midPoint = (starting + ending) / 2f,
                velocity = (ending - starting).normalized * averageVelocity, // Get the direction from the start to the end, and then multiply it by the average velocity. This will give the velocity vector of the swipe
                isFastEnough = value >= 1f
            };
        }

#if UNITY_EDITOR
        // Draw the sword swipe
        private void OnDrawGizmos()
        {
            if (velocities.Count > 0)
            {
                Vector3 starting = velocities.First().from;
                Vector3 ending = velocities.Last().to;

                float averageVelocity = velocities.Average(a => a.velocity);

                float value = averageVelocity / minimumVelocity;

                Gizmos.color = Color.Lerp(Color.green, Color.red, value);
                Gizmos.DrawLine(starting, ending);
            }
        }
    }
#endif
}
