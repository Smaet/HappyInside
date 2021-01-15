using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepartmentStore : BaseFlexPlace
{
    private int accumulatedConsumption;         //누적 소비액

    // Start is called before the first frame update
    public override void OpenFlexPlace()
    {
        Debug.Log("DepartmentStore Call!!");
        base.OpenFlexPlace();
    }
}
