using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR.Catacombs
{
    public class EventSequence : MonoBehaviour
    {
        [SerializeField]
        private EventStruct[] events;

        public int attack;
        public float timeUntilAttacks;

        [Serializable]
        private struct EventStruct
        {
            public float waitTime;
            public float eventTime;
            public HackAndSlashEvent attackEvent;
        }

        private void Start()
        {
            StartCoroutine(Sequence());
        }

        private IEnumerator Sequence()
        {
            int eventIndex = 0;
            while(eventIndex < events.Length)
            {
                events[eventIndex].attackEvent.gameObject.SetActive(true);
                events[eventIndex].attackEvent.time = events[eventIndex].eventTime;

                yield return new WaitForSeconds(events[eventIndex].waitTime);

                eventIndex++;
            }
        }
    }
}
