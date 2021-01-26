using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;




public class BaseFlexPlace : MyUIView
{
    [SerializeField]
    private FlexPlaceIndex flexIndex;
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private Button gameButton;

    // Start is called before the first frame update
    protected virtual void Init()
    {
        //if(backButton == null)
        //{
        //    backButton = transform.GetChild(0).GetComponent<Button>();

        //    //버튼 할당에 UniRx 사용
        //        backButton.onClick
        //        .AsObservable()
        //        .Subscribe(_ =>{
        //               HomeManager.Instance.OnClickBackButton();
        //        }).AddTo(this); 
        //}
        //if(gameButton == null)
        //{
        //    gameButton = transform.GetChild(1).GetComponent<Button>();

        //    if(flexIndex == FlexPlaceIndex.DEPARTMENTSTORE)
        //    {
        //        HomeMenuButtonIndex index = HomeMenuButtonIndex.FLEXGAME_01;

        //        ////버튼 할당에 UniRx 사용
        //        //gameButton.onClick
        //        //    .AsObservable()
        //        //    .Subscribe(_ => {
        //        //        HomeManager.Instance.OnClickHomeUIButton(index);
        //        //    }).AddTo(this);
        //    }
        //}
    }





    public virtual void OpenFlexPlace()
    {
        Init();//초기화
        //해당하는 컨텐츠 Active
        gameObject.SetActive(true);
    }

    public virtual void CloseFlexPlace()
    {
        gameObject.SetActive(false);
    }
}
