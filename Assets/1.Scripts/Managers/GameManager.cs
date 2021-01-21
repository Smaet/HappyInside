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
        HomeManager.Instance.topUIManager.SetNotice("4일 19시간후 할아버지의 의심도 오를 확률이 큼!!");
        HomeManager.Instance.topUIManager.SetPinkChip(userProperties.pinkChip);
        HomeManager.Instance.topUIManager.SetHour(userProperties.gameHour);
        HomeManager.Instance.topUIManager.SetDays(userProperties.daysElapsed);

        //게임 시간 시작
        HomeManager.Instance.timeManager.StartGameTime(user.userBaseProperties.gameHour, user.userBaseProperties.daysElapsed);

        //활성화 되어있는 동료들 활성화
        if(userProperties.collegueInfos[(int)CollegueIndex.HACKER].isActive)
        {
            HomeManager.Instance.timeManager.StartRunHacker();
        }


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
       
        User tempUserData = new User();
        //user.SetNick("HappyRiccc22222");

        tempUserData.userBaseProperties = new UserBaseProperties();
        tempUserData.userBaseProperties.nickName = "플렉스";
        tempUserData.userBaseProperties.crystal = 9999;
        tempUserData.userBaseProperties.startMoney = 50000000000;
        tempUserData.userBaseProperties.ConsumptionMoney = 0;
        tempUserData.userBaseProperties.manipulatedMoney = 5000000000;
        tempUserData.userBaseProperties.resultMoney = 0;
        tempUserData.userBaseProperties.recentChangeMoney = 0;
        tempUserData.userBaseProperties.gameHour = 0;
        tempUserData.userBaseProperties.daysElapsed = 1;
        tempUserData.userBaseProperties.doubt = 0;
        tempUserData.userBaseProperties.pinkChip = 100000;
        tempUserData.userBaseProperties.FlexConsumption = 0;
        tempUserData.userBaseProperties.collegueInfos = new collegueInfo[5];
        
        for(int i=0; i < tempUserData.userBaseProperties.collegueInfos.Length; i++)
        {
            tempUserData.userBaseProperties.collegueInfos[i] = new collegueInfo();
            if(i == (int)CollegueIndex.HACKER)
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

                for(int j = 0; j < 3; j++)
                {
                    tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills[j] = new colleguePassiveSkill();
                }

                tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills[0].chance = 10;
                tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills[1].chance = 20;
                tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills[2].chance = 30;
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

        //Debug.Log(localUser.nickName);

        //ES3.Save<User>("localUser", user);

        //User localUser = ES3.Load<User>("localUser");

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
            TenThousand_str = string.Format("{0}천만원", (Billion % 100000000) / 10000);
        }

        if (Bill != 0)
        {
            result +=  Billion_str;  
        }
        else if(Thou != 0)
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
}
