/*
  2021.01.05 Created 
  GameManager.cs   
  규칙
  1.변수는 소문자로 시작해서 구분하기 위한 부분에 대문자를 넣어서 사용
  2.함수의 매개 변수는 '_'을 변수 앞에 붙이기 -> Example(int _index) {}
  
  기능
  1. 유저 정보 관리
  2. 서버와 통신
  3. 사운드

*/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class GameManager : SimpleSingleton<GameManager>
{


    [Header("User"), SerializeField] 
    public User user;

    public bool isSave = false;


    // Score
    [SerializeField] private float DepthScore;
    public float DepthMultiple;

    public enum GameState
    {
        Ready,
        Playing,
        NotPlaying
    }

    public GameState CurrentGameState;


    protected override void Awake()
    {
        base.Awake();

        Init();

        ClearUserData();

        SetUserInfo();

     }

    private void Start()
    {

        //각종 초기화
        HomeManager.Instance.Init();

        UserBaseProperties userProperties = user.userBaseProperties;

        //각종 셋팅
        //할아버지 의심도및  재산 셋팅
        HomeManager.Instance.comprehensivePanel.SetCurrentAssetStatus_Slider(userProperties.manipulatedMoney);
        HomeManager.Instance.comprehensivePanel.SetCurrentDoubtStatus_Slider(userProperties.ConsumptionMoney, userProperties.manipulatedMoney);
        HomeManager.Instance.comprehensivePanel.SetGarndFaterAssetInfo(userProperties.startMoney, userProperties.manipulatedMoney);

        //상단 패널 셋팅
        HomeManager.Instance.topUIManager.SetCrystal(userProperties.crystal);
        HomeManager.Instance.topUIManager.SetNotice("4일 19시간후 할아버지 의심 떡상!!");
        HomeManager.Instance.topUIManager.SetPinkChip(userProperties.pinkChip);
        HomeManager.Instance.topUIManager.SetHour(userProperties.gameHour);
        HomeManager.Instance.topUIManager.SetDays(userProperties.daysElapsed);

        //게임 시간 시작
        HomeManager.Instance.timeManager.StartGameTime(user.userBaseProperties.gameHour, user.userBaseProperties.daysElapsed);

        //게임 시간에 따른 배경 설정
        if(user.userBaseProperties.gameHour < 9)
        {
            HomeManager.Instance.SetBackground(0);
        }
        else if(user.userBaseProperties.gameHour < 17)
        {
            HomeManager.Instance.SetBackground(1);
        }
        else if(user.userBaseProperties.gameHour < 24)
        {
            HomeManager.Instance.SetBackground(2);
        }

       

        //활성화 되어있는 동료들 활성화
        if (userProperties.collegueInfos[(int)CollegueIndex.HACKER].isActive)
        {
            HomeManager.Instance.timeManager.StartRunHacker();
        }

        //활성화 되어있는 버프들 활성화
        if (userProperties.buffs[0].isActive)
        {
            HomeManager.Instance.timeManager.StartGrandFatherBuff();
        }


        PoloSFX.Instance.PlayHomeBGM();
    }

    void Init()
    {
        //유저 정보 초기화
    }

    #region GameControl

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion

    #region UserData
    public void SetUserInfo()
    {
        

        if(LoadUserData() == false)
        {
            Debug.Log("저장된 데이터 호출!!");
            User tempUserData = new User();
            //user.SetNick("HappyRiccc22222");

            tempUserData.isFirst = true;

            tempUserData.userBaseProperties = new UserBaseProperties();
            tempUserData.userBaseProperties.nickName = "플렉스";
            tempUserData.userBaseProperties.crystal = 99;
            tempUserData.userBaseProperties.startMoney = 50000000000;
            tempUserData.userBaseProperties.ConsumptionMoney = 0;
            tempUserData.userBaseProperties.manipulatedMoney = 5000000000;
            tempUserData.userBaseProperties.resultMoney = 0;
            tempUserData.userBaseProperties.recentChangeMoney = 0;
            tempUserData.userBaseProperties.donateMoney = 1000000000;
            tempUserData.userBaseProperties.gameHour = 0;
            tempUserData.userBaseProperties.daysElapsed = 1;

            tempUserData.userBaseProperties.doubt  = new Doubt();
            tempUserData.userBaseProperties.doubt.curDoubt = 0;
            tempUserData.userBaseProperties.doubt.basicDoubt = 0;
            tempUserData.userBaseProperties.doubt.GrandFatherDoubt = 0;

            tempUserData.userBaseProperties.pinkChip = 1000;
            tempUserData.userBaseProperties.FlexConsumption = 0;

            tempUserData.userBaseProperties.collegueInfos = new collegueInfo[5];

            tempUserData.userBaseProperties.buffs = new Buff[1];
            tempUserData.userBaseProperties.buffs[0] = new Buff();
            tempUserData.userBaseProperties.buffs[0].isActive = false;
            tempUserData.userBaseProperties.buffs[0].isRunning = false;
            tempUserData.userBaseProperties.buffs[0].isGood = false;
            tempUserData.userBaseProperties.buffs[0].isBuffed = false;

            tempUserData.userBaseProperties.buffs[0].continueTime = 12;
            tempUserData.userBaseProperties.buffs[0].remainTime = 0;
            tempUserData.userBaseProperties.buffs[0].effect_Doubt_Plus = -5;
            tempUserData.userBaseProperties.buffs[0].effect_Doubt_Minus = 5;

            for (int i = 0; i < tempUserData.userBaseProperties.collegueInfos.Length; i++)
            {
                tempUserData.userBaseProperties.collegueInfos[i] = new collegueInfo();
                if (i == (int)CollegueIndex.HACKER)
                {   
                    tempUserData.userBaseProperties.collegueInfos[i].isActive = true;

                    tempUserData.userBaseProperties.collegueInfos[i].Level = 1;
                    tempUserData.userBaseProperties.collegueInfos[i].itemLevel = 1;
                    tempUserData.userBaseProperties.collegueInfos[i].deviceLevel = 1;
                    tempUserData.userBaseProperties.collegueInfos[i].collegueBasicSkill = new collegueBasicSkill();
                    tempUserData.userBaseProperties.collegueInfos[i].collegueBasicSkill.hour = 6;
                    tempUserData.userBaseProperties.collegueInfos[i].collegueBasicSkill.money = 10000000;
                    tempUserData.userBaseProperties.collegueInfos[i].collegueBasicSkill.day = -1;
                    tempUserData.userBaseProperties.collegueInfos[i].collegueBasicSkill.chance = -1;

                    tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills = new colleguePassiveSkill[3];

                    for (int j = 0; j < 3; j++)
                    {
                        tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills[j] = new colleguePassiveSkill();
                    }

                    tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills[0].chance = 10;
                    tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills[1].chance = 20;
                    tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills[2].chance = 30;

                    tempUserData.userBaseProperties.collegueInfos[i].collegueItem = new collegueItem();
                    tempUserData.userBaseProperties.collegueInfos[i].collegueItem.isActive = true;
                    tempUserData.userBaseProperties.collegueInfos[i].collegueItem.chance = 10;

                }
                else
                {
                    tempUserData.userBaseProperties.collegueInfos[i].isActive = true;

                    tempUserData.userBaseProperties.collegueInfos[i].Level = 0;
                    tempUserData.userBaseProperties.collegueInfos[i].itemLevel = 0;
                    tempUserData.userBaseProperties.collegueInfos[i].deviceLevel = 0;
                    tempUserData.userBaseProperties.collegueInfos[i].collegueBasicSkill = new collegueBasicSkill();
                    tempUserData.userBaseProperties.collegueInfos[i].collegueBasicSkill.hour = 6;
                    tempUserData.userBaseProperties.collegueInfos[i].collegueBasicSkill.money = 10000000;
                    tempUserData.userBaseProperties.collegueInfos[i].collegueBasicSkill.day = -1;
                    tempUserData.userBaseProperties.collegueInfos[i].collegueBasicSkill.chance = -1;



                    tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills = new colleguePassiveSkill[3];

                    for (int j = 0; j < 3; j++)
                    {
                        tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills[j] = new colleguePassiveSkill();
                    }

                    tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills[0].chance = 10;
                    tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills[1].chance = 20;
                    tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills[2].chance = 30;
                }

            }


            user.SetUserInfo(tempUserData);
        }
        else
        {
            Debug.Log("초기 데이터로 시작!");
        }

        //Debug.Log(localUser.nickName);

        


    }

    #endregion

    #region Util

    public string GetMoneyFormat(long _money)
    {
        string result = "";

        long Billion = (_money);
        string Billion_str = "";
        string TenThousand_str = "";

        var Bill = (Billion % 100000000000) / 100000000;
        var Thou = (Billion % 100000000) / 10000;

        if (Billion >= 100000000)
        {
            Billion_str = string.Format("{0}억", (Billion % 100000000000) / 100000000);
        }
        if (Billion >= 10000)
        {
            TenThousand_str = string.Format("{0}천", (Billion % 100000000) / 10000000);
        }

        if (Bill != 0)
        {
            result +=  Billion_str;  
        }
        if(Thou != 0)
        {
            result +=  " " + TenThousand_str;
        }

        return result;
    }

    public string GetMoneyFormat(float _money)
    {
        string result = "";

        float Billion = (_money);
        string Billion_str = "";
        string TenThousand_str = "";

        var Bill = (Billion % 100000000000) / 100000000;
        var Thou = (Billion % 100000000) / 10000;

        if (Billion >= 100000000)
        {
            Billion_str = string.Format("{0}억", (Billion % 100000000000) / 100000000);
        }
        if (Billion >= 10000)
        {
            TenThousand_str = string.Format("{0}천만원", (Billion % 100000000) / 10000);
        }

        if (Thou != 0)
        {
            result = Billion_str;
        }
        else
        {
            result = Billion_str + " " + TenThousand_str;
        }

        return result;
    }
    #endregion


    #region LocalSave

    public void SaveUserData()
    {
        ES3.Save("localUser",user);

        if (ES3.KeyExists("localUser"))
        {
            Debug.Log("저장 완료");
        }
        else
        {
            Debug.Log("저장 실패");
        }

    }

    public bool LoadUserData()
    {
        if(ES3.KeyExists("localUser"))
        {
            user = ES3.Load<User>("localUser");
            return true;
        }
        else
        {
            return false;
        }

        

       
    }

    public void ClearUserData()
    {
        if(ES3.KeyExists("localUser"))
        {
            ES3.DeleteKey("localUser");
        }
        else
        {
            Debug.Log("저장된 데이터가 존재하지 않음");
        }
      
    }

    #endregion
}
