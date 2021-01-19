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
    private int accumulatedConsumption;         //누적 소비액

    public void StartMiniGame()
    {
        Debug.Log("백화점 미니게임 시작!");
    }

    // Start is called before the first frame update
    public override void OpenFlexPlace()
    {
        Debug.Log("DepartmentStore Call!!");
        base.OpenFlexPlace();
    }

    public void SetGuidePanel()
    {
        Debug.Log("SetGuidePanel Call!!");
    }

    public void SetMyRankingBadge()
    {
        Debug.Log("SetMyRankingBadge Call!!");
    }

    public void SetRankingTop5()
    {
        Debug.Log("SetRankingTop5 Call!!");
    }
   

    public void GoFlexDepartmentEvent(string _string)
    {
        Debug.Log("!!!!!!!!!!!!!!");
    }
  
    
}
