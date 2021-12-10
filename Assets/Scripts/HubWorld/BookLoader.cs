using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HorrorVR.HubWorld
{
    public class BookLoader : MonoBehaviour
    {
        public ParticleSystem particleSystem;
        public void Loadlevel()
        {
            particleSystem.Play();
        }
    }
}
