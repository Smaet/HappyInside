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
using System;

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
    public AgitOffice agitOffice;
    [SerializeField]
    public Agit_B agit_B;


    public CollegueView[] collegueViews;
    public CollegueItemView collegueItemView;
    public CollegueDeviceView[] collegueDeviceView;

    //아지트에 있는 모든 이벤트들 관리
    public event Action<CollegueIndex> OnClickCollegueButton_Dare;      //아지트에서 동료 버튼을 누를때
    public event Action<CollegueIndex> OnClickCollegueButton_Lovely;   
    public event Action<CollegueIndex> OnClickCollegueButton_Soso;     
    public event Action<CollegueIndex> OnClickCollegueButton_Happy;    
    public event Action<CollegueIndex> OnClickCollegueButton_Sad;

    public event Action OnClickCollegueItemButton;

    public event Action<CollegueIndex> OnClickCollegueDevice_Happy;
    public event Action<CollegueIndex> OnClickCollegueDevice_Sad;

    public void Init()
    {
        //각각의 동료들의 창이나 아이템창에 대한 이벤트 초기화
        for(int i=0; i < collegueViews.Length; i++)
        {
            collegueViews[i].Init();
        }

        //collegueView.Init();
        //collegueItemView.Init();
    }

    public void ClickCollegueButton(CollegueIndex _collegueIndex)
    {
        switch(_collegueIndex)
        {
            case CollegueIndex.Dare:
                if (OnClickCollegueButton_Dare != null)
                {
                    OnClickCollegueButton_Dare(_collegueIndex);
                }
                break;
            case CollegueIndex.Lovely:
                if (OnClickCollegueButton_Lovely != null)
                {
                    OnClickCollegueButton_Lovely(_collegueIndex);
                }
                break;
            case CollegueIndex.Soso:
                if (OnClickCollegueButton_Soso != null)
                {
                    OnClickCollegueButton_Soso(_collegueIndex);
                }
                break;
            case CollegueIndex.Happy:
                if (OnClickCollegueButton_Happy != null)
                {
                    OnClickCollegueButton_Happy(_collegueIndex);
                }
                break;
            case CollegueIndex.Sad:
                if (OnClickCollegueButton_Sad != null)
                {
                    OnClickCollegueButton_Sad(_collegueIndex);
                }
                break;
        }
    }

    public void ClickCollegueItemButton()
    {
        if (OnClickCollegueItemButton != null)
        {
            OnClickCollegueItemButton();
        }
    }

}
