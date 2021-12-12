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

        [SerializeField]
        private bool destroyOnFinish = false;

        public bool isFinished { private set; get; } = false;
        public bool hasStarted { private set; get; } = false;

        public UnityEvent onStarted = new UnityEvent();
        public UnityEvent onSucceeded = new UnityEvent();
        public UnityEvent onFailed = new UnityEvent();

        public EnemyController perpetrator;

        private void Start()
        {
            //slider.SetValueWithoutNotify(0f);
            slider.value = 0f;

            onStarted.Invoke();

            StartCoroutine(Sequence());
        }

        IEnumerator Sequence()
        {
            hasStarted = true;
            float currentTime = 0f;
            while (true)
            {
                currentTime += Time.deltaTime;
                
                // Increase slider value
                if (slider)
                {
                    slider.value = (currentTime / time);
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

                    isFinished = true;

                    if (destroyOnFinish)
                    {
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        break;
                    }
                }

                yield return null;
            }
        }

        protected virtual bool HasFulfilledRequirements()
        {
            return false;
        }

        /// <summary>
        /// Complete the event before the timer runs out/before HasFulfilledRequirements check
        /// </summary>
        protected void Complete()
        {
            StopAllCoroutines();
            slider.value = 1f; //slider.SetValueWithoutNotify(1f);
            onSucceeded.Invoke();
            isFinished = true;

            if (destroyOnFinish)
            {
                gameObject.SetActive(false);
            }
        }

        public void Destroy()
        {
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}