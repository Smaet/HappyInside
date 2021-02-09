using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletTap_Inventory : TabletTap
{
    public override void Init()
    {
        base.Init();

    }
    public override void ShowTap()
    {
        base.ShowTap();
    }

    public override void HideTap()
    {
        base.HideTap();
    }

    protected override void UpdateInfo()
    {
        base.UpdateInfo();

        //내정보 업데이트 
        //1. 뱃지
        //2. 조직이름, 랭킹, 등급, 효과,
        //3. 조직 멤버.
    }

    public void ShowMyInfo()
    {
        UpdateInfo();
    }
}
