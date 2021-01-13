using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeComponent : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private bool isTimerOn = false;
    [SerializeField]
    private int currentTime = 0;
    [SerializeField]
    private int minusTime = 1;

    public void StartCountDownTimer(int _startTime)
    {
        currentTime = _startTime;
        if(isTimerOn == false)
        {
            isTimerOn = true;
            StartCoroutine(CountDownTimer());
        }
    
    }
    IEnumerator CountDownTimer()
    {
        

        text.text = currentTime.ToString();

        while (isTimerOn)
        {
            yield return new WaitForSeconds(minusTime);

            currentTime -= minusTime;

            text.text = currentTime.ToString();

        }
    }

    public void StopTimer()
    {
        isTimerOn = false;
    }

    public void AddTime(int _time)
    {
        currentTime += _time;
        text.text = currentTime.ToString();
    }

    public void MinusTime(int _time)
    {
        currentTime -= _time;
        text.text = currentTime.ToString();
    }

}
