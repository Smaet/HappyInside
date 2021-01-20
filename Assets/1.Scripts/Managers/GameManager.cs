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
        user = new User();
        User tempUserData = new User();
        //user.SetNick("HappyRiccc22222");

        tempUserData.userBaseProperties = new UserBaseProperties();
        tempUserData.userBaseProperties.nickName = "HappyRichMan";
        tempUserData.userBaseProperties.crystal = 9999;
        tempUserData.userBaseProperties.startMoney = 99999999999;
        tempUserData.userBaseProperties.money = 99999999999;
        tempUserData.userBaseProperties.manipulatedMoney = 99999999999;
        tempUserData.userBaseProperties.resultMoney = 99999999999;
        tempUserData.userBaseProperties.recentChangeMoney = 99999999999;
        tempUserData.userBaseProperties.gameHour = 0;
        tempUserData.userBaseProperties.daysElapsed = 0;
        tempUserData.userBaseProperties.doubt = 0;
        tempUserData.userBaseProperties.pinkChip = 0;
        tempUserData.userBaseProperties.accumulatedConsumption = 0;
        tempUserData.userBaseProperties.collegueInfos = new collegueInfo[5];
        
        for(int i=0; i < tempUserData.userBaseProperties.collegueInfos.Length; i++)
        {
            tempUserData.userBaseProperties.collegueInfos[i] = new collegueInfo();
            tempUserData.userBaseProperties.collegueInfos[i].Level = 0;
            tempUserData.userBaseProperties.collegueInfos[i].itemLevel = 0;
            tempUserData.userBaseProperties.collegueInfos[i].deviceLevel = 0;
        }

        user.SetUserInfo(tempUserData);

        //Debug.Log(localUser.nickName);

        //ES3.Save<User>("localUser", user);

        //User localUser = ES3.Load<User>("localUser");

        //Debug.Log(localUser.nickName);


    }

    #endregion
}
