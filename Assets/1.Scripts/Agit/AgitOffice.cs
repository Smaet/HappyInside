using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgitOffice : BaseAgit
{
    //아지트에 보여지는 동료 패널 정보 (레벨에 따른 동료의 변화 등등..)
    public ColleguePanel[] colleguePanels;

    //아지트 A
    public override void Init()
    {
        base.Init();
    }
    private void OnEnable()
    {
        Init();
        UpdateAgitACollegueUI(CollegueIndex.Dare);
    }

    
    public void UpdateAgitACollegueUI(CollegueIndex _index)
    {
        switch(_index)
        {
            case CollegueIndex.Dare:
                colleguePanels[0].SetUI(GameManager.Instance.user.userBaseProperties.collegueInfos[(int)_index]);

                break;
        }
    }
}
