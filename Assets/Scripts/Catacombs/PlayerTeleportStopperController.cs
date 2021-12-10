using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace HorrorVR.Core
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerTeleportStopperController : MonoBehaviour
    {
        private CharacterController controller;

        private Vector3 previousPosition;

        private void Start()
        {
            controller = GetComponent<CharacterController>();

            previousPosition = controller.transform.position;
        }

        private void OnEnable()
        {
            MovementTypeManager.current.TeleportationProvider.endLocomotion += CheckTeleportation;
        }

        private void OnDisable()
        {
            MovementTypeManager.current.TeleportationProvider.endLocomotion -= CheckTeleportation;
        }

        //// Update is called once per frame
        //void Update()
        //{
        //    if (controller.transform.position != previousPosition)
        //    {

        //    }
        //}

        private void CheckTeleportation(LocomotionSystem system)
        {
            // check collision
            Vector3 point1 = controller.center + Vector3.up * ((controller.height / 2f) - controller.radius);
            Vector3 point2 = controller.center - Vector3.up * ((controller.height / 2f) - controller.radius);
            RaycastHit[] inTheWay = Physics.CapsuleCastAll(point1, point2, controller.radius, controller.transform.position - previousPosition, Vector3.Distance(controller.transform.position, previousPosition), ~0, QueryTriggerInteraction.Collide);

            inTheWay = inTheWay.OrderBy(h => h.distance).ToArray();

            foreach (var hit in inTheWay)
            {
                if (hit.collider.gameObject.GetComponent<PlayerTeleportStopper>())
                {
                    Vector3 from = controller.transform.position;
                    Vector3 to = hit.collider.GetComponent<PlayerTeleportStopper>().StopPosition.position;

                    controller.Move(to - from);
                }
            }

            previousPosition = controller.transform.position;
        }
    }
}
