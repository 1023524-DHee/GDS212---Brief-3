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

        private ControllerTracker _currentController;
        private bool _isActive;
        private bool _isFading;
        
        // Start is called before the first frame update
        void Start()
        {
            Color currentColor = blackoutMaterial.color;
            currentColor.a = 0;
            blackoutMaterial.color = currentColor;
            
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
            if (_isFading) return;
            
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
        
        private void OnTeleportCancel(InputAction.CallbackContext context)
        {
            lRayInteractor.enabled = false;
            rRayInteractor.enabled = false;
            _isActive = false;
        }
    }
}
