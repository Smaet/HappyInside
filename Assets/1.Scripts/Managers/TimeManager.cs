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
    float HourUpdateCycle = 5;      //시간이 업데이트 되는 주기 (5초)
    float DaysUpdateCycle = 24;      //날짜가 업데이트 되는 주기 (24시간)

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
        //시간 관련 UI 갱신
        curHour = _savedHour;
        curDay = _savedDay;
        StartCoroutine(GameTimer());
    }

    IEnumerator GameTimer()
    {
        float time = 0;

        while(true)
        {
            //시간 관련 UI 갱신
            //동료들이 있다면 유저에게 있는 동료에 관한 기본능력 처리 
            //시간 갱신
            User user = GameManager.Instance.user;


            if (time >= HourUpdateCycle)
            {
                time = 0;
                user.SetUserInfo(ChangeableUserProperties.GAMEHOUR, 1);
            }

            //날짜 갱신
            else if(user.userBaseProperties.gameHour >  DaysUpdateCycle - 1)
            {
                
                user.SetUserInfo(ChangeableUserProperties.GAMEHOUR, 0);
                user.SetUserInfo(ChangeableUserProperties.DAYSELASPSE, 1);

                yield return null;
            }

            time += Time.deltaTime;
            yield return null;
        }
    }
    #region 해커의 시간 관련
    public void StartRunHacker()
    {
        Debug.Log("<color=red>Hacker 활성화!</color>");
        StartCoroutine(RunHacker());
    }

    IEnumerator RunHacker()
    {
        float time = 0;
        float hourCount = 0;

        while (true)
        {
            //시간 관련 UI 갱신
            //동료들이 있다면 유저에게 있는 동료에 관한 기본능력 처리 
            //시간 갱신
            collegueInfo hacker = GameManager.Instance.user.userBaseProperties.collegueInfos[(int)CollegueIndex.HACKER];


            if (time >= HourUpdateCycle)        //시간이 바뀔때.
            {
                time = 0;
                hourCount++;
            }

            else if(hourCount >= hacker.collegueBasicSkill.hour)
            {
                hourCount = 0;
                Debug.Log("<color=red>Hacker 의 능력으로 조작된 돈 추가  </color> : " + hacker.collegueBasicSkill.money);

                //해커의 아이템으로 추가되는 능력 더하기

                Debug.Log("<color=purple>Hacker 의 아이템으로 증가된 수치 </color> : " + hacker.collegueItem.chance);

                double itemChance = hacker.collegueItem.chance * 0.01;
                long result = hacker.collegueBasicSkill.money + (long)(hacker.collegueBasicSkill.money * itemChance);

                GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.MANIPULATEMONEY, result);
                HomeManager.Instance.agitManager.collegueView.SetManipulateMoney();
                HomeManager.Instance.agitManager.agit_A.colleguePanels[0].StartSampleEffect();

            }

            time += Time.deltaTime;
            yield return null;
        }
    }
    #endregion

    #region 할아버지 버프
    public void StartGrandFatherBuff()
    {
        Debug.Log("<color=red>할아버지 버프 활성화!</color>");
        StartCoroutine(GrandFatherBuff());
    }

    IEnumerator GrandFatherBuff()
    {
        float time = 0;
        float hourCount = 0;

      
        while (true)
        {
            Buff grandFather = GameManager.Instance.user.userBaseProperties.buffs[(int)BuffIndex.GRANDFATHER];


            if (grandFather.remainTime > 0)
            {
                if (time >= HourUpdateCycle)        //시간이 바뀔때.
                {
                    time = 0;
                    hourCount++;
                    Debug.Log("<color=green>할아버지 버프 남은 시간.  </color> : " + (grandFather.remainTime - 1));

                    grandFather.remainTime -= 1;
                    GameManager.Instance.user.SetUserInfo(BuffIndex.GRANDFATHER, grandFather);
                }
            }
            else
            {
                hourCount = 0;
                Debug.Log("<color=red>할아버지 버프 종료  </color>");
                grandFather.isActive = false;
                grandFather.isRunning = false;
                grandFather.isBuffed = false;
                grandFather.remainTime = 0;
                grandFather.isReset = true;
                if (grandFather.isGood)
                {
                    grandFather.isGood = false;

                    GameManager.Instance.user.SetUserInfo(BuffIndex.GRANDFATHER, grandFather);
                }
                else
                {
                    grandFather.isGood = false;
                    GameManager.Instance.user.SetUserInfo(BuffIndex.GRANDFATHER, grandFather);
                }

                GameManager.Instance.user.SetUserInfo(BuffIndex.GRANDFATHER, grandFather);

                yield break;

            }
          

            time += Time.deltaTime;
            yield return null;
        }
    }
    #endregion
    #endregion
}
