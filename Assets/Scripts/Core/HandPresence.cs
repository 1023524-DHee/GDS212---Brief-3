using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

namespace HorrorVR.Core
{
    public class HandPresence : MonoBehaviour
    {
        [SerializeField] private InputActionAsset actionAsset;

        public GameObject LSpawnedHandModel;
        public GameObject RSpawnedHandModel;
        public string gripAnimBool;
        public string triggerAnimBool;

        private Animator _LHandAnimator;
        private Animator _RHandAnimator;

        // Start is called before the first frame update
        void Start()
        {
            var L_Grip = actionAsset.FindActionMap("XRI LeftHand").FindAction("Grip");
            L_Grip.Enable();
            L_Grip.performed += LGripAnimation;
            L_Grip.canceled += LUnGripAnimation;
            
            var R_Grip = actionAsset.FindActionMap("XRI RightHand").FindAction("Grip");
            R_Grip.Enable();
            R_Grip.performed += RGripAnimation;
            R_Grip.canceled += RUnGripAnimation;
            
            var L_Trigger = actionAsset.FindActionMap("XRI LeftHand").FindAction("Trigger");
            L_Trigger.Enable();
            L_Trigger.performed += LPinchAnimation;
            L_Trigger.canceled += LUnPinchAnimation;
            
            var R_Trigger = actionAsset.FindActionMap("XRI RightHand").FindAction("Trigger");
            R_Trigger.Enable();
            R_Trigger.performed += RPinchAnimation;
            R_Trigger.canceled += RUnPinchAnimation;
            
            _LHandAnimator = LSpawnedHandModel.GetComponent<Animator>();
            _RHandAnimator = RSpawnedHandModel.GetComponent<Animator>();
        }

        private void RGripAnimation(InputAction.CallbackContext context)
        {
            _RHandAnimator.SetBool(gripAnimBool, true);
        }
        
        private void RUnGripAnimation(InputAction.CallbackContext context)
        {
            _RHandAnimator.SetBool(gripAnimBool, false);
        }
        
        private void RPinchAnimation(InputAction.CallbackContext context)
        {
            _RHandAnimator.SetBool(triggerAnimBool, true);
        }
        
        private void RUnPinchAnimation(InputAction.CallbackContext context)
        {
            _RHandAnimator.SetBool(triggerAnimBool, false);
        }
        
        private void LGripAnimation(InputAction.CallbackContext context)
        {
            _LHandAnimator.SetBool(gripAnimBool, true);
        }
        
        private void LUnGripAnimation(InputAction.CallbackContext context)
        {
            _LHandAnimator.SetBool(gripAnimBool, false);
        }
        
        private void LPinchAnimation(InputAction.CallbackContext context)
        {
            _LHandAnimator.SetBool(triggerAnimBool, true);
        }
        
        private void LUnPinchAnimation(InputAction.CallbackContext context)
        {
            _LHandAnimator.SetBool(triggerAnimBool, false);
        }
    }
}
