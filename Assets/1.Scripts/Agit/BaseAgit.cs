using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class BaseAgit : MonoBehaviour
{
    public Agit_Index aigtIndex;

    [SerializeField]
    private bool isInit = false;

    [SerializeField]
    private Button[] colleagueButtons;      //동료 버튼들
    [SerializeField]
    private Button[] colleagueItemButtons;  //동료 아이템 버튼들

    // Start is called before the first frame update
    protected virtual void Init()
    {
      
      
    }
}
