using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace HorrorVR.Core
{
    public class TeleportationManager : MonoBehaviour
    {
        private enum ControllerTracker
        {
            Lcontroller,
            Rcontroller
        }

        [SerializeField] private InputActionAsset actionAsset;
        [SerializeField] private XRRayInteractor lRayInteractor;
        [SerializeField] private XRRayInteractor rRayInteractor;
        [SerializeField] private TeleportationProvider teleportationProvider;
        [SerializeField] private Material blackoutMaterial;

        private InputAction L_Activate;
        private InputAction R_Activate;
        private InputAction L_Cancel;
        private InputAction R_Cancel;
        
        private ControllerTracker _currentController;
        private bool _isActive;
        private bool _isFading;

        // Start is called before the first frame update
        void Awake()
        {
            Color currentColor = blackoutMaterial.color;
            currentColor.a = 0;
            blackoutMaterial.color = currentColor;
            
            lRayInteractor.enabled = false;
            rRayInteractor.enabled = false;
        }

        private void CheckActiveController(InputAction.CallbackContext context)
        {
            if (context.action.actionMap.name == "XRI LeftHand")
            {
                _currentController = ControllerTracker.Lcontroller;
            }
            else if(context.action.actionMap.name == "XRI RightHand")
            {
                _currentController = ControllerTracker.Rcontroller;
            }
        }
        
        private void OnTeleportActivate(InputAction.CallbackContext context)
        {
            if (!PlayerSettings.teleportMovementEnabled) return;
            if (_isActive) return;
            if (_isFading) return;
            if (MenuUI.current._menuIsOpen) return;            
            
            CheckActiveController(context);

            if (_currentController == ControllerTracker.Lcontroller)
            {
                lRayInteractor.enabled = true;
            }
            else
            {
                rRayInteractor.enabled = true;
            }
            _isActive = true;
        }
        
        private void OnTeleportConfirm(InputAction.CallbackContext context)
        {
            if (!PlayerSettings.teleportMovementEnabled) return;
            if (MenuUI.current._menuIsOpen) return;
           
            RaycastHit hit = new RaycastHit();
            bool canTeleport = false;
            
            if (_currentController == ControllerTracker.Lcontroller)
            {
                lRayInteractor.TryGetCurrent3DRaycastHit(out hit);
            }
            else if(_currentController == ControllerTracker.Rcontroller)
            {
                rRayInteractor.TryGetCurrent3DRaycastHit(out hit);
            }

            if (hit.collider.CompareTag("Ground"))
            {
                canTeleport = true;
            }
            
            if (canTeleport)
            {
                StartCoroutine(Teleport_Coroutine(hit));
            }

            lRayInteractor.enabled = false;
            rRayInteractor.enabled = false;
            _isActive = false;
        }

        private void OnTeleportCancel(InputAction.CallbackContext context)
        {
            lRayInteractor.enabled = false;
            rRayInteractor.enabled = false;
            _isActive = false;
        }
        
        private IEnumerator Teleport_Coroutine(RaycastHit hit)
        {
            _isFading = true;
            
            float startTime = Time.time;
            Color currentColor = blackoutMaterial.color;
            while (Time.time < startTime + 0.5f)
            {
                currentColor.a = Mathf.Lerp(0, 1, (Time.time - startTime) / 0.5f);
                blackoutMaterial.color = currentColor;
                yield return null;
            }

            TeleportRequest request = new TeleportRequest()
            {
                destinationPosition = hit.point
            };
            teleportationProvider.QueueTeleportRequest(request);
            yield return new WaitForSeconds(0.5f);
            
            startTime = Time.time;
            while (Time.time < startTime + 0.5f)
            {
                currentColor.a = Mathf.Lerp(1, 0, (Time.time - startTime) / 0.5f);
                blackoutMaterial.color = currentColor;
                yield return null;
            }

            _isFading = false;
        }

        private void OnEnable()
        {
            L_Activate = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Activate");
            L_Activate.Enable();
            L_Activate.performed += OnTeleportActivate;
            L_Activate.canceled += OnTeleportConfirm;
            
            L_Cancel = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Cancel");
            L_Cancel.Enable();
            L_Cancel.performed += OnTeleportCancel;
            
            R_Activate = actionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Activate");
            R_Activate.Enable();
            R_Activate.performed += OnTeleportActivate;
            R_Activate.canceled += OnTeleportConfirm;
            
            R_Cancel = actionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Cancel");
            R_Cancel.Enable();
            R_Cancel.performed += OnTeleportCancel;
        }

        private void OnDisable()
        {
            L_Activate.performed -= OnTeleportActivate;
            L_Activate.canceled -= OnTeleportConfirm;

            L_Cancel.performed -= OnTeleportCancel;
            
            R_Activate.performed -= OnTeleportActivate;
            R_Activate.canceled -= OnTeleportConfirm;
            
            R_Cancel.performed -= OnTeleportCancel;
            
            L_Activate.Disable();
            R_Activate.Disable();
            L_Cancel.Disable();
            R_Cancel.Disable();
        }
    }
}
