using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class BaseAgit : MyUIView
{
    public Agit_Index aigtIndex;
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private bool isInit = false;

    [SerializeField]
    private Button[] colleagueButtons;      //동료 버튼들
    [SerializeField]
    private Button[] colleagueItemButtons;  //동료 아이템 버튼들

    // Start is called before the first frame update
    protected virtual void Init()
    {
        if(isInit== false)
        {
            isInit = true;

            if (backButton == null)
            {
                backButton = transform.GetChild(1).GetComponent<Button>();

                //버튼 할당에 UniRx 사용
                backButton.onClick
                .AsObservable()
                .Subscribe(_ =>
                {
                    HomeManager.Instance.OnClickBackButton();
                }).AddTo(this);

                //backButton.onClick.AddListener(HomeManager.Instance.OnClickBackButton);
            }
           
        }
      
    }


    public virtual void OpenAgit()
    {
        //해당하는 컨텐츠 Active
        gameObject.SetActive(true);
    }

    public virtual void CloseAgit()
    {
        gameObject.SetActive(false);
    }
}
