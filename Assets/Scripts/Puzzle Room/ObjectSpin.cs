using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using HorrorVR;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

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
    public EventReference spinSound;

    public void IncreaseInt(/*InputAction.CallbackContext context*/)
    {
        //if (!canInteract) return;
        if (isSpinning) return;

        StartCoroutine(SpinTotem_Coroutine());
        spinAmount++;
        Debug.Log(spinAmount);
    }

    public IEnumerator SpinTotem_Coroutine()
    {
        isSpinning = true;
        float startTime = Time.time;
        currentSpinPosition = (currentSpinPosition + 90)%360;
        Quaternion newRotation = Quaternion.Euler(0f, currentSpinPosition, 0f);

        var instance = RuntimeManager.CreateInstance(spinSound);
        RuntimeManager.AttachInstanceToGameObject(instance, transform);
        instance.setVolume(1f);
        instance.start();
        
		while (Time.time < (startTime + 1f))
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, (Time.time - startTime));
			yield return null;
		}

        if (currentSpinPosition == positionNumber)
        {
            correctPosition = true;
        }
        else
        {
            correctPosition = false;
        }

        isSpinning = false;
        instance.stop(STOP_MODE.ALLOWFADEOUT);
    }
}
