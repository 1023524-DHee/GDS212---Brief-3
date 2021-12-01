using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace HorrorVR.Catacombs
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Transform target;

        private Vector2 input;
        private Vector2 smoothedInput;
        private Vector2 previousInput;

        private bool lookAtTarget = true;
        private Vector3 lookAtPosition;
        private float lookAtWeight = 0f;

        private Vector3 wantedPosition;

        [Header("Attacks")]
        [SerializeField]
        private Attack[] attackPatterns;

        [SerializeField]
        private bool isAttacking = false;

        [Serializable]
        private struct Attack
        {
            public int attack;
        }

        // Start is called before the first frame update
        void Start()
        {
            wantedPosition = transform.position;

            lookAtPosition = target.position;

            StartCoroutine(AttackSequence(attackPatterns[0]));
        }

        // Update is called once per frame
        void Update()
        {
            GetInput();
            Movement();
        }

        private IEnumerator AttackSequence(Attack attackPattern)
        {
            isAttacking = true;

            animator.SetInteger("AttackValue", attackPattern.attack);
            animator.SetTrigger("AttackTrigger");

            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Standing"));

            isAttacking = false;
        }

        private void Movement()
        {
            // Smooth input
            previousInput = smoothedInput;
            smoothedInput = Vector2.Lerp(smoothedInput, input, Time.deltaTime * 10f);

            // If we move from inside of the walking dead zone (set by animator) to outside of it, then we set the start angle
            if (smoothedInput.magnitude >= 0.2f && previousInput.magnitude < 0.2f)
            {
                animator.SetFloat("WalkStartAngle", Vector2.SignedAngle(Vector2.up, new Vector2(-smoothedInput.x, smoothedInput.y)));// - transform.eulerAngles.y);
            }

            // Set some animation values
            animator.SetFloat("InputMagnitude", smoothedInput.magnitude);
            animator.SetFloat("X", smoothedInput.x);
            animator.SetFloat("Z", smoothedInput.y);

            // If we move from out of the walking dead zone (set by animator) to inside it, then we set the stop angle
            if (smoothedInput.magnitude < 0.2f && previousInput.magnitude >= 0.2f)
            {
                animator.SetFloat("WalkStopAngle", Vector2.SignedAngle(Vector2.up, new Vector2(-smoothedInput.x, smoothedInput.y)));// - transform.eulerAngles.y);
            }

            if (lookAtTarget)
            {
                // Lerp look weight and position
                lookAtWeight = Mathf.Lerp(lookAtWeight, 1f, Time.deltaTime * 10f);
                lookAtPosition = Vector3.Lerp(lookAtPosition, target.position, Time.deltaTime * 10f);
            }
            else
            {
                // Lerp weight to zero
                lookAtWeight = Mathf.Lerp(lookAtWeight, 0f, Time.deltaTime * 10f);

                // If the weight is lower than the value, then we just set the look at position to the target directly
                // This is so we don't lerp the target from the previous position we looked from
                if (lookAtWeight <= 0.05f)
                {
                    lookAtPosition = target.position;
                }
            }

            /// Some animator values and what they do
            // HitFloat 0 = down, 1 = up, 2 = front
            // BlockFloat 0 = front?, 1 = harder or above?, 2 = even harder, 3 = above
        }

        private void OnAnimatorIK(int layerIndex)
        {
            animator.SetLookAtPosition(target.position);
            animator.SetLookAtWeight(1f);
        }

        public void SetAiming()
        {
            //if (!lookAtTarget)
            //{
            //    // Lerp the aim angles
            //    horAimAngle = Mathf.Lerp(horAimAngle, 0f, Time.deltaTime * 10f);
            //    verAimAngle = Mathf.Lerp(verAimAngle, 0f, Time.deltaTime * 10f);

            //    // Set the angles in the animator
            //    animator.SetFloat("HorAimAngle", horAimAngle);
            //    animator.SetFloat("VerAimAngle", verAimAngle);

            //    return;
            //}

            //// Get the look rotation to the target
            //Vector3 delta = target.position - transform.position + PlayerOffset;
            //Quaternion look = Quaternion.LookRotation(delta);

            //// Get the difference in the y rotation, this will allow us to tell the animator that this is how much we want to move
            //var deltaAngle = Mathf.DeltaAngle(transform.rotation.eulerAngles.y + 5f, look.eulerAngles.y);

            //// The new horizontal aim angle is equal to this difference
            //float newHorAimAngle = deltaAngle;
            //// The new vertical angle is just the look angle
            //float newVerAimAngle = -(look.eulerAngles.x > 180f ? look.eulerAngles.x - 360f : look.eulerAngles.x);// direction.x;

            //// Lerp the aim angles
            //horAimAngle = Mathf.Lerp(horAimAngle, newHorAimAngle, Time.deltaTime * 10f);
            //verAimAngle = Mathf.Lerp(verAimAngle, newVerAimAngle, Time.deltaTime * 10f);

            //// Set the angles in the animator
            //animator.SetFloat("HorAimAngle", horAimAngle);
            //animator.SetFloat("VerAimAngle", verAimAngle);
        }

        public void GetInput()
        {
            if (Vector3.Distance(transform.position, wantedPosition) >= 1f)
            {
                Vector3 diff = wantedPosition - transform.position;
                input = -new Vector2(diff.x, diff.z);
                input = Mathf.Clamp(input.magnitude, 0f, 1f) * input.normalized;
            }
            else
            {
                input = Vector2.zero;
            }
        }
    }
}
