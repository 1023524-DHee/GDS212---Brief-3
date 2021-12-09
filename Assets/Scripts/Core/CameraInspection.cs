using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR.Core
{
    public class CameraInspection : MonoBehaviour
    {
        private List<IInspectable> Active, Inactive;

        private void Start ()
        {
            Active = new List<IInspectable> ();
            Inactive = new List<IInspectable> ();
            GetInspectablesInScene ();
        }

        // Should be called whenever a new scene is loaded
        private void GetInspectablesInScene ()
        {
            Active.Clear ();
            Inactive.Clear ();
            foreach (GameObject o in UnityEngine.SceneManagement.SceneManager.GetActiveScene ().GetRootGameObjects ())
            {
                foreach (IInspectable i in o.GetComponentsInChildren<IInspectable> ())
                    Inactive.Add (i);
            }
            print (Inactive.Count);
        }

        private void Update ()
        {
            int activeCount = Active.Count;
            for (int i = Inactive.Count - 1; i >= 0; i--)
            {
                if (InView (Inactive[i]))
                {
                    print ("In View");
                    Inactive[i].OnInspect ();

                    Active.Add (Inactive[i]);
                    Inactive.RemoveAt (i);
                }
            }

            for (int i = activeCount - 1; i >= 0; i--)
            {
                if (!InView (Active[i]))
                {
                    print ("Not In View");
                    Active[i].OnInspectEnd ();

                    if (!Active[i].DisableAfterNextUse)
                        Inactive.Add (Active[i]);
                    Active.RemoveAt (i);
                }
            }
        }

        private bool InView (IInspectable inspectable)
        {
            Vector3 displacement = inspectable.Transform.position - transform.position;
            return
                displacement.sqrMagnitude <= inspectable.DistanceThreshold * inspectable.DistanceThreshold // Within distance threshold
                &&
                Vector3.Angle (displacement, transform.forward) <= inspectable.LookAngleThreshold // Within look angle threshold
                &&
                Vector3.Angle (-displacement, inspectable.Transform.forward) <= inspectable.RelativeAngleThreshold; // Within relative angle threshold
        }
    }
}
