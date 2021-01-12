using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgitManager : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField]
    private CanvasGroup agitCanvasGroup;

    [Header("Agits")]
    [SerializeField]
    private Agit_A agit_A;
    [SerializeField]
    private Agit_B agit_B;

    public void Init()
    {
        if(agitCanvasGroup == null)
        {
            agitCanvasGroup =  HomeManager.Instance.CanvasParent.transform.GetChild(1).GetComponent<CanvasGroup>();
        }
    }

    public void SetUI_Agit_A()
    {   
        Debug.Log("아지트 호출!!");
        //agitCanvasGroup.alpha = 1;

        HomeManager.Instance.PushUIView(agit_A);
    }

    public void TestButton()
    {
        Debug.Log("아지트 내에 있는 버튼 호출!");
    }
}
