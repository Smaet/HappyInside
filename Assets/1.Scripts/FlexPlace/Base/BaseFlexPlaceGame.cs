using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class BaseFlexPlaceGame : MyUIView
{
    [SerializeField]
    private Button backButton;

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
                       //HomeManager.Instance.OnClickBackButton();
                }).AddTo(this); 
        }
    }

    
    public virtual void OpenFlexPlaceGame()
    {
        Init();//초기화
        //해당하는 컨텐츠 Active
        gameObject.SetActive(true);
        Debug.Log("Base FlexGame Call!!");
       
    }

    public virtual void CloseFlexPlaceGame()
    {
        gameObject.SetActive(false);
    }
}
