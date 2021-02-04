using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceLab : BaseFlexPlace
{
    public ResultPanel resultPanel;
    private void OnEnable()
    {
        //Init();
    }
    //protected override void Init()
    //{
    //    resultPanel.Init();
    //}
    public void OnClickButton_Donate()
    {
        User user = GameManager.Instance.user;
   
    }
}
