using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HorrorVR.Core;

namespace HorrorVR.HubWorld
{
    public class ChangeText : MonoBehaviour
    {
        public TMP_Text textAsset;

		private void Update()
		{
            if (PlayerSettings.continuousMovementEnabled)
            {
                textAsset.text = "Use\n Left Joystick\n To Move";
            }
            else if (PlayerSettings.teleportMovementEnabled)
            {
                textAsset.text = "Hold &\n Release Trigger\n To Teleport";
            }
		}
    }
}
