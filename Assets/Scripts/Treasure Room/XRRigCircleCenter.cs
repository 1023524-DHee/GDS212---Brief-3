using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR
{
    public class XRRigCircleCenter : MonoBehaviour
    {
        public Vector3 setPosition, setRotation;

        public void DoThing ()
        {
            transform.position = setPosition;
            transform.rotation = Quaternion.Euler (setRotation);
            Core.MovementTypeManager.current.DisableMovement ();
        }
    }
}
