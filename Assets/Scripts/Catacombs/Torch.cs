using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR
{
    public class Torch : MonoBehaviour
    {
        public GameObject torchGO;
        public float torchTime;

        void OnTriggerEnter(Collider Torch)
        {
            if (Torch.gameObject.tag == "Torch")
            {
                torchGO.SetActive(true);
                StopCoroutine(TorchCoroutine());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            StartCoroutine(TorchCoroutine());
        }


        IEnumerator TorchCoroutine()
        {
            torchGO.SetActive(true);
            float startTime = Time.time;

            while(Time.time < (startTime + torchTime))
            {
                torchGO.GetComponent<Light>().intensity = Mathf.Lerp(3, 0.1f, (Time.time - startTime) / torchTime);
                yield return null;
            }
            torchGO.SetActive(false);
        }
    }
}
