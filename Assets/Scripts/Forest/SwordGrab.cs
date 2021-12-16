using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using HorrorVR.Core;

namespace HorrorVR.Forest
{
    public class SwordGrab : MonoBehaviour
    {
        //[SerializeField] private XRSimpleInteractable interactable;
        [SerializeField] private Transform XRRig, RigFallParent;
        [SerializeField] private Animator floorAnim;
        [SerializeField] private GameObject ground, floor;

        //private bool g = false;

        private void Awake ()
        {
            //interactable.activated.AddListener (OnInteract); 
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

        public void OnInteract ()
        {
            //transform.parent = args.interactor.attachTransform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            //GetComponent<Animator> ().SetBool ("Flashing", false);
            XRRig.GetComponent<CharacterController> ().enabled = false;

            StartCoroutine (FallSequence ());
        }

        IEnumerator FallSequence ()
        {
            MovementTypeManager.current.Loadlevel("Scene_Catacombs");
            yield return new WaitForSeconds (0.5f);
            XRRig.parent = RigFallParent;
            floorAnim.SetTrigger ("Shake");
            yield return new WaitForSeconds (0.5f);
            floorAnim.SetTrigger ("Collapse");
            ground.GetComponent<MeshCollider>().enabled = false;
            floor.GetComponent<BoxCollider>().enabled = false;  
        }
    }
}
