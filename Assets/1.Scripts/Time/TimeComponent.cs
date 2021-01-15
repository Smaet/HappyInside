using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeComponent : MonoBehaviour
{
    [SerializeField]
    protected Text text;
    [SerializeField]
    protected bool isTimerOn = false;
    [SerializeField]
    protected int currentTime = 0;
    [SerializeField]
    protected int minusTime = 1;

    [SerializeField]
    protected bool isPause = false;
    public virtual void Init<T>(T _t)
    {

    }
    public virtual void StartCountDownTimer(int _startTime)
    {
        currentTime = _startTime;
        if (isTimerOn == false)
        {
            isTimerOn = true;
            StartCoroutine(CountDownTimer());
        }
    }
    protected virtual IEnumerator CountDownTimer()
    {
        yield return null;
    }

    public void StopTimer()
    {
        isTimerOn = false;
    }

    public void AddTime(int _time)
    {
       // SetPause(true);

        currentTime += _time;
        if (currentTime <= 0)
        {
            text.text = "0";
        }
        else
        {
            text.text = currentTime.ToString();
        }
    }

    public void MinusTime(int _time)
    {
      //  SetPause(true);

        currentTime -= _time;

        if(currentTime <= 0)
        {
            text.text = "0";
        }
        else
        {
            text.text = currentTime.ToString();
        }
        
    }

    public bool TimerOn()
    {
        return isTimerOn;
    }

    public void SetPause(bool _pause)
    {
        isPause = _pause;
    }
}
