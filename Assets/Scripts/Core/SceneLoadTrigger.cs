using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR.Core
{
    public class SceneLoadTrigger : MonoBehaviour
    {
        public string levelToLoad;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                MovementTypeManager.current.Loadlevel(levelToLoad);
            }
        }
    }
}
