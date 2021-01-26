using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using UnityEngine;
using Random = UnityEngine.Random;
using Doozy.Engine.UI;
using Febucci.UI;
using Febucci.UI.Core;
using UnityEngine.UI;


public class KitchenGameManager : MonoBehaviour
{
    public static KitchenGameManager instance;

    enum State
    {
        None,
        Request, // 할아버지의 대사
        Answer,  // 주인공 음식 버튼 선택
        Result   // 결과
    }


    [SerializeField] private KitchenButton[] FoodButtons;
    [SerializeField] private Sprite[] FoodSprites;
    [SerializeField] private int FoodCount;


    // UIView,,,
    [SerializeField] private UIView uiView_Grandfather;
    [SerializeField] private UIView uiView_GameStart;
    [SerializeField] private UIView uiView_FoodSelectButton;
    [SerializeField] private UIView uiView_ResultScreen;

    // TextAnimator
    public TextAnimatorPlayer textAnimatorPlayer;


    // Grandfather Scripts
    [TextArea(3, 50), SerializeField] private string scr_Welcome;
    [TextArea(3, 50), SerializeField] private string scr_Menu_01;
    [TextArea(3, 50), SerializeField] private string scr_Menu_02;
    [TextArea(3, 50), SerializeField] private string scr_Menu_03;
    [TextArea(3, 50), SerializeField] private string scr_Menu_04;
    [TextArea(3, 50), SerializeField] private string scr_Menu_05;
    [TextArea(3, 50), SerializeField] private string scr_Result_Good;
    [TextArea(3, 50), SerializeField] private string scr_Result_Bad;



    // 게임 상태
    [SerializeField] private State kitchenState;

    // 현재 게임 메뉴 보기
    [SerializeField] private int[] FilteredMenu;
    private int FilteredMenuCount;

    // 현재 게임 정답 메뉴
    [SerializeField] private int CorrectMenu;


    private bool isInit = false;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    kitchenState = State.None;
      

    //    // -1 is None 
    //    CorrectMenu = -1;

    //    Initialize();
    //}


    //  ->  State.None
    public void Initialize()
    {
        if(isInit == false)
        {
            FilteredMenu = new int[3];
            FilteredMenuCount = 3;
            isInit = true;
        }

        kitchenState = State.None;
        CorrectMenu = -1;

        Array.Clear(FilteredMenu,0,FilteredMenu.Length);
        for (int i = 0; i < FilteredMenuCount; i++)
        {
            FilteredMenu[i] = -1;
        }


        uiView_GameStart.Show();
        uiView_Grandfather.Show();
        uiView_FoodSelectButton.Hide();
        uiView_ResultScreen.Hide();


        // 할아버지 멘트 ㄱㄱ  
        GrandfatherSaid(scr_Welcome);
    }

    // 할아버지의 연설   -> State.Request
    public void GrandfaRequest()
    {
        if (kitchenState == State.None)
        {
            kitchenState = State.Request;

            // 후보 메뉴 선정 , 정답 설정
            int temp = 0;
            int idx = 0;

            while (CorrectMenu == -1)
            {

                // 전체 메뉴 중 이번 게임 메뉴 고르기 (3개)
                temp = Random.Range(0, FoodCount);

                if (FilteredMenu.Contains(temp))
                {
                    continue;
                }
                else
                {
                    FilteredMenu[idx] = temp;
                    idx++;
                }


                // 다 골랐으면 정답 고르기
                if (idx >= FilteredMenuCount)
                {
                    temp = Random.Range(0, FilteredMenuCount);
                    CorrectMenu = FilteredMenu[temp];
                }
            }


            // Debug
            string debugTxt = "Menu : ";
            foreach (var elem in FilteredMenu)
            {
                debugTxt += elem + ", ";
            }

            debugTxt += "Correct Menu : " + CorrectMenu;
            Debug.Log(debugTxt);
            // Debug End



            // 버튼에 메뉴 세팅
            for (int i = 0; i < FilteredMenuCount; i++)
            {
                FoodButtons[i].SetFoodToButton(FilteredMenu[i], FoodSprites[FilteredMenu[i]]);
            }


            // UiView On
            uiView_GameStart.Hide();
            uiView_Grandfather.Show();

            // 할아버지 멘트 ㄱㄱ  
            GrandfatherSaid(GetFoodScript(CorrectMenu));

            StartCoroutine(DelayNextStep(2.0f));
        }
    }



    // 메뉴 선택의 시간(멘트 끝날 때 이벤트 발동)   -> State.Answer
    public void SelectFood()
    {
        kitchenState = State.Answer;

        // 버튼 Show
        uiView_FoodSelectButton.Show();

    }

    // 메뉴 선택  ,  Button Method
    public void CorrectCheck(int foodidx)
    {
        // 버튼 Hide
        uiView_FoodSelectButton.Hide();
        //uiView_Grandfather.Hide();

        // 정답체크
        if (CorrectMenu == foodidx)
        {
            ShowResult(true);
        }
        else
        {
            ShowResult(false);
        }
    }



    // 결과 보기   -> State.Result
    public void ShowResult(bool Success)
    {
        kitchenState = State.Result;


        // 결과멘트 ㄱ
        if(Success) GrandfatherSaid(scr_Result_Good);
        else GrandfatherSaid(scr_Result_Bad);


        // 결과 창 세팅
        if (Success)
        {
            uiView_ResultScreen.gameObject.GetComponentInChildren<Text>().text = "성공";

       
            if (GameManager.Instance.user.userBaseProperties.buffs[(int)BuffIndex.GRANDFATHER].isActive == false)
            {
                Buff GrandFather = new Buff();
                GrandFather.isActive = true;
                GrandFather.isGood = true;
                GrandFather.isRunning = true;
                GrandFather.isBuffed = false;
                GrandFather.remainTime = GameManager.Instance.user.userBaseProperties.buffs[(int)BuffIndex.GRANDFATHER].continueTime;
                GrandFather.continueTime = GameManager.Instance.user.userBaseProperties.buffs[(int)BuffIndex.GRANDFATHER].continueTime;
                GrandFather.effect_Doubt_Plus = GameManager.Instance.user.userBaseProperties.buffs[(int)BuffIndex.GRANDFATHER].effect_Doubt_Plus;
                GrandFather.effect_Doubt_Minus = GameManager.Instance.user.userBaseProperties.buffs[(int)BuffIndex.GRANDFATHER].effect_Doubt_Minus;

                GameManager.Instance.user.SetUserInfo(BuffIndex.GRANDFATHER, GrandFather);
             
                HomeManager.Instance.timeManager.StartGrandFatherBuff();

            }
            
        }
        else
        {
            uiView_ResultScreen.gameObject.GetComponentInChildren<Text>().text = "실패";
            if (GameManager.Instance.user.userBaseProperties.buffs[(int)BuffIndex.GRANDFATHER].isActive == false)
            {
                Buff GrandFather = new Buff();
                GrandFather.isActive = true;
                GrandFather.isGood = false;
                GrandFather.isRunning = true;
                GrandFather.isBuffed = false;
                GrandFather.remainTime = GameManager.Instance.user.userBaseProperties.buffs[(int)BuffIndex.GRANDFATHER].continueTime;
                GrandFather.continueTime = GameManager.Instance.user.userBaseProperties.buffs[(int)BuffIndex.GRANDFATHER].continueTime;
                GrandFather.effect_Doubt_Plus = GameManager.Instance.user.userBaseProperties.buffs[(int)BuffIndex.GRANDFATHER].effect_Doubt_Plus;
                GrandFather.effect_Doubt_Minus = GameManager.Instance.user.userBaseProperties.buffs[(int)BuffIndex.GRANDFATHER].effect_Doubt_Minus;

                GameManager.Instance.user.SetUserInfo(BuffIndex.GRANDFATHER, GrandFather);
                
                HomeManager.Instance.timeManager.StartGrandFatherBuff();
            }



        }





        // UIView On
        uiView_ResultScreen.Show();


    }



    #region Support and Temporary
    // 할아버지 말씀
    private void GrandfatherSaid(string txt)
    {
        textAnimatorPlayer.ShowText(txt);
    }

    // (임시) 음식 멘트 매칭
    private string GetFoodScript(int correct_idx)
    {
        string val;

        switch (correct_idx)
        {
            case 0:
                val = scr_Menu_01;
                break;
            case 1:
                val = scr_Menu_02;
                break;
            case 2:
                val = scr_Menu_03;
                break;
            case 3:
                val = scr_Menu_04;
                break;
            case 4:
                val = scr_Menu_05;
                break;

            default:
                val = "??? check plz";
                break;
        }

        return val;
    }

    // 텍스트애니메이터 에셋 이벤트 핸들링 아직 모르겠음 , 급하게 쓰는 용도
    IEnumerator DelayNextStep(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SelectFood();
    }


    #endregion



}
