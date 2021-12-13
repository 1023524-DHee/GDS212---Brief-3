using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HorrorVR
{
    public class AnimEvent : MonoBehaviour
    {
        public UnityEvent Event;
        
        public void TriggerEvent ()
        {
            Event?.Invoke ();
        }
    }
}
