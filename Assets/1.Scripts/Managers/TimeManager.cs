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
    float HourUpdateCycle = 5;          //시간이 업데이트 되는 주기 (5초)
    float DaysUpdateCycle = 24;         //날짜가 업데이트 되는 주기 (24시간)

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
    //임시
    [Header("Hacker")]
    private int hackerSkillTime;        //해커의 능력이 발동 될 때 까지의 실제 타임

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region 게임의 전체적인 시간
    public void StartGameTime(int _savedHour, int _savedDay)
    {
        //시간 관련 UI 갱신
        curHour = _savedHour;
        curDay = _savedDay;

        Debug.Log("<color=red>Hacker 활성화!</color>");

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
            CollegueInfo hacker = GameManager.Instance.user.userBaseProperties.collegueInfos[(int)CollegueIndex.Dare];


            if (time >= HourUpdateCycle)
            {
                time = 0;
              
                user.SetUserInfo(ChangeableUserProperties.GAMEHOUR, 1);

                //게임 시간에 따른 배경 설정
                CheckBackgroundChange(user);

                //시간에 따른 해커의 능력 설정
                hackerSkillTime++;
                CheckHacker(hacker); 

                //버프관련 해커의 버프는 켜지는 순간에 이미 적용이 되어있음
                if(GameManager.Instance.user.userBaseProperties.buffs.Count > 0)
                {
                    for(int i=0; i < GameManager.Instance.user.userBaseProperties.buffs.Count; i++)
                    {
                        //지속시간 남아있을 때
                        if(GameManager.Instance.user.userBaseProperties.buffs[i].remainTime > 0)
                        {
                            GameManager.Instance.user.userBaseProperties.buffs[i].remainTime--;
                        }
                        //지속시간이 끝났을 때
                        else
                        {
                            GameManager.Instance.user.userBaseProperties.buffs.RemoveAt(i);
                        }
                    }
                }
            }

            //날짜 갱신
            else if(user.userBaseProperties.gameHour >  DaysUpdateCycle - 1)
            {
                
                user.SetUserInfo(ChangeableUserProperties.GAMEHOUR, 0);
                user.SetUserInfo(ChangeableUserProperties.DAYSELASPSE, 1);


                SetBackground(0);

                yield return null;
            }

            time += Time.deltaTime;
            yield return null;
        }
    }

    void CheckBackgroundChange(User user)
    {
        //게임 시간에 따른 배경 설정
        if (user.userBaseProperties.gameHour < 8)
        {
            HomeManager.Instance.SetBackground(0);
        }
        else if (user.userBaseProperties.gameHour < 16)
        {
            HomeManager.Instance.SetBackground(1);
        }
        else if (user.userBaseProperties.gameHour < 24)
        {
            HomeManager.Instance.SetBackground(2);
        }
    }

    void SetBackground(int _index)
    {
        HomeManager.Instance.SetBackground(_index);
    }
    

    #region 해커의 시간 관련

    public void CheckHacker(CollegueInfo _hacker)
    {
        if (_hacker.isActive && hackerSkillTime > _hacker.collegueBasicSkill.hour - 1)
        {
            hackerSkillTime = 0;
            Debug.Log("<color=red>Hacker 의 능력으로 조작된 돈 추가  </color> : " + _hacker.collegueBasicSkill.money);

            //해커의 아이템으로 추가되는 능력 더하기

            Debug.Log("<color=purple>Hacker 의 아이템으로 증가된 수치 </color> : " + _hacker.collegueItem.chance);

            double itemChance = _hacker.collegueItem.chance * 0.01;
            long result = _hacker.collegueBasicSkill.money + (long)(_hacker.collegueBasicSkill.money * itemChance);

            //GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.MANIPULATEMONEY, result);
            //HomeManager.Instance.agitManager.collegueView.SetManipulateMoney();
            HomeManager.Instance.agitManager.agitOffice.colleguePanels[0].StartSampleEffect();
        }           
    }

    #endregion

   
    #endregion
}
