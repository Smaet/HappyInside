using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public enum Agit_Index
{
    None = -1,
    AGIT_A = 0,
    AGIT_B,
}


public class AgitManager : MonoBehaviour
{
    [SerializeField]
    private bool isInit = false;
    public Agit_Index curAgit = Agit_Index.None;

   
    [Header("Agits")]
    [SerializeField]
    public Agit_A agit_A;
    [SerializeField]
    public Agit_B agit_B;

    [SerializeField]
    private Button agitA_Button;
    [SerializeField]
    private Button agitB_Button;


    public CollegueView collegueView;
    public CollegueItemView collegueItemView;
    public CollegueDeviceView collegueDeviceView;

    public void OnEnable()
    {
        if(isInit == false)
        {
            isInit = true;
            //버튼 셋팅
            //아지트 A
            agitA_Button.onClick
               .AsObservable()
               .Subscribe(_ =>
               {
                   Open_Agit(Agit_Index.AGIT_A);
               }).AddTo(this);

            //agitA_Button.onClick
            // .AsObservable()
            // .Subscribe(_ =>
            // {
            //     Open_Agit(Agit_Index.AGIT_B);
            // }).AddTo(this);
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
                //레벨에 따른 UI 셋팅




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
        //agitCanvasGroup.alpha = 1;
        ////Sort to 0
        //canvas.sortingOrder = 0;

        

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

    public void OpenColleguePanel()
    {

    }
}
