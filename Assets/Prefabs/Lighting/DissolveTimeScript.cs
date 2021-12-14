using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR
{
    
    public class DissolveTimeScript : MonoBehaviour
    {
        public Material dissolveMaterial;
        public float dissolveTimer;
        public float timerTime;
        public float timerEnd;
        public float timerStart = 0;
        public int timesDone;
        // Start is called before the first frame update
        private void Update()
        {
            
            if (timerTime >= timerEnd)
            {
                timerTime = timerStart;
                timesDone++;
                dissolveTimer = dissolveMaterial.GetFloat("TimeTillDissolve");
                dissolveTimer++;
                if (timesDone >= 5)
                {
                    Destroy(this.gameObject);
                }
                
            }
            else
            {
                timerTime++;
            }
        }
    }
}
