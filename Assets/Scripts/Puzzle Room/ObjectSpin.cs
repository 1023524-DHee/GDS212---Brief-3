using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using HorrorVR;
using FMODUnity;

public class ObjectSpin : MonoBehaviour
{
    //[SerializeField] private InputActionAsset actionAsset;
    private bool canInteract;
    private bool isSpinning;
    private int currentSpinPosition = 0;
    int spinAmount = 0;
    public Animator cubeAnimator;
    public GameObject button;
    
    public bool correctPosition = false;
    public int positionNumber;
    public PositonChecker gate;
    public Collider trigger;
    public Collider otherCollider;
    public EventReference bannerSound;

    /*private void Start()
	{
        var L_Grip = actionAsset.FindActionMap("XRI LeftHand").FindAction("Grip");
        L_Grip.Enable();
        L_Grip.performed += IncreaseInt;

        var R_Grip = actionAsset.FindActionMap("XRI RightHand").FindAction("Grip");
        R_Grip.Enable();
        R_Grip.performed += IncreaseInt;

        
    }*/

    public void IncreaseInt(/*InputAction.CallbackContext context*/)
    {
        //if (!canInteract) return;
        if (isSpinning) return;

        StartCoroutine(SpinTotem_Coroutine());
        spinAmount++;
        Debug.Log(spinAmount);
    }

    //private void Update()
    //{
    //    switch (spinAmount)
    //    {
    //        case 0:
    //            if(positionNumber == 0)
    //            {
    //                correctPosition = true;
    //            }
    //            break;

    //        case 1:
    //            //0-90 
    //            cubeAnimator.Play("CubeAnim0-90");
    //            if (positionNumber == 90)
    //            {
    //                correctPosition = true;
    //            }
    //            break;

    //        case 2:
    //            //90-180
    //            cubeAnimator.Play("CubeAnim90-180");
    //            if (positionNumber == 180)
    //            {
    //                correctPosition = true;
    //            }
    //            break;

    //        case 3:
    //            //180-270 
    //            cubeAnimator.Play("CubeAnim180-270");
    //            if (positionNumber == 270)
    //            {
    //                correctPosition = true;
    //            }
    //            break;

    //        case 4:
    //            //270-360
    //            cubeAnimator.Play("CubeAnim270-0");
    //            if (positionNumber == 0)
    //            {
    //                correctPosition = true;
    //            }
    //            spinAmount = 0;
    //            break;
    //    }

    //    if (gate.gateOpened == true)
    //    {
    //        correctPosition = false;
    //    }
    //}

    public IEnumerator SpinTotem_Coroutine()
    {
        isSpinning = true;
        float startTime = Time.time;
        currentSpinPosition = (currentSpinPosition + 90)%360;
        Quaternion newRotation = Quaternion.Euler(0f, currentSpinPosition, 0f);

		while (Time.time < (startTime + 1f))
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, (Time.time - startTime));
			yield return null;
		}

		//transform.Rotate(new Vector3(0, transform.rotation.y + 90f, 0f));
		//yield return new WaitForSeconds(0.5f);

		
        if (currentSpinPosition == positionNumber)
        {
            correctPosition = true;
        }
        else
        {
            correctPosition = false;
        }

        isSpinning = false;
        RuntimeManager.PlayOneShot(bannerSound);

    }

	/*private void OnTriggerEnter(Collider other)
	{
        canInteract = true;
	}

	private void OnTriggerExit(Collider other)
	{
        canInteract = false;
    }*/
}
