using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HorrorVR
{
    public class CandleTracker : MonoBehaviour
    {
        public GameObject[] candles;
        public UnityEvent allActiveEvent;

        private bool eventTriggered = false;

        private void Update ()
        {
            if (eventTriggered) return;

            bool allTorchesActive = true;
            foreach (GameObject candle in candles)
            {
                if (!candle.activeInHierarchy)
                {
                    allTorchesActive = false;
                    break;
                }
            }

            if (allTorchesActive)
            {
                allActiveEvent?.Invoke ();
                eventTriggered = true;
            }
        }
    }
}
