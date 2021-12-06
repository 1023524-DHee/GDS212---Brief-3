using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR.Core
{
    public interface IInspectable
    {
        bool DisableAfterNextUse { get; }
        float DistanceThreshold { get; }
        float LookAngleThreshold { get; }
        float RelativeAngleThreshold { get; }
        Transform Transform { get; }
        void OnInspect ();
        void OnInspectEnd ();
    }
}
