
using UnityEngine;
using System;

public class BaseFlexPlaceGame : MonoBehaviour
{
    public virtual void Init() { }
    public virtual void StartSign() { }

    [Header("Events")]
    public Action OnClickDepartmentMiniGameStart;
}
