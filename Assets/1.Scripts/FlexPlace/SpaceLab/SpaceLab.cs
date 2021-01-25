using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceLab : MonoBehaviour
{
    public void OnClickButton_Donate()
    {
        User user = GameManager.Instance.user;
        Debug.Log("기부 한 금액 : " + GameManager.Instance.GetMoneyFormat(user.userBaseProperties.donateMoney));
        user.SetUserInfo(ChangeableUserProperties.FLEXCONSUMPTION, user.userBaseProperties.donateMoney);
    }
}
