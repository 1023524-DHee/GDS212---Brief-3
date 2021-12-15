using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR
{
    public class DeathFadeout : MonoBehaviour
    {
        public void Fade () => StartCoroutine (FadeOut ());

        IEnumerator FadeOut ()
        {
            yield return new WaitForSeconds (0.5f);
            Animator anim = GetComponent<Animator> ();
            anim.SetFloat ("FadeSpeed", 0.15f);
            anim.SetTrigger ("FadeOut");
        }
    }
}
