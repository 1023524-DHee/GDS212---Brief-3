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

        [SerializeField] private Canvas menuCanvas;
        [SerializeField] private InputActionAsset actionAsset;
        [SerializeField] private XRRayInteractor rayInteractor;

        private InputAction Menu_Press;

        public Toggle continuousMovementToggle;
        public Toggle teleportMovementToggle;
        public Toggle continuousTurnToggle;
        public Toggle snapTurnToggle;

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

            continuousMovementEnabled = PlayerSettings.continuousMovementEnabled;
            teleportEnabled = PlayerSettings.teleportMovementEnabled;
            continuousTurnEnabled = PlayerSettings.continuousTurnEnabled;
            snapTurnEnabled = PlayerSettings.snapTurnEnabled;

            continuousMovementToggle.isOn = continuousMovementEnabled;
            teleportMovementToggle.isOn = teleportEnabled;
            continuousTurnToggle.isOn = continuousTurnEnabled;
            snapTurnToggle.isOn = snapTurnEnabled;

            Menu_Press = actionAsset.FindActionMap("XRI LeftHand").FindAction("Menu");
            Menu_Press.Enable();
            Menu_Press.performed += ToggleMenu;
        }

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
            optionsPanel.gameObject.SetActive(false);
            mainMenuPanel.gameObject.SetActive(true);

            PlayerSettings.continuousMovementEnabled = continuousMovementEnabled;
            PlayerSettings.teleportMovementEnabled = teleportEnabled;
            PlayerSettings.continuousTurnEnabled = continuousTurnEnabled;
            PlayerSettings.snapTurnEnabled = snapTurnEnabled;

            MovementTypeManager.current.MovementCheck();
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
