using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Touch;
using UniRx;
public enum PopUpIndex
{
    NONE = -1,
    SHOP = 0,
    USERPAGE,
    USERATTAINMENT
}

public class BasePopUp : MyUIView
{
    [SerializeField]
    private PopUpIndex curPopUpIndex;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private CanvasGroup cg;
    [SerializeField]
    private LeanSelectable leanSelectable;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private GameObject popUpParent;
    public virtual void Init(Canvas _canvas)
    {
        canvas = _canvas;

        popUpParent = canvas.transform.GetChild((int)curPopUpIndex).gameObject;
        cg = popUpParent.transform.GetComponent<CanvasGroup>();
        leanSelectable = popUpParent.transform.GetChild(0).GetComponent<LeanSelectable>();
        closeButton = popUpParent.transform.GetChild(1).transform.GetChild(1).GetComponent<Button>();

        leanSelectable.OnSelect
            .AsObservable()
            .Subscribe(_ =>
            {
                //HomeManager.Instance.OnClickBackPopUpButton();
            }).AddTo(this);

        closeButton.onClick
            .AsObservable()
            .Subscribe(_ =>
            {
                //HomeManager.Instance.OnClickBackPopUpButton();
            }).AddTo(this);
    }   
    public virtual void OpenPopUp()
    {
       
        popUpParent.SetActive(true);
        MyFader.Instance.StartAnotherCanvasFader(cg, FaderState.FADEOUT, 0.5f);
    }

    public virtual void ClosePopUp()
    {
        MyFader.Instance.StartAnotherCanvasFader(cg, FaderState.FADEIN, 0.5f);
    }

}
