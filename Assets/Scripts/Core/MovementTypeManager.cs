using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

namespace HorrorVR.Core
{
    public class MovementTypeManager : MonoBehaviour
    {
        public static MovementTypeManager current;
        
        [Header("XR Rig")]
        [SerializeField]private CharacterController characterController;
        [SerializeField]private XRRig rig;
        
        [Header("Movement Providers")]
        [SerializeField] private ActionBasedContinuousMoveProvider continuousMoveProvider;
        [SerializeField] private ActionBasedSnapTurnProvider snapTurnProvider;
        [SerializeField] private ActionBasedContinuousTurnProvider continuousTurnProvider;
        [SerializeField] private TeleportationProvider teleportationProvider;
        [SerializeField] private TeleportationManager teleportationManager;
        
        [Header("Level Transition")]
        public Material blackoutMaterial;

        private bool _isFading;

        private void Awake()
        {
            current = this;

            StartCoroutine(LoadIntoLevel_Coroutine());
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
        
        #region Movement Type Togglers
        public void MovementCheck()
        {
            if(PlayerSettings.continuousMovementEnabled) EnableContinuousMovement();
            if(PlayerSettings.teleportMovementEnabled) EnableTeleportationMovement();
            if(PlayerSettings.continuousTurnEnabled) EnableContinuousTurn();
            if(PlayerSettings.snapTurnEnabled) EnableSnapTurn();
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

        public void DisableMovement()
        {
            continuousMoveProvider.enabled = false;

            teleportationProvider.enabled = false;
            teleportationManager.enabled = false;

            snapTurnProvider.enabled = false;
            continuousTurnProvider.enabled = false;
        }
        
        public void EnableMovement()
        {
            MovementCheck();
        }
        #endregion
        


        #region Level Loaders
        public void Loadlevel(string levelToLoad)
        {
            if (_isFading) return;

            if (levelToLoad == "Scene_HubWorld")
            {
                SceneManager.LoadScene(levelToLoad);
            }


            if (SceneManager.GetActiveScene().name != levelToLoad)
            {
                StartCoroutine(LoadLevel_Coroutine(levelToLoad));
            }
        }
        
        private IEnumerator LoadLevel_Coroutine(string levelToLoad)
        {
            _isFading = true;
            Time.timeScale = 1;
            
            float startTime = Time.time;
            Color currentColor = blackoutMaterial.color;
            while (Time.time < startTime + 2f)
            {
                currentColor.a = Mathf.Lerp(0, 1, (Time.time - startTime) / 2f);
                blackoutMaterial.color = currentColor;
                yield return null;
            }
            SceneManager.LoadScene(levelToLoad);
            _isFading = false;
        }

        private IEnumerator LoadIntoLevel_Coroutine()
        {
            _isFading = true;
            
            float startTime = Time.time;
            Color currentColor = blackoutMaterial.color;
            while (Time.time < startTime + 1f)
            {
                currentColor.a = Mathf.Lerp(1, 0, (Time.time - startTime) / 1f);
                blackoutMaterial.color = currentColor;
                yield return null;
            }

            _isFading = false;
        }
        #endregion
    }
}
