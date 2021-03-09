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
    private int TraderSkillTime;
    private int CookSkillTime;
    private int ChemistSkillTime;

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
            CollegueInfo trader = GameManager.Instance.user.userBaseProperties.collegueInfos[(int)CollegueIndex.Soso];
            CollegueInfo lovely = GameManager.Instance.user.userBaseProperties.collegueInfos[(int)CollegueIndex.Lovely];


            if (time >= HourUpdateCycle)
            {
                time = 0;
              
                user.SetUserInfo(ChangeableUserProperties.GAMEHOUR, 1);

                //게임 시간에 따른 배경 설정
                CheckBackgroundChange(user);

                //시간에 따른 해커의 능력 설정
                hackerSkillTime++;
                //CheckHacker(hacker);

                //트레이더
                TraderSkillTime++;
                //CheckTrader(trader);

                //요리사
                CookSkillTime++;
                //CheckCook(lovely);

                //화학자
                ChemistSkillTime++;
                //CheckChemist();

                //각각 동료들의 배고픔 체크
                

                //버프관련 해커의 버프는 켜지는 순간에 이미 적용이 되어있음
                if (GameManager.Instance.user.userBaseProperties.buffs.Count > 0)
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
            //else if(user.userBaseProperties.gameHour >  DaysUpdateCycle - 1)
            //{
                
            //    user.SetUserInfo(ChangeableUserProperties.GAMEHOUR, 0);
            //    user.SetUserInfo(ChangeableUserProperties.DAYSELASPSE, 1);


            //    SetBackground(0);

            //    yield return null;
            //}

            time += Time.deltaTime;
            yield return null;
        }
    }

    void CheckBackgroundChange(User user)
    {
        //게임 시간에 따른 배경 설정
        //if (user.userBaseProperties.gameHour < 8)
        //{
        //    HomeManager.Instance.SetBackground(0);
        //}
        //else if (user.userBaseProperties.gameHour < 16)
        //{
        //    HomeManager.Instance.SetBackground(1);
        //}
        //else if (user.userBaseProperties.gameHour < 24)
        //{
        //    HomeManager.Instance.SetBackground(2);
        //}
    }

    void SetBackground(int _index)
    {
        HomeManager.Instance.SetBackground(_index);
    }
    

    #region 동료들의 시간 관련 처리

    //다레
    public void CheckHacker(CollegueInfo _hacker)
    {
        //if (_hacker.isActive && hackerSkillTime > _hacker.collegueBasicSkill.hour - 1)
        {
            hackerSkillTime = 0;

            //??시간 마다 해킹하여 타겟의 재산을 ??만원 만큼 소멸시킨다.
           // Debug.Log("<color=red>Hacker 의 능력으로 조작된 돈 추가  </color> : " + _hacker.collegueBasicSkill.money);

            //패시브 1 -> 해킹시간 1시간 감소
            //적용 시점에 한번만 적용
            //if (_hacker.colleguePassiveSkills[0].isApply == false)
            //{
            //    _hacker.collegueBasicSkill.hour -= 1;
            //    _hacker.colleguePassiveSkills[0].isApply = true;
            //}

            //패시브 2 -> 재산 소멸량 20% 증가
            //상시 적용 
            //if (_hacker.colleguePassiveSkills[1].isActive)
            //{
            //    double money = _hacker.collegueBasicSkill.money * _hacker.colleguePassiveSkills[1].chance;
            //    _hacker.collegueBasicSkill.money += (long)money;
            //}


            ////패시브 3 -> 어나니머스 -> 해킹시간 50% 감소
            ////상시 적용
            //if (_hacker.colleguePassiveSkills[2].isActive)
            //{
            //    double hour = _hacker.collegueBasicSkill.hour * _hacker.colleguePassiveSkills[2].chance;
            //    _hacker.collegueBasicSkill.hour = (int)hour;
            //}

            //아이템에 따른 확률
            //성공시 재산소멸량 2배 적용
            int itemChance = UnityEngine.Random.Range(0, 100); 

            //if(_hacker.collegueItem.chance < itemChance)
            //{
            //    long itemChanceMoney = _hacker.collegueBasicSkill.money * 2;
            //    GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.CURRENTAMOUNT, - itemChanceMoney);
            //}
            //else
            //{
            //    long result = _hacker.collegueBasicSkill.money;
            //    GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.CURRENTAMOUNT, -result);
            //}
            
            //UI 적용

            //이펙트 적용
            HomeManager.Instance.agitManager.agitOffice.colleguePanels[0].StartSampleEffect();
        }           
    }
    
    //쏘쏘
    public void CheckTrader(CollegueInfo _Trader)
    {
        //if (_Trader.isActive && TraderSkillTime > _Trader.collegueBasicSkill.hour - 1)
        {
            TraderSkillTime = 0;

            //??시간 마다 블랙칩을 트레이딩 하여 n%의 수익을 남긴다
            //블랙칩 10개 이상 보유 중일 때, 발동
            //초기 최대 사용 개수 10개
            //블랙칩 보유 수에서 100개 단위 버림하여 기준 금액으로 활용
            //(최대 500개)
            //if(GameManager.Instance.user.userBaseProperties.blackChip >= 10)
            //{
            //    double count = 0;
            //    double benefitResult = 0;
            //    if (_Trader.collegueItem.count < GameManager.Instance.user.userBaseProperties.blackChip)
            //    {
            //        count = _Trader.collegueItem.count - (_Trader.collegueItem.count % 10);
            //        benefitResult = (count * _Trader.collegueBasicSkill.chance);
            //        GameManager.Instance.user.userBaseProperties.blackChip += benefitResult;
            //    }
            //    else
            //    {
            //        count = GameManager.Instance.user.userBaseProperties.blackChip - (GameManager.Instance.user.userBaseProperties.blackChip % 10);
            //        benefitResult = (count * _Trader.collegueBasicSkill.chance);
            //        GameManager.Instance.user.userBaseProperties.blackChip += benefitResult;
            //    }
            //}
        }
    }

    //럽삐
    public void CheckCook(CollegueInfo _cook)
    {
        //if (_cook.isActive && CookSkillTime > _cook.collegueBasicSkill.hour - 1)
        //{
        //    CookSkillTime = 0;
        //    //??시간 마다 디저트를 ??개 만들어서 동료들의 허기를 채워준다.
        //    //패시브 3 -> 디저트 생산량 50% 증가
        //    if (_cook.colleguePassiveSkills[2].isActive)
        //    {
        //        double makeCount = _cook.collegueBasicSkill.count;
        //        //소수점은 짜름
        //        _cook.collegueBasicSkill.count = (int)(makeCount + (makeCount * 0.5));
        //    }

        //    //아이템에 따른 확률
        //    int itemChance = UnityEngine.Random.Range(0, 100);

        //    //if (_cook.collegueItem.chance < itemChance)
        //    //{
        //    //    long cookCount = _cook.collegueBasicSkill.count * 2;
        //    //    GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.DESSERT, cookCount);
        //    //}
        //    //else
        //    //{
        //    //    GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.DESSERT, _cook.collegueBasicSkill.count);
        //    //}
        //}
    }

    //세드
    public void CheckChemist(CollegueInfo _chemist)
    {
        //if (_chemist.isActive && ChemistSkillTime > _chemist.collegueBasicSkill.hour - 1)
        //{
        //    ChemistSkillTime = 0;




        //    //패시브 2 -> 재산 소멸량 50% 증가
        //    //상시 적용 
        //    if (_chemist.colleguePassiveSkills[1].isActive)
        //    {
        //        double money = _chemist.collegueBasicSkill.money * _chemist.colleguePassiveSkills[1].chance;
        //        _chemist.collegueBasicSkill.money += (long)money;
        //    }

        //    //패시브 3 -> 재산 소멸량 3배 적용 (10%의 확률로)
        //    //아이템에 따른 확률
        //    int itemChance = UnityEngine.Random.Range(0, 100);

        //    if (_chemist.colleguePassiveSkills[2].chance < itemChance)
        //    {
        //        long removeMount = _chemist.collegueBasicSkill.money * 3;
        //        GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.CURRENTAMOUNT, removeMount);
        //    }
        //    else
        //    {
        //        GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.CURRENTAMOUNT, _chemist.collegueBasicSkill.money);
        //    }

        //}
    }

    #endregion


    #endregion
}
