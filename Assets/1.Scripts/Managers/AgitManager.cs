using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Agit_Index
{
    None = -1,
    AGIT_A = 0,
    AGIT_B,
}


public class AgitManager : MonoBehaviour
{
    public Agit_Index curAgit = Agit_Index.None;

    [Header("Canvas")]
    [SerializeField]
    private CanvasGroup agitCanvasGroup;
    [SerializeField]
    private Canvas canvas;
    [Header("Agits")]
    [SerializeField]
    private Agit_A agit_A;
    [SerializeField]
    private Agit_B agit_B;

    public void Init(Canvas _canvas)
    {
        canvas = _canvas;

        if (agitCanvasGroup == null)
        {
            agitCanvasGroup = canvas.GetComponent<CanvasGroup>();
        }
        if(agit_A == null)
        {
            agit_A = canvas.transform.GetChild((int)Agit_Index.AGIT_A).GetComponent<Agit_A>();
            agit_A.CloseAgit();
        }
        if (agit_B == null)
        {
            agit_B = canvas.transform.GetChild((int)Agit_Index.AGIT_B).GetComponent<Agit_B>();
            agit_B.CloseAgit();
        }

    }

    public void Open_Agit(Agit_Index _agit)
    {   
        //if(curAgit == Agit_Index.AGIT_A)
        //{
        //    curAgit = Agit_Index.AGIT_B;
        //    _agit = curAgit;
        //}

        switch(_agit)
        {
            case Agit_Index.AGIT_A:
                agit_A.OpenAgit();
                curAgit = Agit_Index.AGIT_A;
                //Add To UINavigation
                //HomeManager.Instance.PushUIView(agit_A);
                break;
            case Agit_Index.AGIT_B:
                agit_B.OpenAgit();
                curAgit = Agit_Index.AGIT_B;
                //Add To UINavigation
                //HomeManager.Instance.PushUIView(agit_B);
                break;
        }

        Debug.Log("Open " + _agit.ToString() + " !!");
    }


    public void Close_Agit(Agit_Index _agit)
    {
        Debug.Log("Close " + _agit.ToString() + " !!");
        //Alpha 0
        agitCanvasGroup.alpha = 1;
        //Sort to 0
        canvas.sortingOrder = 0;

        

        switch (_agit)
        {
            case Agit_Index.AGIT_A:
                agit_A.CloseAgit();
                curAgit = Agit_Index.None;
                break;
            case Agit_Index.AGIT_B:
                agit_B.CloseAgit();
                curAgit = Agit_Index.None;
                break;
        }

    }

    public void TestButton()
    {
        Debug.Log("아지트 내에 있는 버튼 호출!");
    }
}
