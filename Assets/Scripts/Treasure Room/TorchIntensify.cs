using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR.TreasureRoom
{
    public class TorchIntensify : MonoBehaviour
    {
        [SerializeField] private BossSequence bossSequence;
        [SerializeField] private float speed;
        [SerializeField] private float maxIntensity;
        [SerializeField] private Light torchLight;
        [SerializeField] private ParticleSystem torchParticles;

        private float startLightIntensity;

        private void Start ()
        {
            startLightIntensity = torchLight.intensity;
        }

        private float intensity 
        { 
            set
            {
                intensity = Mathf.Clamp (value, 1, maxIntensity);
                torchLight.intensity = startLightIntensity * intensity;
                ParticleSystem.MainModule psMain = torchParticles.main;
                psMain.startSize = 0.08f * intensity;
            } 
            get { return intensity; }
        }

        private void Update ()
        {
            //intensity += (bossSequence.BobInRange && bossSequence.WithinAngleThresh ? 1 : -1) * speed * Time.deltaTime;
        }
    }
}
