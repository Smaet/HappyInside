/*
  2021.01.05 Created 
  GameManager.cs   
  규칙
  1.변수는 소문자로 시작해서 구분하기 위한 부분에 대문자를 넣어서 사용
  2.함수의 매개 변수는 '_'을 변수 앞에 붙이기 -> Example(int _index) {}
  3.각각의 버튼 이벤트들은 해당 버튼의 상위 클래스에서 관리.
  4.UI 관련 변수는 앞에 소문자로 UI 명을 붙이고 다음에 해당변수 이름 사용 -> Image image_Player
  
  기능
  1. 유저 정보 관리
  2. 서버와 통신
  3. 사운드

*/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using BackEnd;


public enum LoginType
{
    NONE = 0,
    GOOGLE,
    FACEBOOK,
    CUSTOM,
}


public class GameManager : SimpleSingleton<GameManager>
{
    public bool isTest = false;

    [Header("User"), SerializeField] 
    public User user;

    public bool isSave = false;


    // Score
    [SerializeField] private float DepthScore;
    public float DepthMultiple;

    public enum GameState
    {
        Ready,
        Playing,
        NotPlaying
    }

    public GameState CurrentGameState;

    //서버에서 받아온 인게임 저장 정보
    //동료 레벨당 정보
    //public List<>


    protected override void Awake()
    {
        DontDestroyOnLoad(this);

        base.Awake();

        //뒤끝 SDK 초기화
        Backend.Initialize(callback => {
            if (callback.IsSuccess())
            {
                // 초기화 성공 시 로직
                print("BackEnd SDK Initialize Success!!");
            }
            else
            {
                // 초기화 실패 시 로직
                print("BackEnd SDK Initialize fail!!");
            }
        });

     

        if (isTest)
        {
            TestUser();
        }
        else
        {
            ClearUserData();

            user.isFirst = true;
        }
    }

    void TestUser()
    {
        ClearUserData();

        SetUserInfo("로빈후드");
    }

    private void Start()
    {
      

        //PlayerPrefs.DeleteKey("Login");

        //이전에 로그인을 했는지 아닌지 체크하기
        //PlayerPrefs Int 
        //0 : 로그인 안함
        //1 : 구글 로그인
        //2 : 페이스북 로그인
        //3 : 게스트 로그인

        //로그인이 한번이라도 되었는지 확인
        //로그인이 한번이라도 했으면 바로 Touch Screen을 보여주고
#if UNITY_EDITOR
            if (PlayerPrefs.HasKey("Login"))
        {
            if(PlayerPrefs.GetInt("Login") == (int)LoginType.CUSTOM)
            {
                TitleManager.Instance.HideLoginButtons();
                TitleManager.Instance.SetTouchScreen(true);
            }
        }
        //아니라면 SNS 와 게스트 로그인 버튼을 띄워 준다.
        else
        {
            TitleManager.Instance.ShowLoginButtons();
            TitleManager.Instance.SetTouchScreen(false);
        }


#elif UNITY_ANDROID

        if (PlayerPrefs.HasKey("Login"))
        {
            TitleManager.Instance.HideLoginButtons();
            TitleManager.Instance.SetTouchScreen(true);
        }
        //아니라면 SNS 와 게스트 로그인 버튼을 띄워 준다.
        else
        {
            TitleManager.Instance.ShowLoginButtons();
            TitleManager.Instance.SetTouchScreen(false);
        }

#endif

    }

    #region GameControl

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion

    #region UserData
    public void SetUserInfo(string _nick)
    {
        if(LoadUserData() == false)
        {
            print("초기 데이터로 시작!");
            User tempUserData = new User();
            //user.SetNick("HappyRiccc22222");

            tempUserData.isFirst = true;

            tempUserData.userBaseProperties = new UserBaseProperties();
            tempUserData.userBaseProperties.nickName = _nick;
            tempUserData.userBaseProperties.crystal = 99;
            //tempUserData.userBaseProperties.startMoney = 50000000000;
            tempUserData.userBaseProperties.currentAmount = HappyRichReadOnly.StartGrandFatherMoney;
          

            tempUserData.userBaseProperties.xCoin = 1000;

            tempUserData.userBaseProperties.collegueInfos = new CollegueInfo[5];

            tempUserData.userBaseProperties.buffs = new List<Buff>();
     

            for (int i = 0; i < tempUserData.userBaseProperties.collegueInfos.Length; i++)
            {
                tempUserData.userBaseProperties.collegueInfos[i] = new CollegueInfo();
                if (i == (int)CollegueIndex.Dare)
                {   
                    tempUserData.userBaseProperties.collegueInfos[i].isActive = true;

                    tempUserData.userBaseProperties.collegueInfos[i].Level = 1;
                    //tempUserData.userBaseProperties.collegueInfos[i].itemLevel = 1;
                    tempUserData.userBaseProperties.collegueInfos[i].deviceLevel = 1;
                    //tempUserData.userBaseProperties.collegueInfos[i].collegueBasicSkill = new CollegueBasicSkill();
                    //tempUserData.userBaseProperties.collegueInfos[i].collegueBasicSkill.hour = 6;
                    //tempUserData.userBaseProperties.collegueInfos[i].collegueBasicSkill.money = 10000000;
                    //tempUserData.userBaseProperties.collegueInfos[i].collegueBasicSkill.day = -1;
                    //tempUserData.userBaseProperties.collegueInfos[i].collegueBasicSkill.chance = -1;

                    //tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills = new ColleguePassiveSkill[3];

                    for (int j = 0; j < 3; j++)
                    {
                       // tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills[j] = new ColleguePassiveSkill();
                    }

                    //tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills[0].chance = 10;
                    //tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills[1].chance = 20;
                    //tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills[2].chance = 30;

           

                }
                else
                {
                    tempUserData.userBaseProperties.collegueInfos[i].isActive = true;

                    tempUserData.userBaseProperties.collegueInfos[i].Level = 0;
                    //tempUserData.userBaseProperties.collegueInfos[i].itemLevel = 0;
                    tempUserData.userBaseProperties.collegueInfos[i].deviceLevel = 0;
                    //tempUserData.userBaseProperties.collegueInfos[i].collegueBasicSkill = new CollegueBasicSkill();
                    //tempUserData.userBaseProperties.collegueInfos[i].collegueBasicSkill.hour = 6;
                    //tempUserData.userBaseProperties.collegueInfos[i].collegueBasicSkill.money = 10000000;
                    //tempUserData.userBaseProperties.collegueInfos[i].collegueBasicSkill.day = -1;
                    //tempUserData.userBaseProperties.collegueInfos[i].collegueBasicSkill.chance = -1;

                    //tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills = new ColleguePassiveSkill[3];

                    for (int j = 0; j < 3; j++)
                    {
                       // tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills[j] = new ColleguePassiveSkill();
                    }

                    //tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills[0].chance = 10;
                    //tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills[1].chance = 20;
                    //tempUserData.userBaseProperties.collegueInfos[i].colleguePassiveSkills[2].chance = 30;
                }
            }
            user.SetUserInfo(tempUserData);


            Param param = new Param();
            param.Add("crystal", user.userBaseProperties.crystal);
            param.Add("xCoin", user.userBaseProperties.xCoin);
            param.Add("dessert", user.userBaseProperties.dessert);
            param.Add("currentAmount", user.userBaseProperties.currentAmount);
            param.Add("Inventory", user.userBaseProperties.happyRichItems);
            param.Add("CollegueInfos", user.userBaseProperties.collegueInfos);
            param.Add("Buffs", user.userBaseProperties.buffs);

            Backend.GameInfo.Insert("happyRich_Character", param);
        }
        else
        {
            Debug.Log("초기 데이터로 시작!");
        }

        //Debug.Log(localUser.nickName);

        


    }

    #endregion

    #region Buff
    public void AddBuff_GrandFather(bool _isGood)
    {
        Buff buff = new Buff();
        buff.icon = 0;
        buff.buffIndex = BuffIndex.GRANDFATHER;
        buff.isActive = false;
        buff.isGood = _isGood;
        buff.isBuffed = false;

        buff.totalContinueTime = 12;
        buff.remainTime = buff.totalContinueTime;
        buff.effect_Doubt_Plus = -5;
        buff.effect_Doubt_Minus = 5;

        user.userBaseProperties.buffs.Add(buff);

       
    }
    #endregion

    #region Util

    public string GetMoneyFormat(long _money)
    {
        string result = "";

        long Billion = (_money);
        string Billion_str = "";
        string TenThousand_str = "";

        var Bill = (Billion % 100000000000) / 100000000;
        var Thou = (Billion % 100000000) / 10000;

        if (Billion >= 100000000)
        {
            Billion_str = string.Format("{0}억", (Billion % 100000000000) / 100000000);
        }
        if (Billion >= 10000)
        {
            TenThousand_str = string.Format("{0}천", (Billion % 100000000) / 10000000);
        }

        if (Bill != 0)
        {
            result +=  Billion_str;  
        }
        if(Thou != 0)
        {
            result +=  " " + TenThousand_str;
        }

        return result;
    }

    public string GetMoneyFormat(float _money)
    {
        string result = "";

        float Billion = (_money);
        string Billion_str = "";
        string TenThousand_str = "";

        var Bill = (Billion % 100000000000) / 100000000;
        var Thou = (Billion % 100000000) / 10000;

        if (Billion >= 100000000)
        {
            Billion_str = string.Format("{0}억", (Billion % 100000000000) / 100000000);
        }
        if (Billion >= 10000)
        {
            TenThousand_str = string.Format("{0}천만원", (Billion % 100000000) / 10000);
        }

        if (Thou != 0)
        {
            result = Billion_str;
        }
        else
        {
            result = Billion_str + " " + TenThousand_str;
        }

        return result;
    }
    #endregion



    #region LocalSave

   

    //0 : 로그인 안함
    //1 : 구글 로그인
    //2 : 페이스북 로그인
    //3 : 게스트 로그인
    public void SetLoginType(LoginType _type)
    {
        PlayerPrefs.SetInt("Login", (int)_type);

        
    }
    public void SaveUserData()
    {
        ES3.Save("localUser",user);

        if (ES3.KeyExists("localUser"))
        {
            Debug.Log("저장 완료");
        }
        else
        {
            Debug.Log("저장 실패");
        }

    }

    public bool LoadUserData()
    {
        if(ES3.KeyExists("localUser"))
        {
            user = ES3.Load<User>("localUser");
            return true;
        }
        else
        {
            return false;
        }

        

       
    }

    public void ClearUserData()
    {
        if(ES3.KeyExists("localUser"))
        {
            ES3.DeleteKey("localUser");
        }
        else
        {
            Debug.Log("저장된 데이터가 존재하지 않음");
        }
      
    }

    #endregion
}
