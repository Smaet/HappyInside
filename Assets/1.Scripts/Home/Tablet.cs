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
    private TabletTap[] tabletTaps;
    [SerializeField]
    private UIButton[] uiButton_TabletTaps;

    [SerializeField]
    private Slider CurrentAssetStatus_Slider;
    [SerializeField]
    private Slider CurrentDoubtStatus_Slider;
    [SerializeField]
    private TextMeshProUGUI grandFatherAsset_TMP;

    public void Init()
    {
        CurrentAssetStatus_Slider.value = 0;
        CurrentDoubtStatus_Slider.value = 0;
        grandFatherAsset_TMP.text = "";

        for(int i=0; i < uiButton_TabletTaps.Length; i++)
        {
            uiButton_TabletTaps[i].Button.onClick.AddListener(() => OpenTabletTap(i));
        }
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
                tabletTaps[_index].ShowTabletTap();
            }
            else
            {
                tabletTaps[_index].HideTabletTap();
            }
        }
    }

    public void SetCurrentAssetStatus_Slider(long _manipulateMoney)
    {
        //전체 재산에서 
        //double manipulatePercent = (double)_manipulateMoney / (double)GameManager.Instance.user.userBaseProperties.startMoney;
        //CurrentAssetStatus_Slider.value =(float) manipulatePercent;
    }

    public void SetCurrentDoubtStatus_Slider(long _consumption, long  _manipulateMoney)
    {
        double doubt = (double)_consumption / (double)_manipulateMoney;
        CurrentDoubtStatus_Slider.value = (float)doubt;
        Debug.Log("현재 의심도 : " + doubt * 100 + "%");
        //GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.DOUBT, (float)doubt);
    }

    public void SetCurrentDoubtStatus_Slider(float _doubt)
    {

        CurrentDoubtStatus_Slider.value += _doubt * 0.01f;
        Debug.Log("현재 의심도 : " + _doubt + "%");
       // GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.DOUBT, _doubt);
    }

    public void SetCurrentDoubtStatus_Slider(double _doubt)
    {

        CurrentDoubtStatus_Slider.value = (float)_doubt * 0.01f;
        Debug.Log("현재 의심도 : " + _doubt + "%");
        // GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.DOUBT, _doubt);
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

        if(Thou != 0)
        {
            grandFatherAsset_TMP.text = "총 할아버지 재산 : " + Billion_str;
        }
        else
        {
            grandFatherAsset_TMP.text = "총 할아버지 재산 : " + Billion_str + " " + TenThousand_str;
        }
        
    }
}
