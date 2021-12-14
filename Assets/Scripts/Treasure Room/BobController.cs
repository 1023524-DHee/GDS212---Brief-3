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
                bobAnim.SetFloat ("ApproachSpeed", moveSpeed / minMaxSpeed.y);
            }
        }
        private float timeToStagger, idleWaitTime, atPlayerWaitTime;
        private int health = 4;
        private bool attacking = false;

        private void Start ()
        {
            Idle ();
        }

        private void Update ()
        {
            //print ($"{-bob.localPosition.z} <= {inRangeDistance} ? {InRange}");
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
                                print ("Staggering");
                                bobAnim.SetTrigger ("Stagger");
                                FMODUnity.RuntimeManager.PlayOneShotAttached ("event:/Audio_Events/BOB/Roar/BOB Roar 2", bob.gameObject);

                                health -= 1;
                                state = BobState.AtPlayer;
                                atPlayerWaitTime = 0;
                                attacking = false;
                                break;
                            }
                        }
                        else
                            timeToStagger = Mathf.Min (timeToStagger + Time.deltaTime, 3);

                        print ($"At Player: {-bob.localPosition.z <= atPlayerDistance}, Attacking: {attacking}");
                        if (-bob.localPosition.z <= atPlayerDistance && !attacking)
                        {
                            print ("Starting Attack");
                            bobAnim.SetFloat ("AttackType", ((float)Random.Range (0, 3) / 2f));
                            bobAnim.SetTrigger ("Attack");
                            attacking = true;
                        }
                    }
                    break;

                case BobState.AtPlayer:
                    if (atPlayerWaitTime > 0)
                        atPlayerWaitTime -= Time.deltaTime;
                    else
                        Retreat ();
                    break;

                case BobState.Retreating:
                    Move (-minMaxSpeed.y);
                    if (-bob.localPosition.z >= furthestDistance)
                        Idle ();
                    break;
            }
        }

        public void AttackComplete ()
        {
            print ("Attack Successful");
            //state = BobState.AtPlayer;
            //atPlayerWaitTime = 0.75f;
            FMODUnity.RuntimeManager.PlayOneShotAttached ("event:/Audio_Events/BOB/Roar/BOB Roar 3", bob.gameObject);
            Retreat ();
            attacking = false;
        }

        private void Approach ()
        {
            SetRandomDirection ();
            moveSpeed = minMaxSpeed.y;
            timeToStagger = 3;
            bobAnim.SetTrigger ("Approach");
            bobAnim.SetFloat ("ApproachSpeed", 1);
            state = BobState.Approaching;
        }

        private void Retreat ()
        {
            bobAnim.SetFloat ("ApproachSpeed", -1);
            state = BobState.Retreating;
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
