using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR
{
    public class BannerCollision : MonoBehaviour
    {
        [Tooltip("Tag on the sword object.")]
        public string swordTag;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(swordTag))
            {
                transform.position = new Vector3(0f, +5.599243f, 0f);
            }
        }
    }
}
