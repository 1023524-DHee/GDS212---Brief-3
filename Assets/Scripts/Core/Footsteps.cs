using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR.Core
{
    public class Footsteps : MonoBehaviour
    {
        [SerializeField]
        private EventReference footstepEvent;

        [SerializeField]
        private Transform footstepPosition;

        public void Step()
        {
            RuntimeManager.PlayOneShot(footstepEvent, footstepPosition.position);
        }
    }
}
