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
    SHOP,
    USERPAGE,
    USERATTAINMENT,
    FLEX_01,
    FLEX_02,
    FLEX_03,
    AGIT,
    FLEXGAME_01,
    FLEXGAME_02,
    FLEXGAME_03,
    
}

public enum CanvasIndex
{
    HOME = 0,
    AGIT,
    FLEXPLACE,
    FADE,
}

#endregion

public class HomeManager : SimpleSingleton<HomeManager>
{
    [SerializeField]
    private UINavigation uINavigation; 


    #region Canvas
    [Header("Canvas")]
    public GameObject CanvasParent;
    [SerializeField]
    private Canvas [] allCanvas;

    [SerializeField]
    private GameObject HomeMenuButtonParent;
    [SerializeField]
    private HomeMenuButton [] homeMenuButtons;
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
     [SerializeField]
    private TimeManager timeManager;
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

        if(HomeMenuButtonParent == null)
        {
            HomeMenuButtonParent = CanvasParent.transform.GetChild(0).GetChild(1).gameObject;
        }

    
    }

    void InitManagers()
    {
        if(shop == null)
        {
            shop = transform.GetChild((int)MenuIndex.SHOP).GetComponent<Shop>();
        }
        if(userPage == null)
        {
            userPage = transform.GetChild((int)MenuIndex.USERPAGE).GetComponent<UserPage>();
        }
        if(userAttainment == null)
        {
            userAttainment = transform.GetChild((int)MenuIndex.USERATTAINMENT).GetComponent<UserAttainment>();
        }
        if(agitManager == null)
        {
            agitManager = transform.GetChild((int)MenuIndex.AGIT).GetComponent<AgitManager>();
            agitManager.Init();
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
        int buttonCount = HomeMenuButtonParent.transform.childCount;
        if(homeMenuButtons == null || homeMenuButtons.Length == 0)
        {
            homeMenuButtons = new HomeMenuButton[buttonCount];

            for(int buttonIndex = 0; buttonIndex < buttonCount; buttonIndex++)
            {
                int index = buttonIndex;
                
                homeMenuButtons[buttonIndex] = HomeMenuButtonParent.transform.GetChild(buttonIndex).GetComponent<HomeMenuButton>();
                homeMenuButtons[buttonIndex].Init();
                
                //버튼 할당에 UniRx 사용
                homeMenuButtons[buttonIndex].button.onClick
                .AsObservable()
                .Subscribe(_ =>{
                        OnClickHomeUIButton((HomeMenuButtonIndex)index);
                }).AddTo(this); 
            }
        }

        
    }


    #region 홈 화면 버튼 이벤트
    
    //Back Button을 위한 UIView Push
    public void PushUIView(UIView _view)
    {
        uINavigation.PushHistory(_view);
    }

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

        //홈 화면 Alpha 0
        OpenHomeCanvas(1);

        yield return new WaitForSeconds(1f);

        //특정 버튼 장면으로 전환
        switch(_index)
        {
            case HomeMenuButtonIndex.SHOP:
                Debug.Log("상점 호출!");
                break;
            case HomeMenuButtonIndex.USERPAGE:
                Debug.Log("환경설정/내정보 호출!");
                break;
            case HomeMenuButtonIndex.USERATTAINMENT:
                Debug.Log("퀘스트/업적 호출!");
                break;
            case HomeMenuButtonIndex.FLEX_01:
                flexPlaceManager.OpenFlexPlaceUI(FlexPlaceIndex.DEPARTMENTSTORE);
                break;
            case HomeMenuButtonIndex.FLEX_02:
                flexPlaceManager.OpenFlexPlaceUI(FlexPlaceIndex.DEPARTMENTSTORE);
                break;
            case HomeMenuButtonIndex.FLEX_03:
                flexPlaceManager.OpenFlexPlaceUI(FlexPlaceIndex.DEPARTMENTSTORE);
                break;
            case HomeMenuButtonIndex.AGIT:
                agitManager.SetUI_Agit_A();
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
            Debug.Log("Close " + index.ToString());
            StartCoroutine(SetHomeUI_Back(index));
            //뒤로가기 했을 때 열려는 UI 정보
            if(uINavigation.historyUI.Count == 0)
            {
                //맨 처음 Home Open
                OpenHomeCanvas(1);
            }
        }
    }
    IEnumerator SetHomeUI_Back(HomeMenuButtonIndex _index)               // 홈 화면에 버튼에 따른 장면 전환
    {
        isFading = true;
        //화면 페이드 아웃
        MyFader.Instance.StartFader(FaderState.FADEOUT,0.5f);


        yield return new WaitForSeconds(1f);

        //특정 버튼 장면으로 전환
        switch(_index)
        {
            case HomeMenuButtonIndex.SHOP:
                Debug.Log("상점 호출!");
                break;
            case HomeMenuButtonIndex.USERPAGE:
                Debug.Log("환경설정/내정보 호출!");
                break;
            case HomeMenuButtonIndex.USERATTAINMENT:
                Debug.Log("퀘스트/업적 호출!");
                break;
            case HomeMenuButtonIndex.FLEX_01:
                flexPlaceManager.CloseFlexPlaceUI(FlexPlaceIndex.DEPARTMENTSTORE);
                break;
            case HomeMenuButtonIndex.FLEX_02:
                flexPlaceManager.CloseFlexPlaceUI(FlexPlaceIndex.DEPARTMENTSTORE);
                break;
            case HomeMenuButtonIndex.FLEX_03:
                flexPlaceManager.CloseFlexPlaceUI(FlexPlaceIndex.DEPARTMENTSTORE);
                break;
            case HomeMenuButtonIndex.AGIT:
                agitManager.SetUI_Agit_A();
                break;
            case HomeMenuButtonIndex.FLEXGAME_01:
                agitManager.SetUI_Agit_A();
                break;
            case HomeMenuButtonIndex.FLEXGAME_02:
                agitManager.SetUI_Agit_A();
                break;
            case HomeMenuButtonIndex.FLEXGAME_03:
                agitManager.SetUI_Agit_A();
                break;
        }

        //1초 후에 다시 페이드 인
        yield return new WaitForSeconds(1f);

        MyFader.Instance.StartFader(FaderState.FADEIN,0.5f);

        yield return new WaitForSeconds(0.1f);
        isFading = false;

    }

    #endregion

    public void OpenHomeCanvas(float alpha)
    {
        //홈 화면으로 돌아감.
        Debug.Log("홈 화면 호출!");
        allCanvas[0].GetComponent<CanvasGroup>().alpha = alpha ;
    }
}
