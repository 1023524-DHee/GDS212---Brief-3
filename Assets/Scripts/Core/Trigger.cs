using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HorrorVR.Core
{
    public class Trigger : MonoBehaviour
    {
        [SerializeField]
        private bool disableOnTrigger = true;

        public UnityEvent onTriggerEnter = new UnityEvent();
        public UnityEvent onTriggerExit = new UnityEvent();
        public UnityEvent onCharacterControllerHit = new UnityEvent();

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            onCharacterControllerHit.Invoke();
            if (disableOnTrigger) gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            onTriggerEnter.Invoke();
            if (disableOnTrigger) gameObject.SetActive(false);
        }

        private void OnTriggerExit(Collider other)
        {
            onTriggerExit.Invoke();
            if (disableOnTrigger) gameObject.SetActive(false);
        }
    }
}
