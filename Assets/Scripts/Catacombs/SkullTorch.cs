using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR
{
    public class SkullTorch : MonoBehaviour
    {
        public List<GameObject> listOfTorches;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                foreach (GameObject torch in listOfTorches)
                {
                    torch.SetActive(true);
                }
            }

            if(other.CompareTag("Bob"))
            {
                foreach (GameObject torch in listOfTorches)
                {
                    torch.SetActive(false);
                }
            }
        }
    }
}
