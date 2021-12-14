using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace HorrorVR.Core
{
    public class JumpScare: MonoBehaviour
    {
        public TrackedPoseDriver trackedPoseDriver;
        public MovementTypeManager movementTypeManager;
        
        public void TriggerJumpScare()
        {
            trackedPoseDriver.trackingType = TrackedPoseDriver.TrackingType.PositionOnly;
            
            //TODO: IMPLEMENT BOB ANIMATION
        }

        // Put at the end of animtion
        public void FinishJumpScare()
        {
            trackedPoseDriver.trackingType = TrackedPoseDriver.TrackingType.RotationAndPosition;
            movementTypeManager.ReloadLevel();
        }
    }
}
