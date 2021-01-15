using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepartmentGameTimer : TimeComponent
{
    DepartmentStoreGame dGame;

    public override void Init<T>(T _t)
    {
        dGame = _t as DepartmentStoreGame;
    }

    public override void StartCountDownTimer(int _startTime)
    {
        base.StartCountDownTimer(_startTime);
    }

    protected override IEnumerator CountDownTimer()
    {
        float time = currentTime;
        text.text = currentTime.ToString();

        while (isTimerOn)
        {
            if (currentTime <= 0)
            {
                text.text = currentTime.ToString();
                isTimerOn = false;
                yield break;
            }

            yield return new WaitForSeconds(minusTime);

            if(isPause == false)
            {
                currentTime -= minusTime;
                //time -= Time.deltaTime;
            }


            
            text.text = currentTime.ToString();

            yield return null;
        }
    }
}
