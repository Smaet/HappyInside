using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Engine.UI;

public class TabletTap : MonoBehaviour
{
    public UIView uiView;
    public ScrollRect scrollRect;

    //이벤트 관련 처리
    public virtual void Init()
    {

    }

    public virtual void ShowTap()
    {
        uiView.gameObject.SetActive(true);
    }
    public virtual void HideTap()
    {
        uiView.gameObject.SetActive(false);
    }


    //각각의 탭 마다 업데이트 되는 정보들
    protected virtual void UpdateInfo()
    {

    }

}
