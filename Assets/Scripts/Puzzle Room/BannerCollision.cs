using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR
{
    public class BannerCollision : MonoBehaviour
    {
        [Tooltip("Tag on the sword object.")]
        public string swordTag;

        private void OnTriggerExit(Collider sword)
        {
            if (sword.CompareTag(swordTag))
            {
                transform.position = new Vector3(0f, 0.076f, 0f);
            }
        }
    }
}
