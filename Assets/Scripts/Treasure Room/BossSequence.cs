using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR.TreasureRoom
{
    public class BossSequence : MonoBehaviour
    {
        private bool WithinAngleThresh => Vector3.Angle ((torch.position - player.position).ZeroY (), (bob.position - player.position).ZeroY ()) < angleThreshold;

        [SerializeField] private Transform bobPivot, bob, torch, player;
        [SerializeField] private Animator bobAnim;
        [SerializeField] private float angleThreshold;

        private BobController bobController;
        private int playerHealth = 3;
        private bool reachedPlayer = false;

        //private void Start ()
        //{
        //    bobController = GetComponent<BobController> ();
        //    StartCoroutine (Sequence ());
        //}

        //private IEnumerator Sequence ()
        //{
        //    //while (bobController.Health > 0 && playerHealth > 0)
        //    //{
        //    //    yield return new WaitUntil (() => bobController.State == BobState.Idle);
        //    //    bobController.Approach ();
        //    //    yield return new WaitUntil (() => bob);
        //    }
        //}
    }
}
