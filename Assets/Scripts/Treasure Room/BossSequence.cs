using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR
{
    public class BossSequence : MonoBehaviour
    {
        public bool BobInRange = false;
        public bool WithinAngleThresh => Vector3.Angle ((torch.position - player.position).ZeroY (), (bob.position - player.position).ZeroY ()) < angleThreshold;

        [SerializeField] private Transform bobPivot, bob, torch, player;
        [SerializeField] private Animator bobAnim;
        [SerializeField] private float angleThreshold;

        private float timeToStagger;
        private int bobHealth = 4, playerHealth = 3;

        private IEnumerator Sequence ()
        {
            while (bobHealth > 0 && playerHealth > 0)
            {
                SetRandomRotation ();
                bobAnim.SetFloat ("ApproachSpeed", 1);
                bobAnim.SetTrigger ("Approach");
                timeToStagger = 3;
                yield return new WaitUntil (() => BobInRange);

                float timeOut = 5;
                while (timeOut > 0 && timeToStagger > 0)
                {
                    if (WithinAngleThresh)
                    {
                        bobAnim.SetFloat ("ApproachSpeed", 0.5f);
                        timeToStagger = Mathf.Max (timeToStagger - Time.deltaTime, 0);
                    }
                    timeOut = Mathf.Max (timeOut - Time.deltaTime, 0);
                    yield return null;
                }

                if (timeToStagger <= 0)
                {
                    bobHealth -= 1;
                    bobAnim.SetTrigger ("Stagger");
                }
                else
                {
                    playerHealth -= 1;
                    bobAnim.SetTrigger ("Attack");
                }
            }
        }

        private void SetRandomRotation ()
        {
            Vector3 rotation = bobPivot.rotation.eulerAngles;
            rotation.y = Random.Range (0, 360);
            bobPivot.rotation = Quaternion.Euler (rotation);
        }
    }
}

public static class ExtensionsBro
{
    public static Vector3 ZeroY (this Vector3 vector) { vector.y = 0; return vector; }
}