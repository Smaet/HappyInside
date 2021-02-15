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


public enum GrandFatherAngerIndex
{
    VALUE01 = 0,
}


public enum CollegueIndex
{
    Dare = 0,
    Lovely,
    Soso,
    Happy,
    Sad,
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
    BLACKCHIP,
    DESSERT,
    CURRENTAMOUNT,
    GRANDFATHERANGER,
    TERROR,
    GAMEHOUR,
    DAYSELASPSE,
    
    HACKER,
    BUFF,
}

//유저의 기본 변수들
[Serializable]
public class UserBaseProperties
{
    public string nickName;                             //닉네임
    public int crystal;                                 //크리스탈
    public double blackChip;                            // 블랙칩    
                                                        // - 공장에서 생산품을 생산화는 화폐    
                                                        // - 단위 : 1개 ~ 1억개 
    public int dessert;                                 // 디저트 - 동료들이 섭취를 해야 돌아감.


    public long currentAmount;                          //재산은 두가지로
                                                        //현재 재산과
                                                        //테러로 부과되는 피해 금액
                                                        //현재 금액

    public Terror terror;                               //테러에 관련된 정보 모음 클래스
    public GrandFatherAnger grandFatherAnger;           //할아버지 분노
    public int gameHour;                                //게임 시간
    public int daysElapsed;                             //경과된 일수



    //인벤토리
    public List<HappyRichItem> happyRichItems;
    //동료들
    public CollegueInfo[] collegueInfos;

    public UserBaseProperties()
    {
        collegueInfos = new CollegueInfo[5];
        for(int i=0; i < collegueInfos.Length; i++)
        {
            collegueInfos[i] = new CollegueInfo();
        }

        buffs = new List<Buff>();
    }
    public List<Buff> buffs;
}

/****************************************
 * 테러 활동에 대한 정보 클래스
 * - 테러 피해 금액에 따른 등급
 * - 테러 피해 금액
 *  - 금액에 따라서 홈 화면이 업데이트
    
   ****************************************/

[Serializable]
public class Terror
{
    public int terrorRanking;
    public long damageAmount;       //추후에 플렉스 공간에 따른 피해액으로 나뉠수 있음 (통계 때문에)

    public void UpdateTerror(long _damageAmount)
    {
        damageAmount = _damageAmount;
    }
}

/****************************************
 * 할아버지 분노 관련 클래스
 * 종료 : 정상, 소노, 중노, 대노, 극대노
 * 
   ****************************************/

[Serializable]
public class GrandFatherAnger   
{
    public double curAnger;                         //현재 분노
    public double specificEventAngerValue01;        //특정이벤트로 인한 값  (임시)
    //public double GrandFatherDoubt;               //할아버지 집에 갔을때 영향을 받은 값.


    public void UpdateAnger()
    {
        curAnger = curAnger + specificEventAngerValue01;
        if(curAnger <= 0)
        {
            curAnger = 0;
        }
    }
}

/****************************************
 * 현재 버프 종류
 * 대노 : 시간당 지속적으로 1% 증가
 * 
   ****************************************/

[Serializable]
public class Buff
{
    public int icon;                        //버프창 아이콘 Index
    public BuffIndex buffIndex;             //버프 구분 인덱스
    public bool isActive;                   //활성화 인지 아닌지
    public bool isGood;                     //버프가 이로운 효과 인지 아닌지
    public bool isBuffed;
    public bool isReset;                    //리셋을 하는지 아닌지
    public int totalContinueTime;           //전체 지속시간  --> 고정 값으로 해도 될 수도 있지만 추후에 지속 시간이 늘어날수도 있어서 변수에 포함
    public int remainTime;                  //남은시간
    public float effect_Doubt_Plus;         //의심에 사용되는 효과 수치 (이로운 수치)
    public float effect_Doubt_Minus;        //의심에 사용되는 효과 수치 (이롭지 않은 수치)
}


/****************************************
 * 동료들 정보 클래스
 * 동료 : 다레, 럽리, 쌔드, 햅삐, 쏘쏘
 * 
   ****************************************/
[Serializable]
public class CollegueInfo
{
    public bool isActive;

    public CollegueIndex collegueIndex; //동료의 인덱스

    public int Level;               //동료의 레벨
    public int itemLevel;           //아이템 레벨
    public int deviceLevel;         //디바이스 레벨
    public int levelUpTime;         
    public int leftLevelUpTime;     

    public CollegueBasicSkill collegueBasicSkill;
    public ColleguePassiveSkill[] colleguePassiveSkills;
    public CollegueItem collegueItem;
}

[Serializable]
public class CollegueBasicSkill
{
    public int hour;
    public int day;
    public long money;
    public double chance;
    public int count;
}


[Serializable]
public class ColleguePassiveSkill
{
    public bool isActive;       //활성화가 됬는지? UI 갱신
    public bool isApply;        //적용이 됬는지?  효과적용 떄문에
    public int hour;
    public int day;
    public long money;
    public double chance;
}

[Serializable]
public class CollegueItem
{
    public bool isActive;
    public double chance;
    public int hour;
    public int count;
}


[Serializable]
public class CollegueDevice
{
    public bool isActive;
}

[Serializable]
public class HappyRichItem
{
    public int icon;            //아이콘
    public int description;     //설명 - 내장되어있는 설명을 인덱스로 불러오는 형식으로
    public int count;           //아이템 개수
    public int timeTaken;       //걸리는 시간
}




[Serializable]
public class User
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
        userBaseProperties.blackChip = _userInfo.userBaseProperties.blackChip;
        userBaseProperties.currentAmount = _userInfo.userBaseProperties.currentAmount;

        userBaseProperties.terror = _userInfo.userBaseProperties.terror;
        userBaseProperties.grandFatherAnger = _userInfo.userBaseProperties.grandFatherAnger;
        userBaseProperties.gameHour = _userInfo.userBaseProperties.gameHour;
        userBaseProperties.daysElapsed = _userInfo.userBaseProperties.daysElapsed;
   
     
    
        userBaseProperties.collegueInfos = _userInfo.userBaseProperties.collegueInfos;
        userBaseProperties.buffs = _userInfo.userBaseProperties.buffs;

        //Debug.Log("NickName : " + userBaseProperties.nickName);
        //Debug.Log("crystal : " + userBaseProperties.crystal);
        //Debug.Log("startMoney : " + userBaseProperties.startMoney);
        //Debug.Log("money : " + userBaseProperties.ConsumptionMoney);
        //Debug.Log("manipulatedMoney : " + userBaseProperties.manipulatedMoney);
        //Debug.Log("NickName : " + userBaseProperties.resultMoney);
        //Debug.Log("resultMoney : " + userBaseProperties.nickName);
        //Debug.Log("recentChangeMoney : " + userBaseProperties.recentChangeMoney);
        //Debug.Log("gameHour : " + userBaseProperties.gameHour);
        //Debug.Log("daysElapsed : " + userBaseProperties.daysElapsed);
        //Debug.Log("doubt : " + userBaseProperties.doubt);
        //Debug.Log("blackCoin : " + userBaseProperties.pinkChip);
        //Debug.Log("FLEXConsumption : " + userBaseProperties.FlexConsumption);

        //Debug.Log("<color=red>해커의 현재 레벨 : </color>" + userBaseProperties.collegueInfos[0].Level);
        //Debug.Log("<color=red>해커의 현재 아이템 레벨 : </color>" + userBaseProperties.collegueInfos[0].itemLevel);
        //Debug.Log("<color=red>해커의 현재 디바이스 레벨 </color> " + userBaseProperties.collegueInfos[0].deviceLevel);

        //Debug.Log("<color=blue>기계공학자의 현재 레벨 : </color>" + userBaseProperties.collegueInfos[0].Level);
        //Debug.Log("<color=blue>기계공학자의 현재 아이템 레벨 : </color>" + userBaseProperties.collegueInfos[0].itemLevel);
        //Debug.Log("<color=blue>기계공학자의 현재 디바이스 레벨 : </color>" + userBaseProperties.collegueInfos[0].deviceLevel);

        //Debug.Log("<color=yellow>화학자의 현재 레벨 : </color>" + userBaseProperties.collegueInfos[0].Level);
        //Debug.Log("<color=yellow>화학자의 현재 아이템 레벨 : </color>" + userBaseProperties.collegueInfos[0].itemLevel);
        //Debug.Log("<color=yellow>화학자의 현재 디바이스 레벨 : </color>" + userBaseProperties.collegueInfos[0].deviceLevel);

        //Debug.Log("<color=purple>요리사의 현재 레벨 : </color>" + userBaseProperties.collegueInfos[0].Level);
        //Debug.Log("<color=purple>요리사의 현재 아이템 레벨 : </color>" + userBaseProperties.collegueInfos[0].itemLevel);
        //Debug.Log("<color=purple>요리사의 현재 디바이스 레벨 : </color>" + userBaseProperties.collegueInfos[0].deviceLevel);

        //Debug.Log("<color=green>트레이더의 현재 레벨 : </color>" + userBaseProperties.collegueInfos[0].Level);
        //Debug.Log("<color=green>트레이더의 현재 아이템 레벨 : </color>" + userBaseProperties.collegueInfos[0].itemLevel);
        //Debug.Log("<color=green>트레이더의 현재 디바이스 레벨 : </color>" + userBaseProperties.collegueInfos[0].deviceLevel);

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
                //Debug.Log("현재 시간 : " + userBaseProperties.gameHour);

                break;
            case ChangeableUserProperties.DAYSELASPSE:
                userBaseProperties.daysElapsed += _value;
                HomeManager.Instance.topUIManager.SetDays(userBaseProperties.daysElapsed);
                Debug.Log("현재 경과된 날짜  : " + userBaseProperties.daysElapsed);
                break;

            case ChangeableUserProperties.DESSERT:
                userBaseProperties.dessert += _value;
                //UI 갱신 예정

                Debug.Log("현재 디저트 갯수 : " + userBaseProperties.dessert);


                break;

        }

        GameManager.Instance.SaveUserData();
    }

    public void SetUserInfo(ChangeableUserProperties _changeableIndex, long _value)
    {

        switch (_changeableIndex)
        {
            case ChangeableUserProperties.BLACKCHIP:
                userBaseProperties.blackChip += _value;

                HomeManager.Instance.topUIManager.SetPinkChip(userBaseProperties.blackChip);
                Debug.Log("현재 X코인 개수 : " + userBaseProperties.blackChip);
                break;
            case ChangeableUserProperties.CURRENTAMOUNT:
                userBaseProperties.currentAmount += _value;

                HomeManager.Instance.topUIManager.SetPinkChip(userBaseProperties.blackChip);
                Debug.Log("현재 X코인 개수 : " + userBaseProperties.blackChip);
                break;
        }

        GameManager.Instance.SaveUserData();
    }

    public void SetUserInfo(GrandFatherAngerIndex _AngerIndex, double _value)
    {
        switch (_AngerIndex)
        {

            case GrandFatherAngerIndex.VALUE01:

                //기본 공식 적용
                userBaseProperties.grandFatherAnger.specificEventAngerValue01 = _value;
                Debug.Log("현재 VALUE 01로 인한 분노 증가량 :  " + userBaseProperties.grandFatherAnger.specificEventAngerValue01 + "%");

                //의심도 갱신
                userBaseProperties.grandFatherAnger.UpdateAnger();

                //UI 갱신
                //HomeManager.Instance.comprehensivePanel.SetCurrentDoubtStatus_Slider(userBaseProperties.doubt.curDoubt);
               

                break;

     
        }



    }


    public void SetUserInfo(CollegueIndex _collegueIndex, CollegueInfo _info)
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


        switch (_buffIndex)
        {
            case BuffIndex.GRANDFATHER:

                if (userBaseProperties.buffs[(int)_buffIndex].isReset == false)
                {
                    if (userBaseProperties.buffs[(int)_buffIndex].isBuffed == false)
                    {
                        if (userBaseProperties.buffs[(int)_buffIndex].isGood)
                        {
                            SetUserInfo(GrandFatherAngerIndex.VALUE01, userBaseProperties.buffs[(int)_buffIndex].effect_Doubt_Plus);
                        }
                        else
                        {
                            SetUserInfo(GrandFatherAngerIndex.VALUE01, userBaseProperties.buffs[(int)_buffIndex].effect_Doubt_Minus);
                        }

                        userBaseProperties.buffs[(int)_buffIndex].isBuffed = true;
                    }
                }
                else
                {
                    SetUserInfo(GrandFatherAngerIndex.VALUE01, 0);
                }

             

            
       
                GameManager.Instance.SaveUserData();

                break;
        }
        
       
    }

    #endregion


}
