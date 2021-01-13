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


    #region Flex Mini Game 백화점용

    private ReactiveProperty<int> _timerReactiveProperty;

    CompositeDisposable disposables = new CompositeDisposable(); // field


    private bool isFlexMiniGameTimeOn = false;
    private IConnectableObservable<int> _countDownFlexGameDepartmentObservable;
    public IObservable<int> CountDownFlexGameDepartmentObservable => _countDownFlexGameDepartmentObservable.AsObservable();     //실제 카운트다운 스트림
    private IObservable<int> CreateCountDownFlexGameDepartmentObservable(int countTime) =>
        Observable
            .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))                    // 0초 이후 1초 간격으로 실행
            .Select(x => (int)(countTime - x))                                          // x는 시작하고 나서의 시간(초)
            .TakeWhile(x => x > 0);                                                     // 0초 초과 동안 OnNext 0이 되면 OnComplete

    public void InitTimerFlexGameDepartmentDown(int _countDownTime)
    {
        isFlexMiniGameTimeOn = true;
        _countDownFlexGameDepartmentObservable = CreateCountDownFlexGameDepartmentObservable(_countDownTime).Publish();
        _countDownFlexGameDepartmentObservable.Connect();
    }

    #endregion


    public void StopTimer(TimerIndex _index)
    {
        switch (_index)
        {
            case TimerIndex.FLEXGAME_DEPARTMENT:
                //isFlexMiniGameTimeOn = false;
                disposables.Clear();
                break;
        }

    }



    public void StartTimerCountDown(TimerIndex _index, int _timer, int _specificFinTime = 0)
    {
        InitTimerFlexGameDepartmentDown(_timer);

        switch (_index)
        {
            case TimerIndex.FLEXGAME_DEPARTMENT:
                CountDownFlexGameDepartmentObservable
                    .Where(timer => isFlexMiniGameTimeOn == true)
                    .Subscribe(time =>
                    //OnNext
                    {
                        Debug.Log(_index.ToString() + "남은 시간 : " + time);
                    },
                    //OnComplete
                    () =>
                    {
                        Debug.Log("TimerCountDown 타이머 끝!");
                    }).AddTo(disposables);
                break;
        }


        
    }

    public void StopTimerFlexGameDepartment()
    {

    }

}
