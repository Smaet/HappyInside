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

public enum BuffIndex
{
    NONE = -1,
    GRANDFATHER = 0,
}




public enum ChangeableUserProperties
{
    NONE = 0,
    CRYSTAL = 1,
    STARTMONEY,
    CONSUMPTION,
    MANIPULATEMONEY,
    RESULTMONEY,
    RECENTCHANGEMONEY,
    GAMEHOUR,
    DAYSELASPSE,
    DOUBT,
    PINKCHIP,
    FLEXCONSUMPTION,
    HACKER,
    BUFF,


}


//유저의 기본 변수들
[Serializable]
public class UserBaseProperties
{
    public string nickName;                 //닉네임
    public int crystal;                     //크리스탈
    public long startMoney;                 //시작재산 (고정된 재산)
    public long ConsumptionMoney;           //현재 가지고 있는 돈 (실제 재산)
    public long manipulatedMoney;           //현재 조작된 돈
    public long resultMoney;                //현재 잔액
    public long recentChangeMoney;          //최근 변화된 돈
    public long FlexConsumption;            //누적 소비액  --> 해당 금액에 따른 난이도 변화
    public long donateMoney;                 //기부하는 돈
    public int gameHour;                  //게임 시간
    public int daysElapsed;               //경과된 일수
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

        buffs = new Buff[1];
        buffs[0] = new Buff();

    }

    public Buff[] buffs;
   

}

[Serializable]
public class Buff
{
    public bool isActive;           //활성화 인지 아닌지
    public bool isRunning;          //활성화 인지 아닌지
    public bool isPlus;             //플러스 효과인지 마이너스 효과인지
    public int continueTime;        //지속시간
    public float effect_Doubt;      //의심에 사용되는 효과 수치
}



[Serializable]
public class collegueInfo
{
    public bool isActive;

    public CollegueIndex collegueIndex; //동료의 인덱스

    public int Level;               //동료의 레벨
    public int itemLevel;           //아이템 레벨
    public int deviceLevel;         //디바이스 레벨
    public int levelUpTime;         
    public int leftLevelUpTime;     

    public collegueBasicSkill collegueBasicSkill;
    public colleguePassiveSkill[] colleguePassiveSkills;
    public collegueItem collegueItem;
    public collegueDevice collegueDevice;
}

[Serializable]
public class collegueBasicSkill
{
    public float hour;
    public int day;
    public long money;
    public float chance;
}

[Serializable]
public class colleguePassiveSkill
{
    public bool isActive;
    public float hour;
    public int day;
    public long money;
    public float chance;
}

[Serializable]
public class collegueItem
{
    public bool isActive;
    public float chance;
}


[Serializable]
public class collegueDevice
{
    public bool isActive;
}


public class User : MonoBehaviour
{
    public bool isFirst;

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
        userBaseProperties.ConsumptionMoney = _userInfo.userBaseProperties.ConsumptionMoney;
        userBaseProperties.manipulatedMoney = _userInfo.userBaseProperties.manipulatedMoney;
        userBaseProperties.resultMoney = _userInfo.userBaseProperties.resultMoney;
        userBaseProperties.recentChangeMoney = _userInfo.userBaseProperties.recentChangeMoney;
        userBaseProperties.gameHour = _userInfo.userBaseProperties.gameHour;
        userBaseProperties.daysElapsed = _userInfo.userBaseProperties.daysElapsed;
        userBaseProperties.doubt = _userInfo.userBaseProperties.doubt;
        userBaseProperties.pinkChip = _userInfo.userBaseProperties.pinkChip;
        userBaseProperties.FlexConsumption = _userInfo.userBaseProperties.FlexConsumption;
        userBaseProperties.collegueInfos = _userInfo.userBaseProperties.collegueInfos;
        userBaseProperties.buffs = _userInfo.userBaseProperties.buffs;


        Debug.Log("NickName : " + userBaseProperties.nickName);
        Debug.Log("crystal : " + userBaseProperties.crystal);
        Debug.Log("startMoney : " + userBaseProperties.startMoney);
        Debug.Log("money : " + userBaseProperties.ConsumptionMoney);
        Debug.Log("manipulatedMoney : " + userBaseProperties.manipulatedMoney);
        Debug.Log("NickName : " + userBaseProperties.resultMoney);
        Debug.Log("resultMoney : " + userBaseProperties.nickName);
        Debug.Log("recentChangeMoney : " + userBaseProperties.recentChangeMoney);
        Debug.Log("gameHour : " + userBaseProperties.gameHour);
        Debug.Log("daysElapsed : " + userBaseProperties.daysElapsed);
        Debug.Log("doubt : " + userBaseProperties.doubt);
        Debug.Log("blackCoin : " + userBaseProperties.pinkChip);
        Debug.Log("FLEXConsumption : " + userBaseProperties.FlexConsumption);

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

                HomeManager.Instance.topUIManager.SetCrystal(userBaseProperties.crystal);
                Debug.Log("현재 크리스탈 : " + userBaseProperties.crystal);

                
                break;
            case ChangeableUserProperties.GAMEHOUR:
                if(_value != 0)
                {
                    userBaseProperties.gameHour += _value;
                }
                else
                {
                    userBaseProperties.gameHour = _value;
                }
              

                HomeManager.Instance.topUIManager.SetHour(userBaseProperties.gameHour);
                Debug.Log("현재 시간 : " + userBaseProperties.gameHour);

                break;
            case ChangeableUserProperties.DAYSELASPSE:
                userBaseProperties.daysElapsed += _value;
                HomeManager.Instance.topUIManager.SetDays(userBaseProperties.daysElapsed);
                Debug.Log("현재 경과된 날짜  : " + userBaseProperties.daysElapsed);

      
                break;

        }

        GameManager.Instance.SaveUserData();
    }

    public void SetUserInfo(ChangeableUserProperties _changeableIndex, long _value)
    {

        switch (_changeableIndex)
        {
            case ChangeableUserProperties.STARTMONEY:
                userBaseProperties.startMoney += _value;
                HomeManager.Instance.comprehensivePanel.SetGarndFaterAssetInfo(userBaseProperties.startMoney, userBaseProperties.manipulatedMoney);
                Debug.Log("현재 시작금액 : " + userBaseProperties.startMoney);
                break;
            case ChangeableUserProperties.CONSUMPTION:
                userBaseProperties.ConsumptionMoney += _value;

                //현재 의심도
                HomeManager.Instance.comprehensivePanel.SetCurrentDoubtStatus_Slider(userBaseProperties.ConsumptionMoney, userBaseProperties.manipulatedMoney);
                Debug.Log("현재 까지 총 소비금액 : " + userBaseProperties.ConsumptionMoney);
                break;

            case ChangeableUserProperties.MANIPULATEMONEY:
                userBaseProperties.manipulatedMoney += _value;

                //해커의 레벨에 따른 추가적인 계산
                double additionalPercent = 0;
                if(userBaseProperties.collegueInfos[(int)CollegueIndex.HACKER].colleguePassiveSkills[0].isActive)
                {
                    additionalPercent = userBaseProperties.collegueInfos[(int)CollegueIndex.HACKER].colleguePassiveSkills[0].chance;
                }
                if (userBaseProperties.collegueInfos[(int)CollegueIndex.HACKER].colleguePassiveSkills[1].isActive)
                {
                    additionalPercent = userBaseProperties.collegueInfos[(int)CollegueIndex.HACKER].colleguePassiveSkills[1].chance;
                }
                if (userBaseProperties.collegueInfos[(int)CollegueIndex.HACKER].colleguePassiveSkills[2].isActive)
                {
                    additionalPercent = userBaseProperties.collegueInfos[(int)CollegueIndex.HACKER].colleguePassiveSkills[2].chance;
                }
                
                if (additionalPercent != 0)
                {
                    additionalPercent = additionalPercent * 0.01;
                    //기존의 조작된 머니 + 
                    userBaseProperties.manipulatedMoney = userBaseProperties.manipulatedMoney + (long)(userBaseProperties.manipulatedMoney * additionalPercent);
                }
                //////////////////////////////////////////////////////////////////////////////////////////////////////

                //현재 의심도
                HomeManager.Instance.comprehensivePanel.SetCurrentDoubtStatus_Slider(userBaseProperties.ConsumptionMoney, userBaseProperties.manipulatedMoney);
               
                //현재 재산 정보
                HomeManager.Instance.comprehensivePanel.SetCurrentAssetStatus_Slider(userBaseProperties.manipulatedMoney);
                
                //현재 할아버지의 총 재산에 대한 정보
                HomeManager.Instance.comprehensivePanel.SetGarndFaterAssetInfo(userBaseProperties.startMoney, userBaseProperties.manipulatedMoney);
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
            case ChangeableUserProperties.FLEXCONSUMPTION:
                userBaseProperties.FlexConsumption += _value;
                Debug.Log("현재 플렉스 소비 금액 : " + userBaseProperties.FlexConsumption);
                break;



        }

        GameManager.Instance.SaveUserData();
    }

    public void SetUserInfo(ChangeableUserProperties _changeableIndex, float _value)
    {

        switch (_changeableIndex)
        {
           
            case ChangeableUserProperties.DOUBT:
                userBaseProperties.doubt += _value;
                Debug.Log("현재 의심도 : " + userBaseProperties.doubt);
            
                //각종 의심도 관련된 버프 On/off;
                if (userBaseProperties.buffs[(int)BuffIndex.GRANDFATHER].isActive && userBaseProperties.buffs[(int)BuffIndex.GRANDFATHER].isRunning == false)
                {
                    userBaseProperties.buffs[(int)BuffIndex.GRANDFATHER].isRunning = true;
                    HomeManager.Instance.timeManager.StartGrandFatherBuff();
                }



                break;
            case ChangeableUserProperties.PINKCHIP:
                userBaseProperties.pinkChip += _value;
                Debug.Log("현재 핑크 칩 : " + userBaseProperties.pinkChip);
                break;

        }

        GameManager.Instance.SaveUserData();
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

        GameManager.Instance.SaveUserData();
    }

    public void SetUserInfo(BuffIndex _buffIndex, Buff _info)
    {

        userBaseProperties.buffs[(int)_buffIndex] = _info;


        GameManager.Instance.SaveUserData();
    }

    #endregion


}
