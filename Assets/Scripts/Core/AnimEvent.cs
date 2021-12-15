using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HorrorVR
{
    public class AnimEvent : MonoBehaviour
    {
        public UnityEvent[] Events;
        
        public void TriggerEvent (int eventNum)
        {
            Events[eventNum]?.Invoke ();
        }
    }
}
