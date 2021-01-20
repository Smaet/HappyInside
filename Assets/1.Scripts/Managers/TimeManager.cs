using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


public enum TimerIndex
{
    NONE = -1,
    FLEXGAME_DEPARTMENT = 0,
}


public class TimeManager : MonoBehaviour
{

    [SerializeField]
    private int curHour;
    [SerializeField]
    private int curDay;

    #region Sample
    private IConnectableObservable<int> _countDownObservable;

    public IObservable<int> CountDownObservable => _countDownObservable.AsObservable();

    private IObservable<int> CreateCountDownObservable(int countTime) =>
  Observable
      .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))                    // 0초 이후 1초 간격으로 실행
      .Select(x => (int)(countTime - x))                                          // x는 시작하고 나서의 시간(초)
      .TakeWhile(x => x > 0);                                                     // 0초 초과 동안 OnNext 0이 되면 OnComplete

    public int addTime = 0;

    public void InitTimerDown(int _countDownTime)
    {
        _countDownObservable = CreateCountDownObservable(_countDownTime).Publish();
        _countDownObservable.Connect();
    }


    public void StartTimerDown(int _timer, int _specificFinTime = 0)
    {
        //중간에 Dispose를 해야하는데 gameObject Destroy?
        //중간에 멈추고 싶으면 Where 조건을 하나 더 넣는다.
        //기존에 선언된 타이머에 다시 Operator를 붙이면 Override 되는듯? 선언이 덮어 씌워진다?

        InitTimerDown(_timer);


        CountDownObservable
        //.Where(timer => bIsTimer == false)
        .TakeWhile(x => x > _specificFinTime)
        .Subscribe(time =>
        //OnNext
        {
            Debug.Log("남은 시간 : " + time);
            if(addTime != 0)
            {
                time += addTime;
                addTime = 0;
            }
        },
    //OnComplete
    () =>
    {
        Debug.Log("TimerCountDown 타이머 끝!");
    });
    }
    #endregion


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region 게임의 전체적인 시간
    public void StartGameTime(int _savedHour, int _savedDay)
    {

    }
    #endregion
}
