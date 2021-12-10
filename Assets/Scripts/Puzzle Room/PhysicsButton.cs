using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HorrorVR
{
    public class PhysicsButton : MonoBehaviour
    {
        [SerializeField] private float threshold = 0.1f;
        [SerializeField] private float deadZone = 0.025f;

        private bool isPressed;
        private Vector3 startPosition;
        private ConfigurableJoint joint;

        public UnityEvent onPressed, onReleased;

        void Start()
        {
            startPosition = transform.localPosition;
            joint = GetComponent<ConfigurableJoint>();
        }

        void Update()
        {
          if (!isPressed && GetValue() + threshold >= 1)
          {
                Pressed();
          }

          if(isPressed && GetValue() - threshold <= 0)
          {
                Released();
          }
        }

        private float GetValue()
        {
            var value = Vector3.Distance(startPosition, transform.localPosition) / joint.linearLimit.limit;

            if (Math.Abs(value) < deadZone)
            {
                value = 0;
            }

            return Mathf.Clamp(value, -1f, 1f);
        }

        private void Pressed()
        {
            isPressed = true;
            onPressed.Invoke();
        }

        private void Released()
        {
            isPressed = false;
            onReleased.Invoke();
        }
    }
}
