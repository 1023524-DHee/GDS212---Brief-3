using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Tooltip("How long the timer will last.")]
    public float startingTime;
    float currentTime;

    void Start()
    {
        currentTime = startingTime;
    }

    
    void Update()
    {
        currentTime -= 1 * Time.deltaTime; 

        if (currentTime == 0.5 * startingTime)
        {
            Debug.Log("Half-Time Reached");
        }

        if (currentTime == 0)
        {
            Debug.Log("Time's Up");
        }
    }
}
