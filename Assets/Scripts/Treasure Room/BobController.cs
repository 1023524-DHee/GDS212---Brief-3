using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorVR.TreasureRoom
{
    public class BobController : MonoBehaviour
    {
        [SerializeField] private Transform bob, torch, player;
        [SerializeField] private Animator bobAnim;
        [SerializeField] private Vector2 minMaxSpeed;
        [SerializeField] private float furthestDistance, inRangeDistance, atPlayerDistance, angleThreshold;

        public BobState State => state;
        public float MoveSpeedRatio => Mathf.InverseLerp (minMaxSpeed.x, minMaxSpeed.y, moveSpeed);
        public float StaggerRatio => 1 - timeToStagger / 3;
        
        public bool InRange => -bob.localPosition.z <= inRangeDistance;
        public bool WithinAngleThresh => Vector3.Angle ((torch.position - player.position).ZeroY (), (bob.position - player.position).ZeroY ()) < angleThreshold;

        private BobState state;
        private float _moveSpeed;
        private float moveSpeed {
            get => _moveSpeed;
            set
            {
                _moveSpeed = Mathf.Clamp (value, minMaxSpeed.x, minMaxSpeed.y);
                //bobAnim.SetFloat ("ApproachSpeed", moveSpeed / minMaxSpeed.y);
            }
        }
        private float timeToStagger, idleWaitTime;
        private int health = 4;

        private BossSequence bossSequence;

        private void Start ()
        {
            bossSequence = GetComponent<BossSequence> ();
            Idle ();
        }

        private void Update ()
        {
            //print (timeToStagger);
            switch (state)
            {
                case BobState.Idle:
                    idleWaitTime -= Time.deltaTime;
                    if (idleWaitTime <= 0)
                        Approach ();
                    break;

                case BobState.Approaching:
                    moveSpeed += InRange && WithinAngleThresh ? -Time.deltaTime : 2 * Time.deltaTime;
                    Move (moveSpeed);
                    
                    if (InRange)
                    {
                        if (WithinAngleThresh)
                        {
                            timeToStagger -= Time.deltaTime;
                            if (timeToStagger <= 0)
                            {
                                //bobAnim.SetTrigger ("Stagger");
                                health -= 1;
                                state = BobState.Retreating;
                            }
                        }
                        else
                            timeToStagger = Mathf.Min (timeToStagger + Time.deltaTime, 3);

                        if (-bob.localPosition.z <= atPlayerDistance && !attacking)
                        {
                            //bobAnim.SetTrigger ("Attack");
                            state = BobState.Retreating;
                        }
                    }
                    break;

                case BobState.Retreating:
                    Move (-minMaxSpeed.y);
                    if (-bob.localPosition.z >= furthestDistance)
                        Idle ();
                    break;
            }
        }

        private void Approach ()
        {
            SetRandomDirection ();
            moveSpeed = minMaxSpeed.y;
            timeToStagger = 3;
            //bobAnim.SetFloat ("ApproachSpeed", 1);
            //bobAnim.SetTrigger ("Approach");
            state = BobState.Approaching;
        }

        private void Idle ()
        {
            idleWaitTime = Random.Range (3, 5);
            state = BobState.Idle;
        }

        private void Move (float speed)
        {
            //transform.localPosition.AddZ (speed * Time.deltaTime);
            bob.transform.localPosition += Vector3.forward * speed * Time.deltaTime;
        }

        private void SetRandomDirection ()
        {
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.y = Random.Range (0, 360);
            transform.rotation = Quaternion.Euler (rotation);
        }
    }

    public enum BobState
    {
        Idle,
        Approaching,
        AtPlayer,
        Retreating
    }
}
