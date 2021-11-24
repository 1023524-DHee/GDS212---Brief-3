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

        private Vector2 input;
        private Vector2 smoothedInput;
        private Vector2 previousInput;



        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            // Smooth input
            previousInput = smoothedInput;
            smoothedInput = Vector2.Lerp(smoothedInput, input, Time.deltaTime * 10f);

            // If we move from inside of the walking dead zone (set by animator) to outside of it, then we set the start angle
            if (smoothedInput.magnitude >= 0.2f && previousInput.magnitude < 0.2f)
            {
                animator.SetFloat("WalkStartAngle", Vector2.SignedAngle(Vector2.up, new Vector2(-smoothedInput.x, smoothedInput.y)) - transform.eulerAngles.y);
            }

            // Set some animation values
            animator.SetFloat("InputMagnitude", smoothedInput.magnitude);
            animator.SetFloat("X", smoothedInput.x);
            animator.SetFloat("Z", smoothedInput.y);

            // If we move from out of the walking dead zone (set by animator) to inside it, then we set the stop angle
            if (smoothedInput.magnitude < 0.2f && previousInput.magnitude >= 0.2f)
            {
                animator.SetFloat("WalkStopAngle", Vector2.SignedAngle(Vector2.up, new Vector2(-smoothedInput.x, smoothedInput.y)) - transform.eulerAngles.y);
            }






            /// Some animator values and what they do
            // HitFloat 0 = down, 1 = up, 2 = front
            // BlockFloat 0 = front?, 1 = harder or above?, 2 = even harder, 3 = above
        }

        public void SetMovement(CallbackContext context)
        {
            input = context.ReadValue<Vector2>();
            input = Mathf.Clamp(input.magnitude, 0f, 1f) * input.normalized;
        }
    }
}
