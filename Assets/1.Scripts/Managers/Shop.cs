using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : BasePopUp
{
    // Start is called before the first frame update
    public override void Init(Canvas _canvas)
    {
        base.Init(_canvas);
    }

    public override void OpenPopUp()
    {
        base.OpenPopUp();
   
        Debug.Log("Shop Open!!");
    }

    public override void ClosePopUp()
    {
        base.ClosePopUp();

        Debug.Log("Shop Close!!");
    }


}
