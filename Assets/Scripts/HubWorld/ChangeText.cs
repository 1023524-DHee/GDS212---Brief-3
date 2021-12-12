using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HorrorVR.Core;
using UnityEngine.UI;

namespace HorrorVR.HubWorld
{
    public class ChangeText : MonoBehaviour
    {
        public TMP_Text textAsset;
        public Image imageComponent;
        
        public Sprite joystickImage;
        public Sprite triggerImage;

		private void Update()
		{
            if (PlayerSettings.continuousMovementEnabled)
            {
                imageComponent.sprite = joystickImage;
                textAsset.text = "To Move";
            }
            else if (PlayerSettings.teleportMovementEnabled)
            {
                imageComponent.sprite = triggerImage;
                textAsset.text = "Hold & Release \n To Teleport";
            }
		}
    }
}
