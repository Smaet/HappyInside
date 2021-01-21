using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ResultPanel : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup cg;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Button confirmButton;
    [SerializeField]
    private Text text;

    public void Init()
    {
        

        if (cg == null)
        {
            cg = GetComponent<CanvasGroup>();
        }

        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }

        //버튼 할당에 UniRx 사용
        confirmButton.onClick
        .AsObservable()
        .Subscribe(_ => {
            HomeManager.Instance.OnClickBackButton();
        }).AddTo(this);

        cg.alpha = 0;
        gameObject.SetActive(false);
    }

    public void OnResultPanel(string _getMoney)
    {
        text.text = "현재 획득한 금액 : " + _getMoney;
       
        gameObject.SetActive(true);

        //캐릭터 관련 정보 처리...

    }
}
