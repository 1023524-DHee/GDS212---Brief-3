using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace HorrorVR
{
    public class BannerCollision : MonoBehaviour
    {
        [Tooltip("Tag on the sword object.")]
        public string swordTag;

        private Vector3 initialPosition;
        private Vector3 endPosition;

        public EventReference bannerSound;

		private void Start()
		{
            initialPosition = transform.position;
            endPosition = initialPosition + new Vector3(0, -75f, 0);
        }

        private void BannerDrop_Start()
        {
            StartCoroutine(BannerDrop());
        }
        
		private void OnTriggerEnter(Collider sword)
        {
            if (sword.CompareTag(swordTag))
            {
                StartCoroutine(BannerDrop());
            }
        }

        private IEnumerator BannerDrop()
        {
            float startTime = Time.time;

            var instance = RuntimeManager.CreateInstance(bannerSound);
            RuntimeManager.AttachInstanceToGameObject(instance, transform);
            instance.setVolume(1f);
            instance.start();
            
            while (Time.time < startTime + 3f)
            {
                transform.position = Vector3.Lerp(initialPosition, endPosition, (Time.time - startTime) / 3f);
                yield return null;
            }

            instance.stop ();
        }
    }
}
