using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace HorrorVR.Forest
{
    public class SwordGrab : MonoBehaviour
    {
        [SerializeField] private XRSimpleInteractable interactable;
        [SerializeField] private Transform XRRig, RigFallParent;
        [SerializeField] private Animator floorAnim;

        //private bool g = false;

        private void Awake ()
        {
            interactable.activated.AddListener (OnInteract); 
        }

        //private void Update ()
        //{
        //    if (Time.time > 3 && !g)
        //    {
        //        ActivateEventArgs args = new ActivateEventArgs ();
        //        args.interactor = FindObjectOfType<XRBaseInteractor> ();
        //        OnInteract (args);
        //        g = true;
        //    }
        //}

        private void OnInteract (ActivateEventArgs args)
        {
            transform.parent = args.interactor.attachTransform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            GetComponent<Animator> ().SetBool ("Flashing", false);

            StartCoroutine (FallSequence ());
        }

        IEnumerator FallSequence ()
        {
            yield return new WaitForSeconds (1);
            XRRig.parent = RigFallParent;
            floorAnim.SetTrigger ("Shake");
            yield return new WaitForSeconds (2);
            floorAnim.SetTrigger ("Collapse");
        }
    }
}
