using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopUIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI hour_TMP;
    [SerializeField]
    private TextMeshProUGUI days_TMP;
    [SerializeField]
    private TextMeshProUGUI pinkChip_TMP;
    [SerializeField]
    private TextMeshProUGUI crystal_TMP;
    [SerializeField]
    private TextMeshProUGUI notice_TMP;

    public Tablet tablet;

    //Event
    public event Action OnClickTabletTab_01;
    public event Action OnClickTabletTab_02;
    public event Action OnClickTabletTab_03;
    public event Action OnClickTabletTab_04;
    public event Action OnClickTabletTab_05;
    public event Action OnClickTabletTab_06;


    public void ClickTabletTab_01()
    {
        if (OnClickTabletTab_01 != null)
        {
            OnClickTabletTab_01();
        }
    }
    public void ClickTabletTab_02()
    {
        if (OnClickTabletTab_02 != null)
        {
            OnClickTabletTab_02();
        }
    }


    public void ClickTabletTab_03()
    {
        if (OnClickTabletTab_03 != null)
        {
            OnClickTabletTab_03();
        }
    }

    public void ClickTabletTab_04()
    {
        if (OnClickTabletTab_04 != null)
        {
            OnClickTabletTab_04();
        }
    }

    public void ClickTabletTab_05()
    {
        if (OnClickTabletTab_05 != null)
        {
            OnClickTabletTab_05();
        }
    }

    public void ClickTabletTab_06()
    {
        if (OnClickTabletTab_06 != null)
        {
            OnClickTabletTab_06();
        }
    }





    public void Init()
    {
        hour_TMP.text = "00:00";
        days_TMP.text = "1Days";
    }
    public void SetHour(int _hour)
    {
        hour_TMP.text = string.Format("{0:00}:00 ", _hour);
    }

    public void SetDays(int _days)
    {
        days_TMP.text = string.Format("{0}일 ", _days);
    }

    public void SetPinkChip(float _pinkChip)
    {
        pinkChip_TMP.text = string.Format("{0} ", _pinkChip);
    }

    public void SetCrystal(float _crystal)
    {
        crystal_TMP.text = string.Format("{0} ", _crystal);
    }

    public void SetNotice(string _notice)
    {
        notice_TMP.text = string.Format("{0} ", _notice);
    }
}
