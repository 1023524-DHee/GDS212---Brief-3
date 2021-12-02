using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

namespace HorrorVR.Core
{
    public class MenuUI : MonoBehaviour
    {
        public static MenuUI current;

        [SerializeField] private Canvas menuCanvas;
        [SerializeField] private InputActionAsset actionAsset;
        [SerializeField] private XRRayInteractor rayInteractor;

        private InputAction Menu_Press;
        
        public GameObject optionsPanel;
        public GameObject mainMenuPanel;
        public bool _menuIsOpen;

        // OPTIONS
        public bool continuousMovementEnabled { get; set; }
        public bool snapTurnEnabled { get; set; }
        public bool continuousTurnEnabled { get; set; }
        public bool teleportEnabled { get; set; }
        
        private void Awake()
        {
            current = this;
        }

        private void Start()
        {
            rayInteractor.enabled = _menuIsOpen;

            continuousMovementEnabled = false;
            snapTurnEnabled = true;
            continuousTurnEnabled = false;
            teleportEnabled = true;


            Menu_Press = actionAsset.FindActionMap("XRI LeftHand").FindAction("Menu");
            Menu_Press.Enable();
            Menu_Press.performed += ToggleMenu;
        }

        private void ToggleMenu(InputAction.CallbackContext context)
        {
            _menuIsOpen = !_menuIsOpen;
            
            rayInteractor.enabled = _menuIsOpen;
            
            menuCanvas.gameObject.SetActive(_menuIsOpen);
            optionsPanel.gameObject.SetActive(false);
            mainMenuPanel.gameObject.SetActive(true);
            
            if(continuousMovementEnabled) MovementTypeManager.current.EnableContinuousMovement();
            if(teleportEnabled) MovementTypeManager.current.EnableTeleportationMovement();
            if(continuousTurnEnabled) MovementTypeManager.current.EnableContinuousTurn();
            if(snapTurnEnabled) MovementTypeManager.current.EnableSnapTurn();
            
            Debug.Log(continuousMovementEnabled);
            Debug.Log(teleportEnabled);
            Debug.Log(continuousTurnEnabled);
            Debug.Log(snapTurnEnabled);
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void OpenOptionsMenu()
        {
            optionsPanel.gameObject.SetActive(true);
            mainMenuPanel.gameObject.SetActive(false);
        }

        public void OpenMainMenu()
        {
            optionsPanel.gameObject.SetActive(false);
            mainMenuPanel.gameObject.SetActive(true);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private void OnDisable()
        {
            Menu_Press.performed -= ToggleMenu;
            Menu_Press.Disable();
        }
    }
}
