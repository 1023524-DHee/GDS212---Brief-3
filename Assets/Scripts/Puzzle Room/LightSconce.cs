using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSconce : MonoBehaviour
{
    public GameObject fire;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Torch"))
        {
           Instantiate(fire);
           fire.transform.position = transform.position;
            Destroy(this);
        }
    }
}
