using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HorrorVR
{
    public class GenericInspectable : MonoBehaviour, IInspectable
    {
        [SerializeField] private bool singleUse;
        [Range (10, 180)]
        [SerializeField] private float lookAngleThreshold;
        [Range (10, 360)]
        [SerializeField] private float relativeAngleThreshold;
        [SerializeField] private float distanceThreshold;
        [SerializeField] private UnityEvent OnInspectEvent, OnInspectEndEvent;

        public bool DisableAfterNextUse => singleUse;
        public float LookAngleThreshold => lookAngleThreshold;
        public float RelativeAngleThreshold => relativeAngleThreshold;
        public float DistanceThreshold => distanceThreshold;
        public Transform Transform => transform;

        void IInspectable.OnInspect ()
        {
            OnInspectEvent?.Invoke ();
        }

        void IInspectable.OnInspectEnd ()
        {
            OnInspectEndEvent?.Invoke ();
        }

        private void OnDrawGizmos ()
        {
            //Gizmos.DrawWireSphere (transform.position, distanceThreshold);
            float forwardAngle = Mathf.Atan2 (transform.forward.z, transform.forward.x);
            Debug.DrawRay (transform.position, new Vector3 (Mathf.Cos (forwardAngle - relativeAngleThreshold * Mathf.Deg2Rad), 0, Mathf.Sin (forwardAngle - relativeAngleThreshold * Mathf.Deg2Rad)) * distanceThreshold);
            Debug.DrawRay (transform.position, new Vector3 (Mathf.Cos (forwardAngle + relativeAngleThreshold * Mathf.Deg2Rad), 0, Mathf.Sin (forwardAngle + relativeAngleThreshold * Mathf.Deg2Rad)) * distanceThreshold);
        }
    }
}
