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

        public UnityEvent onStarted = new UnityEvent();
        public UnityEvent onSucceeded = new UnityEvent();
        public UnityEvent onFailed = new UnityEvent();

        private void Start()
        {
            slider.value = 0f;

            onStarted.Invoke();

            StartCoroutine(Sequence());
        }

        IEnumerator Sequence()
        {
            float currentTime = 0f;
            while (true)
            {
                currentTime += Time.deltaTime;

                if (slider)
                {
                    slider.value = currentTime / time;
                }

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

                    Destroy(gameObject);
                }

                yield return null;
            }
        }

        protected virtual bool HasFulfilledRequirements()
        {
            return true;
        }
    }
}