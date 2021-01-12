using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;


public class BaseFlexPlace : UIView
{   
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private Button gameButton;
    private FlexPlaceManager manager;

    // Start is called before the first frame update
    protected virtual void Init()
    {
        if(backButton == null)
        {
            backButton = transform.GetChild(0).GetComponent<Button>();

            //버튼 할당에 UniRx 사용
                backButton.onClick
                .AsObservable()
                .Subscribe(_ =>{
                       HomeManager.Instance.OnClickBackButton();
                }).AddTo(this); 
        }
        if(gameButton == null)
        {
            gameButton = transform.GetChild(1).GetComponent<Button>();

            //버튼 할당에 UniRx 사용
                backButton.onClick
                .AsObservable()
                .Subscribe(_ =>{
                    manager.OpenFlexPlaceGameUI(FlexPlaceIndex.DEPARTMENTSTORE);
                }).AddTo(this); 
        }
    }





    public virtual void OpenFlexPlace()
    {
        //manager = _manager;

        Init();//초기화
       
        this.gameObject.SetActive(true);
        //해당하는 컨텐츠 Active
    }

    public virtual void CloseFlexPlace()
    {
        this.gameObject.SetActive(false);
    }
}
