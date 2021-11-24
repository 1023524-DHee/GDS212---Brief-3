using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace HorrorVR.Core
{
    public class MovementTypeManager : MonoBehaviour
    {
        [SerializeField]private CharacterController characterController;
        [SerializeField]private XRRig rig;
        
        [SerializeField] private InputActionAsset actionAsset;
        [SerializeField] private ActionBasedContinuousMoveProvider continuousMoveProvider;
        [SerializeField] private ActionBasedContinuousTurnProvider continuousTurnProvider;
        [SerializeField] private ActionBasedSnapTurnProvider snapTurnProvider;
        [SerializeField] private TeleportationProvider teleportationProvider;
        [SerializeField] private TeleportationManager teleportationManager;

        private bool _isContinuous;
        
        // Start is called before the first frame update
        void Start()
        {
            var continuousMove = actionAsset.FindActionMap("XRI LeftHand").FindAction("Move");
            continuousMove.Enable();
            continuousMove.performed += EnableContinuousMovement;
            
            var L_teleportationMove = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Activate");
            L_teleportationMove.Enable();
            L_teleportationMove.performed += EnableTeleportationMovement;
            
            var R_teleportationMove = actionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Activate");
            R_teleportationMove.Enable();
            R_teleportationMove.performed += EnableTeleportationMovement;
        }

        private void Update()
        {
            CapsuleFollowHeadset();
        }

        private void CapsuleFollowHeadset()
        {
            Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);

            characterController.center = new Vector3(capsuleCenter.x, characterController.height / 2, capsuleCenter.z);
            characterController.height = rig.cameraInRigSpaceHeight + 0.2f;
        }
        
        private void EnableContinuousMovement(InputAction.CallbackContext context)
        {
            if (_isContinuous) return;
            
            continuousMoveProvider.enabled = true;
            
            continuousTurnProvider.enabled = true;
            snapTurnProvider.enabled = true;
            
            teleportationProvider.enabled = false;
            teleportationManager.enabled = false;
            
            _isContinuous = true;
        }
        
        private void EnableTeleportationMovement(InputAction.CallbackContext context)
        {
            if (!_isContinuous) return;
            
            continuousMoveProvider.enabled = false;
            
            continuousTurnProvider.enabled = false;
            snapTurnProvider.enabled = false;
            
            teleportationProvider.enabled = true;
            teleportationManager.enabled = true;
            
            _isContinuous = false;
        }
    }
}
