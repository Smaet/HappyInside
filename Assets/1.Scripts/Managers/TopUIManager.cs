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
