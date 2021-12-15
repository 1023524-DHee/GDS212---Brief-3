using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace HorrorVR
{
    public class OpenDoorTorch : MonoBehaviour
    {
        public GameObject door;
        public bool isNegative;
        public float openTime = 3;

        public bool isOpen = false;
        public DoorGroup doorGroupParent;
        public bool doorGroupCheckIsOpen;

        public FMODUnity.EventReference doorSound;

        private Quaternion initialRotation;

        // Start is called before the first frame update
        void Start()
        {
            initialRotation = door.transform.rotation;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!isOpen)
            {
                if (doorGroupParent != null)
                {
                    doorGroupCheckIsOpen = true;
                    doorGroupParent.CheckOpenDoor();
                }
                else if (doorGroupParent == null)
                {
                    OpenDoor();
                }
            }
            
        }

        public void OpenDoor()
        {
            StartCoroutine(OpenDoor_Coroutine());
            var instance = RuntimeManager.CreateInstance(doorSound);
            instance.setVolume(50f);
            RuntimeManager.AttachInstanceToGameObject(instance, transform);
            instance.start();
            instance.release();
        }

        IEnumerator OpenDoor_Coroutine()
        {
            isOpen = true;
            Quaternion newAngle = Quaternion.Euler(0, initialRotation.y + (isNegative ? -90f : 90f), 0);
            float startTime = Time.time;

            while(Time.time < startTime + openTime)
            {
                door.transform.rotation = Quaternion.Lerp(initialRotation, newAngle, (Time.time - startTime) / openTime);
                yield return null;
            }
        }
    }
}
