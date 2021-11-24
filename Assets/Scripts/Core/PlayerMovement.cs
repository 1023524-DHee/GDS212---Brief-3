using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace HorrorVR.Core
{
    public class PlayerMovement : MonoBehaviour
    {
        public XRNode inputSource;

        public LayerMask groundLayer;
        
        public float movementSpeed;
        public float gravity = -9.81f;

        public float additionalHeight = 0.2f;
        
        private XRRig _rig;
        private Vector2 _inputAxis;
        private CharacterController _characterController;

        private float _fallingSpeed;
        private bool _isGrounded; 
        
        // Start is called before the first frame update
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _rig = GetComponent<XRRig>();
        }

        // Update is called once per frame
        private void Update()
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
            device.TryGetFeatureValue(CommonUsages.primary2DAxis, out _inputAxis);

            IsGrounded();
        }

        private void FixedUpdate()
        {
            CapsuleFollowHeadset();
            Movement();
            Gravity();
        }

        private void Gravity()
        {
            if (_isGrounded) _fallingSpeed = 0f;
            else _fallingSpeed += gravity * Time.fixedDeltaTime;

            _characterController.Move((Vector3.up) * _fallingSpeed * Time.fixedDeltaTime);
        }

        private void IsGrounded()
        {
            Vector3 rayStart = transform.TransformPoint(_characterController.center);
            float rayLength = _characterController.center.y + 0.01f;

            if (Physics.SphereCast(rayStart, _characterController.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer))
            {
                _isGrounded = true;
            }
            else
            {
                _isGrounded = false;
            }
        }

        private void CapsuleFollowHeadset()
        {
            Vector3 capsuleCenter = transform.InverseTransformPoint(_rig.cameraGameObject.transform.position);

            _characterController.center = new Vector3(capsuleCenter.x, _characterController.height / 2, capsuleCenter.z);
            _characterController.height = _rig.cameraInRigSpaceHeight + additionalHeight;
        }
        
        private void Movement()
        {
            Quaternion headYaw = Quaternion.Euler(0, _rig.cameraGameObject.transform.eulerAngles.y, 0);
            Vector3 direction = headYaw * new Vector3(_inputAxis.x, 0, _inputAxis.y);

            _characterController.Move(direction * movementSpeed * Time.fixedDeltaTime);
        }
    }
}

