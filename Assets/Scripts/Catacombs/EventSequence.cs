using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR.Catacombs
{
    public class EventSequence : MonoBehaviour
    {
        [SerializeField, Tooltip("Make sure that wait time and event time are the same for any added AttackEvent")]
        private EventStruct[] events;

        [Tooltip("Whether or not to play attack animation")]
        public bool isAttack = true;

        [Tooltip("This is the attack value found in the animator to choose which attack pattern animation to play")]
        public int attack;

        [Tooltip("Time until the animation starts playing")]
        public float timeUntilAttacks;

        public bool isFinished { private set; get; } = false;

        public EnemyController perpetrator;

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
            // Loop through each attack event and enable them at the right times
            int eventIndex = 0;
            while(eventIndex < events.Length)
            {
                if (perpetrator.IsDead) break;

                events[eventIndex].attackEvent.perpetrator = perpetrator;
                events[eventIndex].attackEvent.time = events[eventIndex].eventTime;
                events[eventIndex].attackEvent.gameObject.SetActive(true);

                yield return new WaitForSeconds(events[eventIndex].waitTime);

                eventIndex++;
            }

            // Wait until all the events have been completed
            yield return new WaitUntil(() =>
            {
                foreach (var e in events)
                {
                    if ((e.attackEvent.hasStarted && !e.attackEvent.isFinished))
                    {
                        return false;
                    }
                }

                return true;
            });

            isFinished = true;

            // Destroy the object when all events have been deactivated
            while (true)
            {
                bool isFinished = true;
                foreach (var e in events)
                {
                    if (e.attackEvent.gameObject.activeSelf)
                    {
                        isFinished = false;
                    }
                }
                if (isFinished)
                {
                    break;
                }

                yield return null;
            }
            //Destroy(gameObject);
        }
    }
}
