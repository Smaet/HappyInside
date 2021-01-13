using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPage : BasePopUp
{
    public override void Init(Canvas _canvas)
    {
        base.Init(_canvas);
    }

    public override void OpenPopUp()
    {
        base.OpenPopUp();
        HomeManager.Instance.PushUIView(this);
        Debug.Log("UserPage Open!!");
    }

    public override void ClosePopUp()
    {
        base.ClosePopUp();

        Debug.Log("UserPage Close!!");
    }

}
