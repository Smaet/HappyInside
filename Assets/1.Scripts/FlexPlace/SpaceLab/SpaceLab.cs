using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceLab : BaseFlexPlace
{
    public ResultPanel resultPanel;
    private void OnEnable()
    {
        Init();
    }
    protected override void Init()
    {
        resultPanel.Init();
    }
    public void OnClickButton_Donate()
    {
        User user = GameManager.Instance.user;
   
        user.SetUserInfo(ChangeableUserProperties.FLEXCONSUMPTION, user.userBaseProperties.donateMoney);
        user.SetUserInfo(ChangeableUserProperties.CONSUMPTION, user.userBaseProperties.donateMoney);

        Debug.Log("기부 한 금액 : " + GameManager.Instance.GetMoneyFormat(user.userBaseProperties.donateMoney));

        resultPanel.OnResultPanel_SpaceLab(GameManager.Instance.GetMoneyFormat(user.userBaseProperties.donateMoney));
    }
}
