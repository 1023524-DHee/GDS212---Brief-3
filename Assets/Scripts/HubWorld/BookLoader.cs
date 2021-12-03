using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HorrorVR.HubWorld
{
    public class BookLoader : MonoBehaviour
    {
        public string levelToLoad;
        public Material blackoutMaterial;
        public ParticleSystem particleSystem;
        
        private bool _isFading;
        
        public void Loadlevel()
        {
            if (_isFading) return;

            StartCoroutine(LoadLevel_Coroutine());
        }

        private IEnumerator LoadLevel_Coroutine()
        {
            _isFading = true;
            
            float startTime = Time.time;
            Color currentColor = blackoutMaterial.color;
            particleSystem.Play();
            while (Time.time < startTime + 5f)
            {
                currentColor.a = Mathf.Lerp(0, 1, (Time.time - startTime) / 5f);
                blackoutMaterial.color = currentColor;
                yield return null;
            }
            SceneManager.LoadScene(levelToLoad);
            _isFading = false;
        }
}
}
