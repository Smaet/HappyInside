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

public enum CollegueIndex
{
    HACKER = 0,
    MECHANIC,
    CHEMIST,
    COOK,
    TRADER,
}




public enum ChangeableUserProperties
{
    NONE = 0,
    CRYSTAL = 1,
    STARTMONEY,
    MONEY,
    MANIPULATEMONEY,
    RESULTMONEY,
    RECENTCHANGEMONEY,
    GAMEHOUR,
    DAYSELASPSE,
    DOUBT,
    PINKCHIP,
    ACCUMULATEDCONSUMPTION,
    HACKER,

}


//유저의 기본 변수들
[Serializable]
public class UserBaseProperties
{
    public string nickName;                 //닉네임
    public int crystal;                     //크리스탈
    public long startMoney;                 //시작재산 (고정된 재산)
    public long money;                      //현재 가지고 있는 돈 (실제 재산)
    public long manipulatedMoney;           //현재 조작된 돈
    public long resultMoney;                //현재 잔액
    public long recentChangeMoney;          //최근 변화된 돈
    public long accumulatedConsumption;     //누적 소비액  --> 해당 금액에 따른 난이도 변화
    public float gameHour;                  //게임 시간
    public float daysElapsed;               //경과된 일수
    public float doubt;                     //의심도
    public float pinkChip;                  //핑크칩?


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
    public CollegueIndex collegueIndex; //동료의 인덱스

    public int Level;               //동료의 레벨
    public int itemLevel;           //아이템 레벨
    public int deviceLevel;         //디바이스 레벨


}

[Serializable]
public class collegueBasicSkill
{
    public float hour;
    public int day;
    public float money;
    public float chance;
}

[Serializable]
public class colleguePassiveSkill
{
    public bool isActive;
    public float hour;
    public int day;
    public float money;
    public float chance;
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
        userBaseProperties.gameHour = _userInfo.userBaseProperties.gameHour;
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
        Debug.Log("gameHour : " + userBaseProperties.gameHour);
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

    #region UserInfo Setter
    //Setter에 UI 갱신을 넣어야 함 
    public void SetUserInfo(ChangeableUserProperties _changeableIndex, int _value)
    {

        switch (_changeableIndex)
        {
            case ChangeableUserProperties.CRYSTAL:
                userBaseProperties.crystal += _value;
                Debug.Log("현재 크리스탈 : " + userBaseProperties.crystal);
                break;

        }
    }

    public void SetUserInfo(ChangeableUserProperties _changeableIndex, long _value)
    {

        switch (_changeableIndex)
        {
            case ChangeableUserProperties.STARTMONEY:
                userBaseProperties.startMoney += _value;
                Debug.Log("현재 시작금액 : " + userBaseProperties.startMoney);
                break;
            case ChangeableUserProperties.MONEY:
                userBaseProperties.money += _value;
                Debug.Log("현재 금액 : " + userBaseProperties.money);
                break;

            case ChangeableUserProperties.MANIPULATEMONEY:
                userBaseProperties.manipulatedMoney += _value;
                Debug.Log("현재 조작된금액 : " + userBaseProperties.manipulatedMoney);
                break;

            case ChangeableUserProperties.RESULTMONEY:
                userBaseProperties.resultMoney += _value;
                Debug.Log("현재 결과금액 : " + userBaseProperties.resultMoney);
                break;
            case ChangeableUserProperties.RECENTCHANGEMONEY:
                userBaseProperties.recentChangeMoney += _value;
                Debug.Log("현재 최근바뀐금액 : " + userBaseProperties.recentChangeMoney);
                break;
            case ChangeableUserProperties.ACCUMULATEDCONSUMPTION:
                userBaseProperties.accumulatedConsumption += _value;
                Debug.Log("현재 누적금액 : " + userBaseProperties.accumulatedConsumption);
                break;



        }
    }

    public void SetUserInfo(ChangeableUserProperties _changeableIndex, float _value)
    {

        switch (_changeableIndex)
        {
            case ChangeableUserProperties.GAMEHOUR:
                userBaseProperties.gameHour += _value;
                Debug.Log("현재 몇 시인지 : " + userBaseProperties.gameHour);
                break;
            case ChangeableUserProperties.DAYSELASPSE:
                userBaseProperties.daysElapsed += _value;
                Debug.Log("현재 경과된 날짜  : " + userBaseProperties.daysElapsed);
                break;
            case ChangeableUserProperties.DOUBT:
                userBaseProperties.doubt += _value;
                Debug.Log("현재 의심도 : " + userBaseProperties.doubt);
                break;
            case ChangeableUserProperties.PINKCHIP:
                userBaseProperties.pinkChip += _value;
                Debug.Log("현재 핑크 칩 : " + userBaseProperties.pinkChip);
                break;

        }
    }

    public void SetUserInfo(CollegueIndex _collegueIndex, collegueInfo _info)
    {

        userBaseProperties.collegueInfos[(int)_collegueIndex].Level = _info.Level;
        userBaseProperties.collegueInfos[(int)_collegueIndex].itemLevel = _info.itemLevel;
        userBaseProperties.collegueInfos[(int)_collegueIndex].deviceLevel = _info.deviceLevel;


        Debug.Log(_info.collegueIndex.ToString() +  " Level : " + userBaseProperties.collegueInfos[(int)_collegueIndex].Level);
        Debug.Log(_info.collegueIndex.ToString() + " ItemLevel : " + userBaseProperties.collegueInfos[(int)_collegueIndex].itemLevel);
        Debug.Log(_info.collegueIndex.ToString() + " DeviceLevel : " + userBaseProperties.collegueInfos[(int)_collegueIndex].deviceLevel);
        
        //유저의 정보에 따라 UI 갱신 
    }

    #endregion

}
