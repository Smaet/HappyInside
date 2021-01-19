using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserAttainment : BasePopUp 
{
    public override void Init(Canvas _canvas)
    {
        base.Init(_canvas);
    }
    public override void OpenPopUp()
    {
        base.OpenPopUp();
       
        Debug.Log("UserAttainment Open!!");
    }

    public override void ClosePopUp()
    {
        base.ClosePopUp();

        Debug.Log("UserAttainment Close!!");
    }

}
