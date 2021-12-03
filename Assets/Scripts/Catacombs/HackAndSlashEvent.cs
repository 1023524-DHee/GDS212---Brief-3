using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HorrorVR.Catacombs
{
    public class HackAndSlashEvent : MonoBehaviour
    {
        public float time;

        [SerializeField]
        private Slider slider;

        public bool isFinished { private set; get; } = false;

        public UnityEvent onStarted = new UnityEvent();
        public UnityEvent onSucceeded = new UnityEvent();
        public UnityEvent onFailed = new UnityEvent();

        private void Start()
        {
            slider.SetValueWithoutNotify(0f);

            onStarted.Invoke();

            StartCoroutine(Sequence());
        }

        IEnumerator Sequence()
        {
            float currentTime = 0f;
            while (true)
            {
                currentTime += Time.deltaTime;
                
                // Increase slider value
                if (slider)
                {
                    slider.SetValueWithoutNotify(currentTime / time);
                }

                // If timer runs out, check if the player fulfilled the event's requirements
                if (currentTime >= time)
                {
                    if (HasFulfilledRequirements())
                    {
                        onSucceeded.Invoke();
                    }
                    else
                    {
                        onFailed.Invoke();
                        Debug.Log("Failed");
                    }

                    gameObject.SetActive(false);
                    isFinished = true;
                }

                yield return null;
            }
        }

        protected virtual bool HasFulfilledRequirements()
        {
            return true;
        }

        /// <summary>
        /// Complete the event before the timer runs out/before HasFulfilledRequirements check
        /// </summary>
        protected void Complete()
        {
            StopCoroutine(Sequence());
            slider.SetValueWithoutNotify(1f);
            onSucceeded.Invoke();
            isFinished = true;
            gameObject.SetActive(true);
        }
    }
}