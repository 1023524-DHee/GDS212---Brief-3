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

        private ControllerTracker _currentController;
        private bool _isActive;
        
        // Start is called before the first frame update
        void Start()
        {
            lRayInteractor.enabled = false;
            rRayInteractor.enabled = false;
            
            var L_activate = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Activate");
            L_activate.Enable();
            L_activate.performed += OnTeleportActivate;
            L_activate.canceled += OnTeleportConfirm;
            
            var L_Cancel = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Cancel");
            L_Cancel.Enable();
            L_Cancel.performed += OnTeleportCancel;
            
            var R_activate = actionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Activate");
            R_activate.Enable();
            R_activate.performed += OnTeleportActivate;
            R_activate.canceled += OnTeleportConfirm;
            
            var R_cancel = actionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Cancel");
            R_cancel.Enable();
            R_cancel.performed += OnTeleportCancel;
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
            if (_isActive) return;
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
            RaycastHit hit;
            bool canTeleport = false;
            
            if (_currentController == ControllerTracker.Lcontroller)
            {
                lRayInteractor.TryGetCurrent3DRaycastHit(out hit);
                if (hit.collider.CompareTag("Ground"))
                {
                    canTeleport = true;
                }

            }
            else
            {
                rRayInteractor.TryGetCurrent3DRaycastHit(out hit);
                if (hit.collider.CompareTag("Ground"))
                {
                    canTeleport = true;
                }
            }

            if (canTeleport)
            {
                TeleportRequest request = new TeleportRequest()
                {
                    destinationPosition = hit.point
                };
                teleportationProvider.QueueTeleportRequest(request);
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
    }
}
