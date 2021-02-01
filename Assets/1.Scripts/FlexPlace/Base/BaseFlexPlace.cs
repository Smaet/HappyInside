using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;




public class BaseFlexPlace : MonoBehaviour
{
    [SerializeField]
    private FlexPlaceIndex flexIndex;
    [SerializeField]
    public Button[] buttons;                //플렉스 공간(로비)에서 사용되는 버튼들
    //public Doozy.Examples.

    // Start is called before the first frame update
    public virtual void SetButton()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.RemoveAllListeners();
        }
    }



}
