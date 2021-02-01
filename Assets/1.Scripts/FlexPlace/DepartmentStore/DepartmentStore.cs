using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public enum DepartmentStoreButtonIndex
{
    None = -1,
    Badge =0,
    Ranking,
    Guide,
}


public class DepartmentStore : BaseFlexPlace
{
    public override void SetButton()
    {
        base.SetButton();

        for (int i = 0; i < buttons.Length; i++)
        {
            if((DepartmentStoreButtonIndex)i == DepartmentStoreButtonIndex.Badge)
            {
                buttons[i].onClick.AddListener(PoloSFX.Instance.PlayAgitBGM);
            }
        }
    }
}
