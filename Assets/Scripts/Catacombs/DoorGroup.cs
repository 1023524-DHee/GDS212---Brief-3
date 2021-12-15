using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR
{
    public class DoorGroup : MonoBehaviour
    {
        public List<OpenDoorTorch> listOfDoors;

        public void CheckOpenDoor()
        {
            bool canOpen = true;
            foreach(OpenDoorTorch doors in listOfDoors)
            {
                if(!doors.doorGroupCheckIsOpen)
                {
                    canOpen = false;
                    break;
                }
            }

            if(canOpen)
            {
                foreach(OpenDoorTorch doors in listOfDoors)
                {
                    doors.OpenDoor();
                }
            }
        }
    }
}
