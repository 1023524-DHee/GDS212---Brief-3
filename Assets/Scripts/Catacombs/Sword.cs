using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace HorrorVR.Catacombs
{
    public class Sword : MonoBehaviour
    {
        public static Sword Instance;

        public Transform midPoint;

        [SerializeField]
        private Queue<Vector3> trail = new Queue<Vector3>();

        private void Start()
        {
            Instance = this;
        }

        private void Update()
        {
            
        }
    }
}
