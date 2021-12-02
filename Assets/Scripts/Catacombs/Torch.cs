using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR
{
    public class Torch : MonoBehaviour
    {
        public GameObject Light;

        // Update is called once per frame
        //void OnCollisionEnter(Collision collision)
        //{
        //    if (collision.gameObject.tag == "Torch")
        //    {
        //        Light.SetActive(true);
        //        //StartCoroutine(TorchCoroutine());
        //    }
        //}

        void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Light.SetActive(true);
            }
        }

        
        //IEnumerator TorchCoroutine()
        //{
        //    yield return new WaitForSeconds(15);
        //    Light.SetActive(false);
        //}
    }
}
