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
   - 동료

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public string nickName { private set; get; }        //닉네임
    public int crystal { private set; get; }            //크리스탈
    public int startMoney { private set; get; }         //시작재산 (고정된 재산)
    public int money {private set;get;}                 //현재 가지고 있는 돈 (실제 재산)
    public int manipulatedMoney{private set;get;}       //현재 조작된 돈
    public int resultMoney{private set;get;}            //현재 잔액
    public int recentChangeMoney{private set;get;}      //최근 변화된 돈
    public float gameTime{private set;get;}             //게임 시간
    public float daysElapsed{private set;get;}          //경과된 일수
    public float doubt{private set;get;}                //의심도
    public float blackCoin{private set;get;}            //블랙 코인

    //MiniGame
    //백화점
    public int accumulatedConsumption { private set; get; } //누적 소비액

   

    //아지트(추가예정)
    //동료(추가예정)

}
