using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace HorrorVR.Core
{
    public class MovementTypeManager : MonoBehaviour
    {
        public static MovementTypeManager current;
        
        [SerializeField]private CharacterController characterController;
        [SerializeField]private XRRig rig;
        
        [SerializeField] private ActionBasedContinuousMoveProvider continuousMoveProvider;
        [SerializeField] private ActionBasedSnapTurnProvider snapTurnProvider;
        [SerializeField] private ActionBasedContinuousTurnProvider continuousTurnProvider;
        [SerializeField] private TeleportationProvider teleportationProvider;
        [SerializeField] private TeleportationManager teleportationManager;

        private void Awake()
        {
            current = this;
        }

        private void Update()
        {
            CapsuleFollowHeadset();
        }

        public void MovementCheck()
        {
            if(PlayerSettings.continuousMovementEnabled) EnableContinuousMovement();
            if(PlayerSettings.teleportMovementEnabled) EnableTeleportationMovement();
            if(PlayerSettings.continuousTurnEnabled) EnableContinuousTurn();
            if(PlayerSettings.snapTurnEnabled) EnableSnapTurn();
        }
        
        private void CapsuleFollowHeadset()
        {
            Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);

            characterController.center = new Vector3(capsuleCenter.x, characterController.height / 2, capsuleCenter.z);
            characterController.height = rig.cameraInRigSpaceHeight + 0.2f;
        }
        
        public void EnableContinuousMovement()
        {
            continuousMoveProvider.enabled = true;
 
            teleportationProvider.enabled = false;
            teleportationManager.enabled = false;
        }
        
        public void EnableTeleportationMovement()
        {
            continuousMoveProvider.enabled = false;

            teleportationProvider.enabled = true;
            teleportationManager.enabled = true;
        }

        public void EnableContinuousTurn()
        {
            continuousTurnProvider.enabled = true;
            snapTurnProvider.enabled = false;
        }

        public void EnableSnapTurn()
        {
            continuousTurnProvider.enabled = false;
            snapTurnProvider.enabled = true;
        }
    }
}
