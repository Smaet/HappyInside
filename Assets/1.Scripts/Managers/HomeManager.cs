/*
  2021.01.11 Created 
  HomeManager.cs   
  


  기능
  1. 전체적인 매니저 클래스 관리
  2. 홈 화면에서 이루어지는 버튼 이벤트 관리

  3. 홈 화면에서 다른 화면으로 넘어가는 버튼들은 DoozyUI로 처리
  4. 버프 관리

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Engine.UI;
using UniRx;
using Doozy;

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
    TOPUI,
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
    TOPUI,
    FADE,
}

#endregion

public class HomeManager : SimpleSingleton<HomeManager>
{
    //배경 관련
    [Header("Background")]
    public Sprite[] backgrounds;
    public Image background_Image;

    //할아버지 패널
    [Header("GrandFatherPanel")]
    public ScrollRect scrollRect_GrandFather;
    private bool isButtonSliding_GrandFather;
    private bool isButtonOn_GrandFather;

    //오른쪽 아래 기능 버튼들
    [Header("SlideButtonPanel")]
    [SerializeField]
    private BaseSlideButtonPanel[] baseSlideButtonPanels;

    //버프 관리
    public List<Buff> buffs = new List<Buff>();

    #region Managers
    [Header("Managers")]
    [SerializeField]
    private Shop shop;
    [SerializeField]
    private UserPage userPage;
    [SerializeField]
    private UserAttainment userAttainment;
    public AgitManager agitManager;
    [SerializeField]
    public FlexPlaceManager flexPlaceManager;
    [SerializeField]
    private InputManager inputManager;
    public TimeManager timeManager;

    public TopUIManager topUIManager ;
    public GrandFatherHouseManager grandFatherHouseManager;

    public HappyRichDialogueManager happyRichDialogueManager;

    [Header("Gov Sample")]
    public UIView uiView_Gov01;
    public UIView uiView_Gov02;
   
    #endregion
    public void HideGov01Sample()
    {
        uiView_Gov01.Hide();
    }

 
    public void ShowGov02Sample()
    {
        uiView_Gov02.Show();
    }

    public void HideGov02Sample()
    {
        uiView_Gov02.Hide();
    }

    protected override void Awake()
    {
        base.Awake();

        Init();
    }

    public void Init()
    {
        //플렉스 매니저 초기화
        flexPlaceManager.Init();
        //아지트 매니저 초기화
        agitManager.Init();

        ////////////////////////////////
        topUIManager.Init();
        //comprehensivePanel.Init();

        //오른쪽 아래 버튼 패널 초기화
        for(int i=0; i < baseSlideButtonPanels.Length; i++)
        {
            baseSlideButtonPanels[i].SetButton();
        }


        //dController.displaySettings
        //강제 시작.
        //dst.TryStart(actor);
        //dst.start

        User user = GameManager.Instance.user;


        //상단 패널 셋팅
        topUIManager.SetNick(user.userBaseProperties.nickName);
        topUIManager.SetCrystal(user.userBaseProperties.crystal);
        topUIManager.SetNotice("4일 19시간후 할아버지 의심 떡상!!");
        topUIManager.SetPinkChip(user.userBaseProperties.xCoin);
        //topUIManager.SetHour(user.userBaseProperties.gameHour);
        //topUIManager.SetDays(user.userBaseProperties.daysElapsed);

        //게임 시간 시작
        //timeManager.StartGameTime(user.userBaseProperties.gameHour, user.userBaseProperties.daysElapsed);

        //게임 시간에 따른 배경 설정
        //if (user.userBaseProperties.gameHour < 9)
        //{
        //    SetBackground(0);
        //}
        //else if (user.userBaseProperties.gameHour < 17)
        //{
        //    SetBackground(1);
        //}
        //else if (user.userBaseProperties.gameHour < 24)
        //{
        //    SetBackground(2);
        //}


        PoloSFX.Instance.PlayHomeBGM();

    }

    private void Start()
    {
        //Test
        happyRichDialogueManager.StartDialogue();
    }

    public void ResetAgitAndFlexSlide()
    {
        for(int i= 0; i < baseSlideButtonPanels.Length; i++)
        {
            if(baseSlideButtonPanels[i].isButtonTopOn())
            {
                baseSlideButtonPanels[i].StartButtonAutoSlide();
            }
        }
    }

    #region 배경
    public void SetBackground(int _index)
    {
        background_Image.sprite = backgrounds[_index];

    }
    #endregion

    //통계로 수정 예정
    #region 할아버지 그림 버튼
    public void DebugScrollRect(Vector2 _vector2)
    {
        Debug.Log(scrollRect_GrandFather.horizontalNormalizedPosition);
    }

    public void StartButtonAutoSlide_GrandFather()
    {
        if (isButtonSliding_GrandFather == false)
        {
            isButtonSliding_GrandFather = true;
            StartCoroutine(ButtonAutoSlide_GradnFather());
        }

    }

    IEnumerator ButtonAutoSlide_GradnFather()
    {
        float startPoint = 0;
        float time = 0;
        float totalTime = 0.3f;

        if (isButtonOn_GrandFather == false)
        {
            startPoint = 0.03f;
        }
        else
        {
            startPoint = 1;
        }
        scrollRect_GrandFather.horizontalNormalizedPosition = startPoint;
        while (true)
        {
            if (time >= totalTime)
            {
                if (isButtonOn_GrandFather == false)
                {
                    isButtonOn_GrandFather = true;
                    scrollRect_GrandFather.horizontalNormalizedPosition = 0.03f;
                }
                else
                {
                    isButtonOn_GrandFather = false;
                    scrollRect_GrandFather.horizontalNormalizedPosition = 1;
                }


                isButtonSliding_GrandFather = false;
                yield break;
            }

            if (isButtonOn_GrandFather == false)
            {
                scrollRect_GrandFather.horizontalNormalizedPosition = Mathf.Lerp(1, startPoint, time / totalTime);
            }
            else
            {
                scrollRect_GrandFather.horizontalNormalizedPosition = Mathf.Lerp(0, startPoint, time / totalTime);
            }
            time += Time.deltaTime;
            yield return null;
        }
    }
    #endregion]

    public void ShowAnimation()
    {
        print(this.gameObject.name + " Show Animation!");
    }

}
