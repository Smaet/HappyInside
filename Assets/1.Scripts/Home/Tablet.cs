/*
  2021.02.05 Created 
  Tablet.cs   
  
  기능
    1. 모든 정보를 종합해서 볼수있는 테블릿 역할
        - 각각의 기능은 탭으로 나누어 져있음.
        - 통계등 실시간으로 반영이 되어야 하는 부분도 존재.
        - 
    2. 기능 종류 
        - 상점, 인벤토리, 환경설정, 퀘스트, 업적, 조직레벨?

    3. 기본 기능 탭으로 나누기
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Doozy.Engine.UI;

public class Tablet : MonoBehaviour
{
    [SerializeField]
    private UIView uiView;

    [SerializeField]
    private TabletTap[] tabletTaps;
    [SerializeField]
    private UIButton[] uiButton_TabletTaps;

    
    public void ShowTablet()
    {
        uiView.Show();

        HomeManager.Instance.topUIManager.ClickTabletTab_MyInfo();

        OpenTabletTap(0);

    }

    public void CloseTablet()
    {
        uiView.Hide();
    }

    public void Init()
    {

        for (int i=0; i < tabletTaps.Length; i++)
        {
            tabletTaps[i].Init();
        }
        //내 정보
        uiButton_TabletTaps[0].Button.onClick.AddListener(() => ShowTap(0));
        uiButton_TabletTaps[1].Button.onClick.AddListener(() => ShowTap(1));
    }

    public void OpenTabletTap(int _index)
    {
        print("Tablet_" + string.Format("{0:00}", _index));
        ShowTap(_index);
    }

    //해당 인덱스 말고 나머지 는 닫기
    public void ShowTap(int _index)
    {
        for(int i=0; i < tabletTaps.Length; i++)
        {
            if(i == _index)
            {
                tabletTaps[_index].ShowTap();
            }
            else
            {
                tabletTaps[_index].HideTap();
            }
        }
    }

    public void SetCurrentAssetStatus_Slider(long _manipulateMoney)
    {
        //전체 재산에서 
        //double manipulatePercent = (double)_manipulateMoney / (double)GameManager.Instance.user.userBaseProperties.startMoney;
        //CurrentAssetStatus_Slider.value =(float) manipulatePercent;
    }

   

    public void SetGarndFaterAssetInfo(long _startMoney, long _manipulatedMoney)
    {
        //억 표시
        long Billion = (_startMoney - _manipulatedMoney);
        string Billion_str = "";
        string TenThousand_str = "";

        var Bill = (Billion % 100000000000) / 100000000;
        var Thou = (Billion % 100000000) / 10000;

        if (Billion>= 100000000)
        {
            Billion_str = string.Format("{0}억", (Billion % 100000000000) / 100000000);
        }
        if(Billion >= 10000)
        {
            TenThousand_str = string.Format("{0}만원", (Billion % 100000000) / 10000);
        }

    
        
    }
}
