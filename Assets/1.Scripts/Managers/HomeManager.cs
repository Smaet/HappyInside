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
    [Header("GrandFatherPanel")]
    public ScrollRect scrollRect_GrandFather;
    private bool isButtonSliding_GrandFather;
    private bool isButtonOn_GrandFather;
   

    [Header("RightBottomMenuButton")]
    public ScrollRect scrollRect_Agit;
    public ScrollRect scrollRect_Flex;

    private bool isButtonSliding_Agit = false;
    public bool isButtonOnTop_Agit = false;


    private bool isButtonSliding_Flex = false;
    public bool isButtonOnTop_Flex = false;

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
    private FlexPlaceManager flexPlaceManager;
    [SerializeField]
    private InputManager inputManager;
    public TimeManager timeManager;

    public TopUIManager topUIManager ;
    public ComprehensivePanel comprehensivePanel;
    public GrandFatherHouseManager grandFatherHouseManager;
    #endregion

    #region Variables
    private bool isFading = false;

    #endregion
   

    protected override void Awake()
    {
        base.Awake();
    }

    public void Init()
    {
        //InitManagers();
        topUIManager.Init();
        comprehensivePanel.Init();

    }

    

    void InitManagers()
    {
        if(shop == null)
        {
            shop = this.transform.GetChild((int)MenuIndex.SHOP).GetComponent<Shop>();
            //shop.Init(allCanvas[(int)CanvasIndex.POPUP]);
        }
        if(userPage == null)
        {
            userPage = transform.GetChild((int)MenuIndex.USERPAGE).GetComponent<UserPage>();
            //userPage.Init(allCanvas[(int)CanvasIndex.POPUP]);
        }
        if(userAttainment == null)
        {
            userAttainment = transform.GetChild((int)MenuIndex.USERATTAINMENT).GetComponent<UserAttainment>();
            //userAttainment.Init(allCanvas[(int)CanvasIndex.POPUP]);
        }
        if(agitManager == null)
        {
            agitManager = transform.GetChild((int)MenuIndex.AGIT).GetComponent<AgitManager>();
            //agitManager.Init(allCanvas[(int)CanvasIndex.AGIT]);
        }
        if(flexPlaceManager == null)
        {
            flexPlaceManager = transform.GetChild((int)MenuIndex.FLEXPLACE).GetComponent<FlexPlaceManager>();
            //flexPlaceManager.Init(allCanvas[(int)CanvasIndex.FLEXPLACE]);
        }
        if(inputManager == null)
        {
            inputManager = transform.GetChild((int)MenuIndex.INPUT).GetComponent<InputManager>();
        }
        if(timeManager == null)
        {
            timeManager = transform.GetChild((int)MenuIndex.TIME).GetComponent<TimeManager>();
        }
        if (topUIManager == null)
        {
            topUIManager = transform.GetChild((int)MenuIndex.TOPUI).GetComponent<TopUIManager>();
        }
    }

    


    #region 홈 화면 버튼 이벤트
  

    //Back Button을 위한 UIView Push
   
    #region HOMEUI BUTTON
    //홈 화면에 있는 버튼들을 눌렀했을 때 실행되는 이벤트
    public void OnClickHomeUIButton(GetEnums _index)
    {
        if(isFading == false)
        {
           
            StartCoroutine(OpenHomeUI(_index));
        }
    }
    IEnumerator OpenHomeUI(GetEnums _index)               // 홈 화면에 버튼에 따른 장면 전환
    {
        isFading = true; //화면 페이드 아웃
        MyFader.Instance.StartFader(FaderState.FADEOUT,0.5f);


        yield return new WaitForSeconds(0.5f);


        //홈 화면 Alpha 0
        //CanvasSetting(_index, 0, 1, 0);


        //특정 버튼 장면으로 전환
        switch (_index._index)
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
                flexPlaceManager.OpenFlexPlaceUI(HomeMenuButtonIndex.FLEX_01);
                StartButtonAutoSlide_Flex();
                break;
            case HomeMenuButtonIndex.FLEX_02:
                flexPlaceManager.OpenFlexPlaceUI(HomeMenuButtonIndex.FLEX_01);
                StartButtonAutoSlide_Flex();
                break;
            case HomeMenuButtonIndex.FLEX_03:
                flexPlaceManager.OpenFlexPlaceUI(HomeMenuButtonIndex.FLEX_01);
                StartButtonAutoSlide_Flex();
                break;
            case HomeMenuButtonIndex.AGIT_A:
                agitManager.Open_Agit(Agit_Index.AGIT_A);
                StartButtonAutoSlide_Agit();
                break;
            case HomeMenuButtonIndex.AGIT_B:
                agitManager.Open_Agit(Agit_Index.AGIT_B);
                StartButtonAutoSlide_Agit();
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
         

            //현재 페이지 Off
          
          
        }
    }
    IEnumerator SetHomeUI_Back(HomeMenuButtonIndex _index)               // 홈 화면에 버튼에 따른 장면 전환
    {
        isFading = true;
        //화면 페이드 아웃
        MyFader.Instance.StartFader(FaderState.FADEOUT,0.5f);


        yield return new WaitForSeconds(0.5f);

        //뒤로가기 했을 때 열려는 UI 정보
      

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

            //현재 페이지 Off
          

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

     

        isFading = false;

    }

    #endregion



    #endregion

 


    #region 오른쪽 하단 버튼

    public void UserPageButton()
    {
        Debug.Log("User Button Click!!");
    }
   
    //메뉴가 열려있으면 들어가게 함
    public void RightBottomMenuCheck()
    {
        if (isButtonOnTop_Agit)
        {
            StartButtonAutoSlide_Agit();
        }

        if (isButtonOnTop_Flex)
        {
            StartButtonAutoSlide_Flex();
        }
    }

    public void StartButtonAutoSlide_Agit()
    {
        if(isButtonSliding_Agit == false)
        {
            isButtonSliding_Agit = true;
            StartCoroutine(ButtonAutoSlide());
        }
    
    }

    IEnumerator ButtonAutoSlide()
    {
        float startPoint = 0;
        float time = 0;
        float totalTime = 0.3f;

        if(isButtonOnTop_Agit == false)
        {
            startPoint = 0;
        }
        else
        {
            startPoint = 1;
        }
        scrollRect_Agit.verticalNormalizedPosition = startPoint;
        while(true)
        {
            if(time >= totalTime)
            {
                if (isButtonOnTop_Agit == false)
                {
                    isButtonOnTop_Agit = true;
                    scrollRect_Agit.verticalNormalizedPosition = 0;
                }
                else
                {
                    isButtonOnTop_Agit = false;
                    scrollRect_Agit.verticalNormalizedPosition = 1;
                }


                isButtonSliding_Agit = false;
                yield break;
            }

            if (isButtonOnTop_Agit == false)
            {
                scrollRect_Agit.verticalNormalizedPosition = Mathf.Lerp(1, startPoint, time / totalTime);
            }
            else
            {
                scrollRect_Agit.verticalNormalizedPosition = Mathf.Lerp(0, startPoint, time / totalTime);
            }
            time += Time.deltaTime;
            yield return null;
        }
    }

    public void StartButtonAutoSlide_Flex()
    {
        if (isButtonSliding_Flex == false)
        {
            isButtonSliding_Flex = true;
            StartCoroutine(ButtonAutoSlide2());
        }

    }

    

    IEnumerator ButtonAutoSlide2()
    {
        float startPoint = 0;
        float time = 0;
        float totalTime = 0.5f;

        if (isButtonOnTop_Flex == false)
        {
            startPoint = 0;
        }
        else
        {
            startPoint = 1;
        }
        scrollRect_Flex.verticalNormalizedPosition = startPoint;
        while (true)
        {
            if (time >= totalTime)
            {
                if (isButtonOnTop_Flex == false)
                {
                    isButtonOnTop_Flex = true;
                    scrollRect_Flex.verticalNormalizedPosition = 0;
                }
                else
                {
                    isButtonOnTop_Flex = false;
                    scrollRect_Flex.verticalNormalizedPosition = 1;
                }


                isButtonSliding_Flex = false;
                yield break;
            }

            if (isButtonOnTop_Flex == false)
            {
                scrollRect_Flex.verticalNormalizedPosition = Mathf.Lerp(1, startPoint, time / totalTime);
            }
            else
            {
                scrollRect_Flex.verticalNormalizedPosition = Mathf.Lerp(0, startPoint, time / totalTime);
            }
            time += Time.deltaTime;
            yield return null;
        }
    }




    #endregion


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
    #endregion

}
