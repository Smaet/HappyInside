using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agit_A : BaseAgit
{
    public override void OpenAgit()
    {
        Init();
        base.OpenAgit();
    }

    public void OnUseMouse()
    {
        Debug.Log("대화창 클릭!");
    }
}
