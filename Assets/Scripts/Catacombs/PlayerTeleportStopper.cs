using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR.Core
{
    public class PlayerTeleportStopper : MonoBehaviour
    {
        [SerializeField]
        private Transform stopPosition;

        public Transform StopPosition
        {
            get
            {
                return stopPosition;
            }
        }
    }
}
