using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using Random = UnityEngine.Random;

namespace HorrorVR.Catacombs
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        private int maxHealth = 5;
        private int currentHealth;

        [Header("Components")]

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Transform playerPosition;

        [SerializeField]
        private Transform lookTarget;

        [SerializeField]
        private Transform targetPosition; 

        [Header("Attacks")]
        [SerializeField]
        private EventSequence[] attackPatterns;

        [SerializeField]
        private bool isAttacking = false;

        [SerializeField]
        private bool unpauseOnDeath = true;

        // Movement
        private Vector2 input;
        private Vector2 smoothedInput;
        private Vector2 previousInput;
        private Vector3 wantedPosition;
        private bool isMovingToTargetLocation = true;

        // Head looking
        private bool lookAtTarget = true;
        private Vector3 lookAtPosition;
        private float lookAtWeight = 0f;

        // Start is called before the first frame update
        void Start()
        {
            wantedPosition = targetPosition.position;//transform.position;

            lookAtPosition = lookTarget.position;

            currentHealth = maxHealth;
        }

        // Update is called once per frame
        void Update()
        {
            GetInput();
            Movement();

            // Look at the position we want to be in
            Vector3 lookPosition = lookTarget ? lookAtPosition : wantedPosition;
            Vector3 from = new Vector3(transform.position.x, 0f, transform.position.z);
            Vector3 to = new Vector3(lookPosition.x, 0f, lookPosition.z);
            transform.rotation = Quaternion.LookRotation(to - from);

            if (isMovingToTargetLocation)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Idle And Rotate Blend Tree") && !animator.IsInTransition(0))
                {
                    isMovingToTargetLocation = false;
                }
            }

            if (!isMovingToTargetLocation)
            {
                if (!isAttacking)
                {
                    StartCoroutine(AttackSequence(attackPatterns[Random.Range(0, attackPatterns.Length)]));
                }
            }
        }

        private IEnumerator AttackSequence(EventSequence attackPattern)
        {
            isAttacking = true;

            // Create the attack pattern
            EventSequence pattern = Instantiate(attackPattern, playerPosition.position, Quaternion.LookRotation(transform.position - playerPosition.position));//transform.position, transform.rotation);
            pattern.perpetrator = this;

            yield return new WaitForSeconds(attackPattern.timeUntilAttacks);

            if (attackPattern.isAttack)
            {
                // Play the attack animation
                animator.SetInteger("AttackValue", attackPattern.attack);
                animator.SetTrigger("AttackTrigger");

                // Wait until we return back into the idle state
                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Idle And Rotate Blend Tree") && !animator.IsInTransition(0));
            }

            // Wait until the pattern is finished, then destroy it
            yield return new WaitUntil(() => pattern.isFinished);
            Destroy(pattern.gameObject);

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
                animator.SetFloat("WalkStartAngle", Vector2.SignedAngle(Vector2.up, new Vector2(smoothedInput.x, smoothedInput.y)));// - transform.eulerAngles.y);
            }

            // Set some animation values
            animator.SetFloat("InputMagnitude", smoothedInput.magnitude);
            animator.SetFloat("X", smoothedInput.x);
            animator.SetFloat("Z", smoothedInput.y);

            // If we move from out of the walking dead zone (set by animator) to inside it, then we set the stop angle
            if (smoothedInput.magnitude < 0.2f && previousInput.magnitude >= 0.2f)
            {
                animator.SetFloat("WalkStopAngle", Vector2.SignedAngle(Vector2.up, new Vector2(smoothedInput.x, smoothedInput.y)));// - transform.eulerAngles.y);
            }

            if (lookAtTarget)
            {
                // Lerp look weight and position
                lookAtWeight = Mathf.Lerp(lookAtWeight, 1f, Time.deltaTime * 10f);
                lookAtPosition = Vector3.Lerp(lookAtPosition, lookTarget.position, Time.deltaTime * 10f);
            }
            else
            {
                // Lerp weight to zero
                lookAtWeight = Mathf.Lerp(lookAtWeight, 0f, Time.deltaTime * 10f);

                // If the weight is lower than the value, then we just set the look at position to the target directly
                // This is so we don't lerp the target from the previous position we looked from
                if (lookAtWeight <= 0.05f)
                {
                    lookAtPosition = lookTarget.position;
                }
            }

            /// Some animator values and what they do
            // HitFloat 0 = down, 1 = up, 2 = front
            // BlockFloat 0 = front?, 1 = harder or above?, 2 = even harder, 3 = above
        }

        private void OnAnimatorIK(int layerIndex)
        {
            // Look at target
            animator.SetLookAtPosition(lookTarget.position);
            animator.SetLookAtWeight(IsDead ? 0f : 1f);
        }

        public void SetAiming()
        {
            //TODO use this for the look direction, or a modified version

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
            if (Vector3.Distance(transform.position, wantedPosition) >= 0.5f)
            {
                Vector3 diff = wantedPosition - transform.position;

                // Cos and Sin of player rotation
                float s = Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad + Mathf.PI);
                float c = Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad + Mathf.PI);

                // Rotate the difference by the player rotation
                // We rotate it because the animator does not know the rotation of the object, so we have to do it ourselves
                diff.x = diff.x * c - diff.z * s;
                diff.z = diff.x * s + diff.z * c;

                input = -new Vector2(diff.x, diff.z);
                input = Mathf.Clamp(input.magnitude, 0f, 1f) * input.normalized;
            }
            else
            {
                input = Vector2.zero;
            }
        }

        //TODO move this to a seperate script to handle the damaging
        public bool IsDead
        {
            get
            {
                return currentHealth <= 0;
            }
        }

        public void Damage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                StopAllCoroutines();
                animator.SetFloat("DeathFloat", Random.Range(0, 5));
                animator.SetTrigger("Die");
                if (unpauseOnDeath) Pauser.current.Unpause();
            }
        }
    }
}
