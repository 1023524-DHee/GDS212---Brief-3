using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR.TreasureRoom
{
    public class TorchIntensify : MonoBehaviour
    {
        [SerializeField] private BobController bob;
        [SerializeField] private float speed;

        private Animator anim;
        private bool canCharge = true;

        private void Start ()
        {
            anim = GetComponent<Animator> ();
        }

        private float _intensity;
        private float intensity 
        { 
            set
            {
                _intensity = Mathf.Clamp01 (value);
                anim.SetFloat ("Intensity", _intensity);
            } 
            get { return _intensity; }
        }

        private void Update ()
        {
            print (bob.StaggerRatio);
            if (bob.State == BobState.Approaching)
            {
                if (!canCharge)
                {
                    canCharge = true;
                    anim.SetBool ("CanCharge", true);
                }
                intensity = bob.StaggerRatio;
            }
            else if (canCharge)
            {
                canCharge = false;
                anim.SetBool ("CanCharge", false);
                intensity = 0;
            }
        }
    }
}
