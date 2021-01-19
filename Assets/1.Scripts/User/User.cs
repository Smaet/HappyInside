/*
  2021.01.11 Created 
  User.cs   

  
  기능
  1. 유저 정보
   - 재산
   - 게임 시간(경과된 일수)
   - 의심도
   - 블랙 코인
   - 아지트
   - 동료 (Hacker, Mechanic, Chemist, Cook, Trader)

*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//유저의 기본 변수들
[Serializable]
public class UserBaseProperties
{
    public string nickName;             //닉네임
    public int crystal;                 //크리스탈
    public long startMoney;              //시작재산 (고정된 재산)
    public long money;                   //현재 가지고 있는 돈 (실제 재산)
    public long manipulatedMoney;        //현재 조작된 돈
    public long resultMoney;             //현재 잔액
    public long recentChangeMoney;       //최근 변화된 돈
    public float gameTime;              //게임 시간
    public float daysElapsed;           //경과된 일수
    public float doubt;                 //의심도
    public float pinkChip;              //핑크칩?
    public long accumulatedConsumption;  //누적 소비액  --> 해당 금액에 따른 난이도 변화

    public collegueInfo[] collegueInfos;

    public UserBaseProperties()
    {
        collegueInfos = new collegueInfo[5];
        for(int i=0; i < collegueInfos.Length; i++)
        {
            collegueInfos[i] = new collegueInfo();
        }
    }
}

[Serializable]
public class collegueInfo
{
    public int Level;               //동료의 레벨
    public int itemLevel;           //아이템 레벨
    public int deviceLevel;         //디바이스 레벨
}


public class User : MonoBehaviour
{
    public UserBaseProperties userBaseProperties;


    #region Setter


    #endregion
    //아지트(추가예정)
    //동료(추가예정)
    

    public void SetUserInfo(User _userInfo)
    {
        userBaseProperties = new UserBaseProperties();
        

        userBaseProperties.nickName = _userInfo.userBaseProperties.nickName;
        userBaseProperties.crystal = _userInfo.userBaseProperties.crystal;
        userBaseProperties.startMoney = _userInfo.userBaseProperties.startMoney;
        userBaseProperties.money = _userInfo.userBaseProperties.money;
        userBaseProperties.manipulatedMoney = _userInfo.userBaseProperties.manipulatedMoney;
        userBaseProperties.resultMoney = _userInfo.userBaseProperties.resultMoney;
        userBaseProperties.recentChangeMoney = _userInfo.userBaseProperties.recentChangeMoney;
        userBaseProperties.gameTime = _userInfo.userBaseProperties.gameTime;
        userBaseProperties.daysElapsed = _userInfo.userBaseProperties.daysElapsed;
        userBaseProperties.doubt = _userInfo.userBaseProperties.doubt;
        userBaseProperties.pinkChip = _userInfo.userBaseProperties.pinkChip;
        userBaseProperties.accumulatedConsumption = _userInfo.userBaseProperties.accumulatedConsumption;
        userBaseProperties.collegueInfos = _userInfo.userBaseProperties.collegueInfos;


        Debug.Log("NickName : " + userBaseProperties.nickName);
        Debug.Log("crystal : " + userBaseProperties.crystal);
        Debug.Log("startMoney : " + userBaseProperties.startMoney);
        Debug.Log("money : " + userBaseProperties.money);
        Debug.Log("manipulatedMoney : " + userBaseProperties.manipulatedMoney);
        Debug.Log("NickName : " + userBaseProperties.resultMoney);
        Debug.Log("resultMoney : " + userBaseProperties.nickName);
        Debug.Log("recentChangeMoney : " + userBaseProperties.recentChangeMoney);
        Debug.Log("gameTime : " + userBaseProperties.gameTime);
        Debug.Log("daysElapsed : " + userBaseProperties.daysElapsed);
        Debug.Log("doubt : " + userBaseProperties.doubt);
        Debug.Log("blackCoin : " + userBaseProperties.pinkChip);
        Debug.Log("accumulatedConsumption : " + userBaseProperties.accumulatedConsumption);

        Debug.Log("<color=red>해커의 현재 레벨 : </color>" + userBaseProperties.collegueInfos[0].Level);
        Debug.Log("<color=red>해커의 현재 아이템 레벨 : </color>" + userBaseProperties.collegueInfos[0].itemLevel);
        Debug.Log("<color=red>해커의 현재 디바이스 레벨 </color> " + userBaseProperties.collegueInfos[0].deviceLevel);

        Debug.Log("<color=blue>기계공학자의 현재 레벨 : </color>" + userBaseProperties.collegueInfos[0].Level);
        Debug.Log("<color=blue>기계공학자의 현재 아이템 레벨 : </color>" + userBaseProperties.collegueInfos[0].itemLevel);
        Debug.Log("<color=blue>기계공학자의 현재 디바이스 레벨 : </color>" + userBaseProperties.collegueInfos[0].deviceLevel);

        Debug.Log("<color=yellow>화학자의 현재 레벨 : </color>" + userBaseProperties.collegueInfos[0].Level);
        Debug.Log("<color=yellow>화학자의 현재 아이템 레벨 : </color>" + userBaseProperties.collegueInfos[0].itemLevel);
        Debug.Log("<color=yellow>화학자의 현재 디바이스 레벨 : </color>" + userBaseProperties.collegueInfos[0].deviceLevel);

        Debug.Log("<color=purple>요리사의 현재 레벨 : </color>" + userBaseProperties.collegueInfos[0].Level);
        Debug.Log("<color=purple>요리사의 현재 아이템 레벨 : </color>" + userBaseProperties.collegueInfos[0].itemLevel);
        Debug.Log("<color=purple>요리사의 현재 디바이스 레벨 : </color>" + userBaseProperties.collegueInfos[0].deviceLevel);

        Debug.Log("<color=green>트레이더의 현재 레벨 : </color>" + userBaseProperties.collegueInfos[0].Level);
        Debug.Log("<color=green>트레이더의 현재 아이템 레벨 : </color>" + userBaseProperties.collegueInfos[0].itemLevel);
        Debug.Log("<color=green>트레이더의 현재 디바이스 레벨 : </color>" + userBaseProperties.collegueInfos[0].deviceLevel);

    }

}
