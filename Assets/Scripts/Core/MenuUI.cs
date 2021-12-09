using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

namespace HorrorVR.Core
{
    public class MenuUI : MonoBehaviour
    {
        public static MenuUI current;

        [Header("Interactions")]
        [SerializeField] private InputActionAsset actionAsset;
        [SerializeField] private XRRayInteractor rayInteractor;
        
        [Header("UI")]
        [SerializeField] private Canvas menuCanvas;
        public Toggle continuousMovementToggle;
        public Toggle teleportMovementToggle;
        public Toggle continuousTurnToggle;
        public Toggle snapTurnToggle;
        public GameObject optionsPanel;
        public GameObject mainMenuPanel;

        private InputAction Menu_Press;

        public bool _menuIsOpen;

        private void Awake()
        {
            current = this;

            Time.timeScale = 1;
        }

        private void Start()
        {
            rayInteractor.enabled = _menuIsOpen;

            PlayerSettings.InitializeValues(true, false, true, false);

            continuousMovementToggle.isOn = PlayerSettings.continuousMovementEnabled;
            teleportMovementToggle.isOn = PlayerSettings.teleportMovementEnabled;
            continuousTurnToggle.isOn = PlayerSettings.continuousTurnEnabled;
            snapTurnToggle.isOn = PlayerSettings.snapTurnEnabled;

            MovementTypeManager.current.MovementCheck();
            
            Menu_Press = actionAsset.FindActionMap("XRI LeftHand").FindAction("Menu");
            Menu_Press.Enable();
            Menu_Press.performed += ToggleMenu;
        }

        #region Menu Toggle Functions
        private void ToggleMenu(InputAction.CallbackContext context)
        {
            ToggleMenu();
        }

        public void ToggleMenu()
        {
            _menuIsOpen = !_menuIsOpen;
            Time.timeScale = _menuIsOpen ? 0 : 1;
            
            rayInteractor.enabled = _menuIsOpen;
            
            menuCanvas.gameObject.SetActive(_menuIsOpen);
            OpenMainMenu();

            PlayerSettings.continuousMovementEnabled = continuousMovementToggle.isOn;
            PlayerSettings.teleportMovementEnabled = teleportMovementToggle.isOn;
            PlayerSettings.continuousTurnEnabled = continuousTurnToggle.isOn;
            PlayerSettings.snapTurnEnabled = snapTurnToggle.isOn;

            MovementTypeManager.current.MovementCheck();
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
        #endregion
        
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
