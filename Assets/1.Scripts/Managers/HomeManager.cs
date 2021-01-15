/*
  2021.01.11 Created 
  HomeManager.cs   
  
  * Layer 순서
  - 0 : 안보이는 Layer
  - 500 : 현재 보이는 Layer
  - 1000 : Fade Layer

  기능
  1. 전체적인 매니저 클래스 관리
  2. 홈 화면에서 이루어지는 버튼 이벤트 관리

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;


#region Enums
public enum MenuIndex
{
    SHOP = 0,
    USERPAGE,
    USERATTAINMENT,
    AGIT,
    FLEXPLACE,
    INPUT,
    TIME,
}

public enum HomeMenuButtonIndex
{
    None = -1,
    SHOP = 0,
    USERPAGE,
    USERATTAINMENT,
    FLEX_01,
    FLEX_02,
    FLEX_03,
    AGIT_A,
    AGIT_B,
    FLEXGAME_01,
    FLEXGAME_02,
    FLEXGAME_03,
    
}

public enum CanvasIndex
{
    HOME = 0,
    AGIT,
    FLEXPLACE,
    POPUP,
    FADE,
}

#endregion

public class HomeManager : SimpleSingleton<HomeManager>
{
    [Header("Test")]
    public ScrollRect scrollRect;
    public ScrollRect scrollRect2;

    [SerializeField]
    private UINavigation uINavigation; 


    #region Canvas
    [Header("HomeCanvas")]
    public GameObject CanvasParent;
    [SerializeField]
    private Canvas [] allCanvas;
    [SerializeField]
    private GameObject TopButtonsPanel;
    [SerializeField]
    private GameObject AgitPanel;
    [SerializeField]
    private GameObject FlexPanel;
    [SerializeField]
    private HomeMenuButton [] topPanelButtons;
    [SerializeField]
    private HomeMenuButton[] agitPanelButtons;
    [SerializeField]
    private HomeMenuButton[] flexPanelButtons;
    #endregion

    #region Managers
    [Header("Managers")]
    [SerializeField]
    private Shop shop;
    [SerializeField]
    private UserPage userPage;
    [SerializeField]
    private UserAttainment userAttainment;
    [SerializeField]
    private AgitManager agitManager;
    [SerializeField]
    private FlexPlaceManager flexPlaceManager;
    [SerializeField]
    private InputManager inputManager;
    public TimeManager timeManager;
    #endregion

    #region Variables
    private bool isFading = false;

    #endregion
   

    protected override void Awake()
    {
        base.Awake();

        if(uINavigation == null)
        {
            uINavigation = GetComponent<UINavigation>();
            uINavigation.Init();
        }

        Init();

    }

    void Init()
    {
        InitCanvas();
        InitManagers();
        InitButtons();
    }

    void InitCanvas()
    {
        if(CanvasParent == null)
        {
            CanvasParent = GameObject.FindGameObjectWithTag("CanvasParent");

            if(allCanvas == null || allCanvas.Length == 0)
            {
                allCanvas = new Canvas[CanvasParent.transform.childCount];
            }

            for(int canvasIndex = 0; canvasIndex < CanvasParent.transform.childCount; canvasIndex++)
            {
                allCanvas[canvasIndex] = CanvasParent.transform.GetChild(canvasIndex).GetComponent<Canvas>();
            }
        }

        if(TopButtonsPanel == null)
        {
            TopButtonsPanel = CanvasParent.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
        }

        if(AgitPanel == null)
        {
            AgitPanel = CanvasParent.transform.GetChild(0).GetChild(2).gameObject;
        }

        if (FlexPanel == null)
        {
            FlexPanel = CanvasParent.transform.GetChild(0).GetChild(3).gameObject;
        }

        //CanvasSetting(HomeMenuButtonIndex.None, 1, 0, 500);


    }

    void InitManagers()
    {
        if(shop == null)
        {
            shop = transform.GetChild((int)MenuIndex.SHOP).GetComponent<Shop>();
            shop.Init(allCanvas[(int)CanvasIndex.POPUP]);
        }
        if(userPage == null)
        {
            userPage = transform.GetChild((int)MenuIndex.USERPAGE).GetComponent<UserPage>();
            userPage.Init(allCanvas[(int)CanvasIndex.POPUP]);
        }
        if(userAttainment == null)
        {
            userAttainment = transform.GetChild((int)MenuIndex.USERATTAINMENT).GetComponent<UserAttainment>();
            userAttainment.Init(allCanvas[(int)CanvasIndex.POPUP]);
        }
        if(agitManager == null)
        {
            agitManager = transform.GetChild((int)MenuIndex.AGIT).GetComponent<AgitManager>();
            agitManager.Init(allCanvas[(int)CanvasIndex.AGIT]);
        }
        if(flexPlaceManager == null)
        {
            flexPlaceManager = transform.GetChild((int)MenuIndex.FLEXPLACE).GetComponent<FlexPlaceManager>();
            flexPlaceManager.Init(allCanvas[(int)CanvasIndex.FLEXPLACE]);
        }
        if(inputManager == null)
        {
            inputManager = transform.GetChild((int)MenuIndex.INPUT).GetComponent<InputManager>();
        }
        if(timeManager == null)
        {
            timeManager = transform.GetChild((int)MenuIndex.TIME).GetComponent<TimeManager>();
        }
    }

    //버튼 초기화
    void InitButtons()
    {
        //상단 패널 버튼
        int buttonCount = TopButtonsPanel.transform.childCount;
        if(topPanelButtons == null || topPanelButtons.Length == 0)
        {
            topPanelButtons = new HomeMenuButton[buttonCount];

            for(int buttonIndex = 0; buttonIndex < buttonCount; buttonIndex++)
            {
                int index = buttonIndex;

                topPanelButtons[buttonIndex] = TopButtonsPanel.transform.GetChild(buttonIndex).GetComponent<HomeMenuButton>();
                topPanelButtons[buttonIndex].Init();

                topPanelButtons[buttonIndex].button.onClick
                     .AsObservable()
                     .Subscribe(_ => {
                         OnClickHomeUIPopUpButton((HomeMenuButtonIndex)index);
                     }).AddTo(this);
            }
        }

        //하단 아지트 버튼
        ScrollRect agitScrollRect = AgitPanel.transform.GetChild(0).GetComponent<ScrollRect>();
        int agitButtonCount = agitScrollRect.content.childCount;
        if (agitPanelButtons == null || agitPanelButtons.Length == 0)
        {
            agitPanelButtons = new HomeMenuButton[agitButtonCount];

            for (int agitButtonIndex = 0; agitButtonIndex < agitButtonCount; agitButtonIndex++)
            {
                agitPanelButtons[agitButtonIndex] = agitScrollRect.content.GetChild(agitButtonIndex).GetComponent<HomeMenuButton>();
                agitPanelButtons[agitButtonIndex].Init();


                if (agitButtonIndex == 0)
                {
                    agitPanelButtons[agitButtonIndex].button.onClick
                   .AsObservable()
                   .Subscribe(_ => {
                       OnClickHomeUIButton(HomeMenuButtonIndex.AGIT_A);
                   }).AddTo(this);
                }
                else if (agitButtonIndex == 1)
                {
                    agitPanelButtons[agitButtonIndex].button.onClick
                   .AsObservable()
                   .Subscribe(_ => {
                       OnClickHomeUIButton(HomeMenuButtonIndex.AGIT_B);
                   }).AddTo(this);
                }
            }
        }

        ScrollRect flexScrollRect = FlexPanel.transform.GetChild(0).GetComponent<ScrollRect>();
        int flexButtonCount = flexScrollRect.content.childCount;
        if (flexPanelButtons == null || flexPanelButtons.Length == 0)
        {
            flexPanelButtons = new HomeMenuButton[flexButtonCount];

            for (int flexButtonIndex = 0; flexButtonIndex < flexButtonCount; flexButtonIndex++)
            {
                flexPanelButtons[flexButtonIndex] = flexScrollRect.content.GetChild(flexButtonIndex).GetComponent<HomeMenuButton>();
                flexPanelButtons[flexButtonIndex].Init();

                if (flexButtonIndex == 0)
                {
                    flexPanelButtons[flexButtonIndex].button.onClick
                   .AsObservable()
                   .Subscribe(_ => {
                       OnClickHomeUIButton(HomeMenuButtonIndex.FLEX_01);
                   }).AddTo(this);
                }
                else if (flexButtonIndex == 1)
                {
                    flexPanelButtons[flexButtonIndex].button.onClick
                   .AsObservable()
                   .Subscribe(_ => {
                       OnClickHomeUIButton(HomeMenuButtonIndex.FLEX_02);
                   }).AddTo(this);
                }
                else if (flexButtonIndex == 2)
                {
                    flexPanelButtons[flexButtonIndex].button.onClick
                   .AsObservable()
                   .Subscribe(_ => {
                       OnClickHomeUIButton(HomeMenuButtonIndex.FLEX_03);
                   }).AddTo(this);
                }
            }
        }
    }


    #region 홈 화면 버튼 이벤트
    
    //Back Button을 위한 UIView Push
    public void PushUIView(UIView _view)
    {
        uINavigation.PushHistory(_view);
    }
    #region HOMEUI BUTTON
    //홈 화면에 있는 버튼들을 눌렀했을 때 실행되는 이벤트
    public void OnClickHomeUIButton(HomeMenuButtonIndex _index)
    {
        if(isFading == false)
        {
           
            StartCoroutine(OpenHomeUI(_index));
        }
    }
    IEnumerator OpenHomeUI(HomeMenuButtonIndex _index)               // 홈 화면에 버튼에 따른 장면 전환
    {
        isFading = true; //화면 페이드 아웃
        MyFader.Instance.StartFader(FaderState.FADEOUT,0.5f);


        yield return new WaitForSeconds(0.5f);


        //홈 화면 Alpha 0
        CanvasSetting(_index, 0, 1, 0);


        //특정 버튼 장면으로 전환
        switch (_index)
        {
            case HomeMenuButtonIndex.SHOP:
                shop.OpenPopUp();
                break;
            case HomeMenuButtonIndex.USERPAGE:
                userPage.OpenPopUp();
                break;
            case HomeMenuButtonIndex.USERATTAINMENT:
                userAttainment.OpenPopUp();
                break;
            case HomeMenuButtonIndex.FLEX_01:
                flexPlaceManager.OpenFlexPlaceUI(_index);
                break;
            case HomeMenuButtonIndex.FLEX_02:
                flexPlaceManager.OpenFlexPlaceUI(_index);
                break;
            case HomeMenuButtonIndex.FLEX_03:
                flexPlaceManager.OpenFlexPlaceUI(_index);
                break;
            case HomeMenuButtonIndex.AGIT_A:
                agitManager.Open_Agit(Agit_Index.AGIT_A);
                break;
            case HomeMenuButtonIndex.AGIT_B:
                agitManager.Open_Agit(Agit_Index.AGIT_B);
                break;
            case HomeMenuButtonIndex.FLEXGAME_01:
                flexPlaceManager.OpenFlexPlaceGameUI(FlexPlaceIndex.DEPARTMENTSTORE);
                break;
            case HomeMenuButtonIndex.FLEXGAME_02:
                flexPlaceManager.OpenFlexPlaceGameUI(FlexPlaceIndex.DEPARTMENTSTORE);
                break;
            case HomeMenuButtonIndex.FLEXGAME_03:
                flexPlaceManager.OpenFlexPlaceGameUI(FlexPlaceIndex.DEPARTMENTSTORE);
                break;
        }

        //1초 후에 다시 페이드 인
        yield return new WaitForSeconds(1f);

        MyFader.Instance.StartFader(FaderState.FADEIN,0.5f);

        yield return new WaitForSeconds(0.1f);
        isFading = false;

    }
    

    //Back Button을 눌렀을때 실행되는 이벤트
    public void OnClickBackButton()
    {
        if(isFading == false)
        {
           
            //뒤로가기 했을 때 닫을려는 UI의 정보
            UIView uIView = uINavigation.PopHistory();

            //현재 페이지 Off
            HomeMenuButtonIndex index = uIView.homeMenuButtonIndex;
            StartCoroutine(SetHomeUI_Back(index));
          
        }
    }
    IEnumerator SetHomeUI_Back(HomeMenuButtonIndex _index)               // 홈 화면에 버튼에 따른 장면 전환
    {
        isFading = true;
        //화면 페이드 아웃
        MyFader.Instance.StartFader(FaderState.FADEOUT,0.5f);


        yield return new WaitForSeconds(0.5f);

        //뒤로가기 했을 때 열려는 UI 정보
        if (uINavigation.historyUI.Count == 0)
        {
            //맨 처음 Home Open
            CanvasSetting(_index, 1, 0, 500);
        }

        //특정 버튼 장면으로 전환
        switch (_index)
        {
            case HomeMenuButtonIndex.FLEX_01:
                flexPlaceManager.CloseFlexPlaceUI(FlexPlaceIndex.DEPARTMENTSTORE);
                break;
            case HomeMenuButtonIndex.FLEX_02:
                flexPlaceManager.CloseFlexPlaceUI(FlexPlaceIndex.DEPARTMENTSTORE);
                break;
            case HomeMenuButtonIndex.FLEX_03:
                flexPlaceManager.CloseFlexPlaceUI(FlexPlaceIndex.DEPARTMENTSTORE);
                break;
            case HomeMenuButtonIndex.AGIT_A:
                agitManager.Close_Agit(agitManager.curAgit);
                break;
            case HomeMenuButtonIndex.AGIT_B:
                agitManager.Close_Agit(agitManager.curAgit);
                break;
            case HomeMenuButtonIndex.FLEXGAME_01:
                flexPlaceManager.CloseFlexPlaceGameUI(FlexPlaceIndex.DEPARTMENTSTORE);
                break;
            case HomeMenuButtonIndex.FLEXGAME_02:
                flexPlaceManager.CloseFlexPlaceGameUI(FlexPlaceIndex.DEPARTMENTSTORE);
                break;
            case HomeMenuButtonIndex.FLEXGAME_03:
                flexPlaceManager.CloseFlexPlaceGameUI(FlexPlaceIndex.DEPARTMENTSTORE);
                break;
        }

        //1초 후에 다시 페이드 인
        yield return new WaitForSeconds(1f);

        MyFader.Instance.StartFader(FaderState.FADEIN,0.5f);

        yield return new WaitForSeconds(0.1f);
        isFading = false;

    }

    #endregion

    #region POP BUTTON
    //홈 화면에 있는 POPUP Button
    public void OnClickHomeUIPopUpButton(HomeMenuButtonIndex _index)
    {
        if (isFading == false)
        {
            StartCoroutine(OpenHomeUI_PopUp(_index));
        }
    }
    IEnumerator OpenHomeUI_PopUp(HomeMenuButtonIndex _index)               // 홈 화면에 버튼에 따른 장면 전환
    {
        isFading = true;


        //홈 화면 Alpha 0
        CanvasSetting(_index, 1, 1, 0);

        //특정 버튼 장면으로 전환
        switch (_index)
        {
            case HomeMenuButtonIndex.SHOP:
                shop.OpenPopUp();
                break;
            case HomeMenuButtonIndex.USERPAGE:
                userPage.OpenPopUp();
                break;
            case HomeMenuButtonIndex.USERATTAINMENT:
                userAttainment.OpenPopUp();
                break;
        }

        yield return new WaitForSeconds(0.5f);
            
        isFading = false;

    }


    //Back Button을 눌렀을때 실행되는 이벤트
    public void OnClickBackPopUpButton()
    {
        if (isFading == false)
        {

            //뒤로가기 했을 때 닫을려는 UI의 정보
            UIView uIView = uINavigation.PopHistory();

            //현재 페이지 Off
            HomeMenuButtonIndex index = uIView.homeMenuButtonIndex;
            StartCoroutine(SetHomeUIPopUp_Back(index));

        }
    }
    IEnumerator SetHomeUIPopUp_Back(HomeMenuButtonIndex _index)               // 홈 화면에 버튼에 따른 장면 전환
    {
        isFading = true;

      

        //특정 버튼 장면으로 전환
        switch (_index)
        {
            case HomeMenuButtonIndex.SHOP:
                shop.ClosePopUp();
                break;
            case HomeMenuButtonIndex.USERPAGE:
                userPage.ClosePopUp();
                break;
            case HomeMenuButtonIndex.USERATTAINMENT:
                userAttainment.ClosePopUp();
                break;
        }
        yield return new WaitForSeconds(0.5f);

        //뒤로가기 했을 때 열려는 UI 정보
        if (uINavigation.historyUI.Count == 0)
        {
            //맨 처음 Home Open
            CanvasSetting(_index, 1, 0, 500);
        }

        isFading = false;

    }

    #endregion



    #endregion

    public void CanvasSetting(HomeMenuButtonIndex _buttonIndex, float _homeCGAlpha, float _anotherCGAlpha, int _homeSortingOrder)
    {
        if (_buttonIndex != HomeMenuButtonIndex.None)
        {
            //홈 캔버스 설정
            if (_homeCGAlpha == 0)
            {
                allCanvas[(int)CanvasIndex.HOME].GetComponent<UnityEngine.UI.GraphicRaycaster>().enabled = false;
            }
            else
            {
                allCanvas[(int)CanvasIndex.HOME].GetComponent<UnityEngine.UI.GraphicRaycaster>().enabled = true;
            }
            allCanvas[(int)CanvasIndex.HOME].sortingOrder = _homeSortingOrder;
            allCanvas[(int)CanvasIndex.HOME].GetComponent<CanvasGroup>().alpha = _homeCGAlpha;

            //다른 캔버스 설정
            Canvas curCanvas = GetCanvas(_buttonIndex);
            CanvasGroup cg = curCanvas.GetComponent<CanvasGroup>();
            GraphicRaycaster gr = curCanvas.GetComponent<GraphicRaycaster>();
            cg.alpha = _anotherCGAlpha;
            if (_anotherCGAlpha == 0)
            {
                curCanvas.sortingOrder = 0;
                gr.enabled = false;
            }
            else
            {
                curCanvas.sortingOrder = 500;
                gr.enabled = true;
            }
        }
        //초기 설정
        else
        { 
            //홈 캔버스 설정
            if (_homeCGAlpha == 0)
            {
                allCanvas[(int)CanvasIndex.HOME].GetComponent<UnityEngine.UI.GraphicRaycaster>().enabled = false;
            }
            else
            {
                allCanvas[(int)CanvasIndex.HOME].GetComponent<UnityEngine.UI.GraphicRaycaster>().enabled = true;
            }
            allCanvas[(int)CanvasIndex.HOME].sortingOrder = _homeSortingOrder;
            allCanvas[(int)CanvasIndex.HOME].GetComponent<CanvasGroup>().alpha = _homeCGAlpha;

            for (int i=1; i < 4; i++)
            {
                allCanvas[i].GetComponent<CanvasGroup>().alpha = _anotherCGAlpha;
                allCanvas[i].GetComponent<GraphicRaycaster>().enabled = false;
            }
          
        }
        
    }

    private Canvas GetCanvas(HomeMenuButtonIndex _index)
    {
        Canvas canvas = null;
        switch (_index)
        {
            case HomeMenuButtonIndex.SHOP:
                canvas = allCanvas[(int)CanvasIndex.POPUP];
                break;
            case HomeMenuButtonIndex.USERPAGE:
                canvas = allCanvas[(int)CanvasIndex.POPUP];
                break;
            case HomeMenuButtonIndex.USERATTAINMENT:
                canvas = allCanvas[(int)CanvasIndex.POPUP];
                break;
            case HomeMenuButtonIndex.FLEX_01:
                canvas = allCanvas[(int)CanvasIndex.FLEXPLACE];
                break;
            case HomeMenuButtonIndex.FLEX_02:
                canvas = allCanvas[(int)CanvasIndex.FLEXPLACE];
                break;
            case HomeMenuButtonIndex.FLEX_03:
                canvas = allCanvas[(int)CanvasIndex.FLEXPLACE];
                break;
            case HomeMenuButtonIndex.AGIT_A:
                canvas = allCanvas[(int)CanvasIndex.AGIT];
                break;
            case HomeMenuButtonIndex.AGIT_B:
                canvas = allCanvas[(int)CanvasIndex.AGIT];
                break;
            case HomeMenuButtonIndex.FLEXGAME_01:
                canvas = allCanvas[(int)CanvasIndex.FLEXPLACE];
                break;
            case HomeMenuButtonIndex.FLEXGAME_02:
                canvas = allCanvas[(int)CanvasIndex.FLEXPLACE];
                break;
            case HomeMenuButtonIndex.FLEXGAME_03:
                canvas = allCanvas[(int)CanvasIndex.FLEXPLACE];
                break;
        }

        return canvas;
    }



    #region 임시 스크롤 렉트


    private bool isButtonSliding = false;
    private bool isButtonOnTop = false;
    public void StartButtonAutoSlide()
    {
        if(isButtonSliding == false)
        {
            isButtonSliding = true;
            StartCoroutine(ButtonAutoSlide());
        }
    
    }

    IEnumerator ButtonAutoSlide()
    {
        float startPoint = 0;
        float time = 0;
        float totalTime = 0.3f;

        if(isButtonOnTop == false)
        {
            startPoint = 0;
        }
        else
        {
            startPoint = 1;
        }
        scrollRect.verticalNormalizedPosition = startPoint;
        while(true)
        {
            if(time >= totalTime)
            {
                if (isButtonOnTop == false)
                {
                    isButtonOnTop = true;
                }
                else
                {
                    isButtonOnTop = false;
                }


                isButtonSliding = false;
                yield break;
            }

            if (isButtonOnTop == false)
            {
                scrollRect.verticalNormalizedPosition = Mathf.Lerp(1, startPoint, time / totalTime);
            }
            else
            {
                scrollRect.verticalNormalizedPosition = Mathf.Lerp(0, startPoint, time / totalTime);
            }
            time += Time.deltaTime;
            yield return null;
        }
    }

    private bool isButtonSliding2 = false;
    private bool isButtonOnTop2 = false;
    public void StartButtonAutoSlide2()
    {
        if (isButtonSliding2 == false)
        {
            isButtonSliding2 = true;
            StartCoroutine(ButtonAutoSlide2());
        }

    }

    

    IEnumerator ButtonAutoSlide2()
    {
        float startPoint = 0;
        float time = 0;
        float totalTime = 0.5f;

        if (isButtonOnTop2 == false)
        {
            startPoint = 0;
        }
        else
        {
            startPoint = 1;
        }
        scrollRect2.verticalNormalizedPosition = startPoint;
        while (true)
        {
            if (time >= totalTime)
            {
                if (isButtonOnTop2 == false)
                {
                    isButtonOnTop2 = true;
                }
                else
                {
                    isButtonOnTop2 = false;
                }


                isButtonSliding2 = false;
                yield break;
            }

            if (isButtonOnTop2 == false)
            {
                scrollRect2.verticalNormalizedPosition = Mathf.Lerp(1, startPoint, time / totalTime);
            }
            else
            {
                scrollRect2.verticalNormalizedPosition = Mathf.Lerp(0, startPoint, time / totalTime);
            }
            time += Time.deltaTime;
            yield return null;
        }
    }




    #endregion


}
