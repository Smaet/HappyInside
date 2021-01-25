using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agit_A : BaseAgit
{
    public override void OpenAgit()
    {
        Init();
        SetCollegueUI(CollegueIndex.HACKER);


    }

    public ColleguePanel[] colleguePanels;


    public void OnUseMouse()
    {
        Debug.Log("대화창 클릭!");
    }


    public void OnCollegueButtonClick()
    {

    }

    public void SetCollegueUI(CollegueIndex _index)
    {
        switch(_index)
        {
            case CollegueIndex.HACKER:
                colleguePanels[0].SetUI(GameManager.Instance.user.userBaseProperties.collegueInfos[(int)_index]);

                break;
        }
    }
}
