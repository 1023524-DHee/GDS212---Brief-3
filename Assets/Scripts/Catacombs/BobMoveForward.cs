using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HorrorVR.Core;

namespace HorrorVR
{
    public class BobMoveForward : MonoBehaviour
    {
        [SerializeField] private Animator anim, fader;
        [SerializeField] private float moveSpeed;
        [SerializeField] private Vector3 moveDirection;

        private bool walkin;

        private void Start ()
        {
            //FMODUnity.RuntimeManager.PlayOneShotAttached ("event:/Audio_Events/BOB/Roar/BOB Roar 4", gameObject);
        }

        public void StartSequence ()
        {
            StartCoroutine (Sequence ());
        }

        IEnumerator Sequence ()
        {
            anim.SetTrigger ("Pose");

            yield return new WaitForSeconds (1);

            anim.SetBool ("Chase", true);

            walkin = true;
            while (walkin)
            {
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
                yield return null;
            }

            print ("You Died Biiiitch");
            anim.SetBool ("Chase", false);
            anim.SetTrigger ("Die");
            fader.SetTrigger ("FadeRed");

            yield return new WaitForSeconds (1.5f);
            Core.MovementTypeManager.current.ReloadLevel ();
            //FMODUnity.RuntimeManager.PlayOneShotAttached ("event:/Audio_Events/BOB/Breath/BOB Breathing 2", gameObject);
        }

        private void OnTriggerEnter (Collider other)
        {
            if (other.CompareTag ("Player"))
            {
                walkin = false;
            }
        }
    }
}
