/*
  2021.01.05 Created 
  AgitManager.cs   
  
  
  기능
  1. 아지트 A,B의 출입 기능
 //각각으로 구현하는 걸 고려중... 게임 시간에 따라 바뀌어야 해서 계속해서 각각을 갱신하는 방식으로...?
  2. 동료 창                          
  3. 동료 아이템 창
  4. 동료 디바이스 창

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public enum Agit_Index
{
    None = -1,
    AGIT_A = 0,
    AGIT_B,
}


public class AgitManager : MonoBehaviour
{ 
    [Header("Agits")]
    [SerializeField]
    public Agit_A agit_A;
    [SerializeField]
    public Agit_B agit_B;


    public CollegueView collegueView;
    public CollegueItemView collegueItemView;
    public CollegueDeviceView collegueDeviceView;



    public void Init()
    {
        //각각의 동료들의 창이나 아이템창에 대한 이벤트 초기화
        collegueView.Init();
        collegueItemView.Init();
    }

}
