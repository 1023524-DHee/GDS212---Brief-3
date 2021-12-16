using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PositonChecker : MonoBehaviour
{
    public ObjectSpin objectOne;
    public ObjectSpin objectTwo;
    public ObjectSpin objectThree;
    public ObjectSpin objectFour;
    public bool gateOpened;

    private Vector3 initialPosition;
    public Vector3 finalPosition;
    public EventReference bannerSound;


    private void Start()
	{
        initialPosition = transform.position;
	}

	void Update()
    {
        if(objectOne.correctPosition == true && objectTwo.correctPosition == true 
            && objectThree.correctPosition == true && objectFour.correctPosition == true && gateOpened == false)
        {
            OpenGate();
            RuntimeManager.PlayOneShot(bannerSound);
        }
    }

    private void OpenGate()
    {
        gateOpened = true;
        StartCoroutine(OpenGate_Coroutine());
    }

    private IEnumerator OpenGate_Coroutine()
    {
        float startTime = Time.time;

        while (Time.time < (startTime + 2f))
        {
            transform.position = Vector3.Lerp(initialPosition, finalPosition, (Time.time - startTime) / 2f);
            yield return null;
        }
    }
}
