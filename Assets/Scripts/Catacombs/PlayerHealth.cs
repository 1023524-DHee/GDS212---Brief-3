using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace HorrorVR.Catacombs
{
    public class PlayerHealth : MonoBehaviour
    {
        public static PlayerHealth current { private set; get; }

        [SerializeField]
        private float maxDamage = 4f;
        [SerializeField]
        private float timeToFullyHeal = 3f;
        [SerializeField]
        private float timeBeforeHeal = 3f;
        [SerializeField]
        private float damagePerHit = 1f;
        [SerializeField]
        private float lerpSpeed = 5f;

        private float currentDamage = 0f;
        private float lerpedDamage = 0f;

        [SerializeField]
        private Volume playerDamageVolume;


        // Start is called before the first frame update
        void Start()
        {
            current = this;

            if (playerDamageVolume == null) Debug.LogError("No volume added, player will not see any effects when damaged. If you want them, please add the volume to 'playerDamageVolume''");
        }

        private void Update()
        {
            maxDamage += Time.deltaTime;

            // Wanna smooth that bitch
            lerpedDamage = Mathf.Lerp(lerpedDamage, currentDamage, Time.deltaTime * lerpSpeed);

            // Squared (or more) feels better than linear!!
            if (playerDamageVolume != null) playerDamageVolume.weight = Mathf.Pow(lerpedDamage / maxDamage, 2f);
        }

        public void Damage()
        {
            currentDamage = Mathf.Clamp(currentDamage + damagePerHit, 0f, maxDamage);
            StartHealing();
        }

        private void StartHealing()
        {
            StopAllCoroutines();
            StartCoroutine(Heal());
        }

        private IEnumerator Heal()
        {
            // Wait for a bit before healing
            yield return new WaitForSeconds(timeBeforeHeal);

            // Decrease damage until we reach zero
            while (currentDamage > 0f)
            {
                currentDamage -= (Time.deltaTime * maxDamage) / timeToFullyHeal;
                yield return null;
            }

            currentDamage = Mathf.Clamp(currentDamage, 0f, maxDamage);
        }
    }
}
