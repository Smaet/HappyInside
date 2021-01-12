using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepartmentStore : BaseFlexPlace
{
    // Start is called before the first frame update
    public override void OpenFlexPlace()
    {
        Debug.Log("백화점 호출!");
        base.OpenFlexPlace();
    }
}
