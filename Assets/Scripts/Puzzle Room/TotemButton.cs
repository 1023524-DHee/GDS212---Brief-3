using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HorrorVR
{
    public class TotemButton : MonoBehaviour
    {
        [SerializeField] private InputActionAsset actionAsset;
        private bool canInteract;
        public bool isSpinning;
        public GameObject totem;
        int spinAmount = 0;

        private void Start()
        {
            var L_Grip = actionAsset.FindActionMap("XRI LeftHand").FindAction("Grip");
            L_Grip.Enable();
            L_Grip.performed += IncreaseInt;

            var R_Grip = actionAsset.FindActionMap("XRI RightHand").FindAction("Grip");
            R_Grip.Enable();
            R_Grip.performed += IncreaseInt;


        }

        public void IncreaseInt(InputAction.CallbackContext context)
        {
            if (!canInteract) return;
            if (isSpinning) return;

            totem.GetComponent<ObjectSpin>().StartCoroutine(totem.GetComponent<ObjectSpin>().SpinTotem_Coroutine());
            spinAmount++;
            Debug.Log(spinAmount);

        }

        private void OnTriggerEnter(Collider other)
        {
            canInteract = true;
        }

        private void OnTriggerExit(Collider other)
        {
            canInteract = false;
        }
    }
}
