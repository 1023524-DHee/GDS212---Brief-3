using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace HorrorVR
{
    public class BobFootsteps : MonoBehaviour
    {
        public Vector2 volumeRange, pitchRange;
        public bool canPlay = true;

        private string[] footstepEvents = new string[]
        {
            "event:/Audio_Events/BOB/Footsteps/Monstersteps",
            "event:/Audio_Events/BOB/Footsteps/Monstersteps4",
            "event:/Audio_Events/BOB/Footsteps/Monstersteps5",
            "event:/Audio_Events/BOB/Footsteps/Monstersteps6"
        };

        public void Step ()
        {
            if (!canPlay) return;

            var instance = RuntimeManager.CreateInstance (footstepEvents.GetRandom ());
            instance.setVolume (Random.Range (volumeRange.x, volumeRange.y));
            instance.setPitch (Random.Range (pitchRange.x, pitchRange.y));
            RuntimeManager.AttachInstanceToGameObject (instance, transform);
            instance.start ();
            instance.release ();
        }
    }
}
