using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RyansFrigginEbicEggstensionMethods
{
    public static Vector3 ZeroY (this Vector3 vector) { vector.y = 0; return vector; }

    public static Vector3 SetX (this Vector3 vector, float value) { vector.x = value; return vector; }
    public static Vector3 SetY (this Vector3 vector, float value) { vector.y = value; return vector; }
    public static Vector3 SetZ (this Vector3 vector, float value) { vector.z = value; return vector; }
}