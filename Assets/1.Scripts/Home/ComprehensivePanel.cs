using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComprehensivePanel : MonoBehaviour
{
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
    }

    public void SetCurrentAssetStatus_Slider(long _manipulateMoney)
    {
        //전체 재산에서 
        double manipulatePercent = (double)_manipulateMoney / (double)GameManager.Instance.user.userBaseProperties.startMoney;
        CurrentAssetStatus_Slider.value =(float) manipulatePercent;
    }

    public void SetCurrentDoubtStatus_Slider(long _consumption, long  _manipulateMoney)
    {
        double doubt = (double)_consumption / (double)_manipulateMoney;
        CurrentDoubtStatus_Slider.value = (float)doubt;
        Debug.Log("현재 의심도 : " + doubt * 100 + "%");
        GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.DOUBT, (float)doubt);
    }

    public void SetCurrentDoubtStatus_Slider(float _doubt)
    {

        CurrentDoubtStatus_Slider.value += _doubt * 0.01f;
        Debug.Log("현재 의심도 : " + _doubt + "%");
        GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.DOUBT, _doubt);
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
