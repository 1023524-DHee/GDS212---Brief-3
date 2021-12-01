using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace HorrorVR.Core
{
    public class MenuUI : MonoBehaviour
    {
        public static MenuUI current;

        [SerializeField] private Canvas menuCanvas;
        [SerializeField] private InputActionAsset actionAsset;
        [SerializeField] private XRRayInteractor rayInteractor;

        public bool _menuIsOpen;

        private void Awake()
        {
            current = this;
        }

        private void Start()
        {
            rayInteractor.enabled = _menuIsOpen;
            
            var Menu_Press = actionAsset.FindActionMap("XRI LeftHand").FindAction("Menu");
            Menu_Press.Enable();
            Menu_Press.performed += ToggleMenu;
        }

        private void ToggleMenu(InputAction.CallbackContext context)
        {
            _menuIsOpen = !_menuIsOpen;
            rayInteractor.enabled = _menuIsOpen;
            menuCanvas.gameObject.SetActive(_menuIsOpen);
        }
    }
}
