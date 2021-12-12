using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;
using HorrorVR.Core;


namespace HorrorVR.Catacombs
{
    public class Pauser : MonoBehaviour
    {
        private int pausers = 0; // Keep track of how many things want to keep pause the player

        public UnityEvent onPause = new UnityEvent();
        public UnityEvent onUnpause = new UnityEvent();


        public static Pauser current { private set; get; }

        private void OnEnable()
        {
            current = this;
        }

        private void OnDisable()
        {
            if (current == this)
            {
                current = null;
            }
        }

        public void Pause()
        {
            if (pausers == 0)
            {
                onPause.Invoke();
                MovementTypeManager.current.DisableMovement();
            }

            pausers++;
        }

        public void Unpause()
        {
            if (pausers > 0)
            {
                pausers--;
            }

            if (pausers == 0)
            {
                onUnpause.Invoke();
                MovementTypeManager.current.EnableMovement();
            }
        }
    }
}
