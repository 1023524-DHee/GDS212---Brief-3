using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class ObjectSpin : MonoBehaviour
{
    public Animator cubeAnimator;
    int spinAmount = 0;
    public bool correctPosition = false;
    public int positionNumber;
    public PositonChecker gate;

    public void IncreaseInt()
    {
        spinAmount++;
        Debug.Log(spinAmount);
    }

    private void Update()
    {
        switch (spinAmount)
        {
            case 0:
                if(positionNumber == 0)
                {
                    correctPosition = true;
                }
                break;

            case 1:
                //0-90 
                cubeAnimator.Play("CubeAnim0-90");
                if (positionNumber == 90)
                {
                    correctPosition = true;
                }
                break;

            case 2:
                //90-180
                cubeAnimator.Play("CubeAnim90-180");
                if (positionNumber == 180)
                {
                    correctPosition = true;
                }
                break;

            case 3:
                //180-270 
                cubeAnimator.Play("CubeAnim180-270");
                if (positionNumber == 270)
                {
                    correctPosition = true;
                }
                break;

            case 4:
                //270-360
                cubeAnimator.Play("CubeAnim270-0");
                if (positionNumber == 0)
                {
                    correctPosition = true;
                }
                spinAmount = 0;
                break;
        }

        if (gate.gateOpened == true)
        {
            correctPosition = false;
        }
    }
}
