using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HorrorVR
{
    public class GenericInspectable : MonoBehaviour, IInspectable
    {
        [SerializeField] private bool singleUse;
        [Range (10, 90)]
        [SerializeField] private float angleThreshold;
        [SerializeField] private float distanceThreshold;
        [SerializeField] private UnityEvent OnInspectEvent, OnInspectEndEvent;

        public bool DisableAfterNextUse => singleUse;
        public float AngleThreshold => angleThreshold;
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
            Gizmos.DrawWireSphere (transform.position, distanceThreshold);
        }
    }
}
