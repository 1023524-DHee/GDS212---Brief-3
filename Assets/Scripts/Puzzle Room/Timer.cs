    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HorrorVR.Core;
using FMODUnity;

public class Timer : MonoBehaviour
{
    [Tooltip("How long the timer will last.")]
    public float startingTime;
    public Animator fader;
    float currentTime;
    bool noTimeEventCalled = false;
    [Tooltip("Name of the hub scene")]
    public string hubScene;
    public EventReference bannerSound;

    void Start()
    {
        StartCoroutine(Timer_Coroutine());
        //currentTime = startingTime;
    }


    //void Update()
    //{
    //    currentTime -= 1 * Time.deltaTime;

    //    if (currentTime == 0.5 * startingTime)
    //    {
    //        Debug.Log("Half-Time Reached");
    //    }

    //    if (currentTime == 0 && noTimeEventCalled == false)
    //    {
    //        Debug.Log("Time's Up");
    //        MovementTypeManager.current.Loadlevel(hubScene);
    //    }
    //}

    private IEnumerator Timer_Coroutine()
    {
        yield return new WaitForSeconds(startingTime);
        fader.SetTrigger ("FadeRed");
        FMODUnity.RuntimeManager.PlayOneShot ("event:/Audio_Events/BOB/Roar/BOB Roar 3");
        yield return new WaitForSeconds (1.5f);
        MovementTypeManager.current.Loadlevel(hubScene);
        RuntimeManager.PlayOneShot(bannerSound);
    }
}
