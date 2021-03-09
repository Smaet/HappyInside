
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using BackEnd;

public class GoogleManager : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
        .Builder()
        .RequestServerAuthCode(false)
        .RequestIdToken()
        .Build();
        //커스텀된 정보로 GPGS 초기화
        PlayGamesPlatform.InitializeInstance(config);

        PlayGamesPlatform.DebugLogEnabled = true;
        //GPGS 시작.
        PlayGamesPlatform.Activate();

        TitleManager.Instance.GPGS_LoginClick += GPGSLogin;
        TitleManager.Instance.GPGS_LoginOutClick += LogOut;
        TitleManager.Instance.GPGS_GetLoginInfo += GetGPGSLoginInfo;
    }

    public void GetGPGSLoginInfo()
    {

        BackendReturnObject BRO = Backend.BMember.LoginWithTheBackendToken();

        //유저 정보 가져오기 및 적용 후 지문 인식으로 넘어가기.
        print("BRO Status : " + BRO.GetStatusCode());

        if (BRO.IsSuccess())
        {

            BackendReturnObject bro = Backend.BMember.GetUserInfo();


            string nickname = bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();

            print("저장된 닉네임 : " + nickname);

            TitleManager.Instance.uiView_FingerPrint.Show();


        }
    }
  
    public void GPGSLogin()
    {
#if UNITY_ANDROID
        print("GPGS Login Button Clicked!!");


        // 이미 로그인 된 경우
        if (Social.localUser.authenticated == true)
        {
            
        }
        else
        {
            Social.localUser.Authenticate((bool success) => {
                if (success)
                {  
                    // 로그인 성공 -> 뒤끝 서버에 획득한 구글 토큰으로 가입요청
                    BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");


                    if(BRO.IsSuccess())
                    {
                        string statusCode = BRO.GetStatusCode();

                        print("BackEnd Login Success : " + statusCode);

                        //기존 회원 로그인
                        if(statusCode == "200")
                        {
                            print("기존 회원으로 로그인 성공!!");

                            GetGPGSLoginInfo();

                            GameManager.Instance.SetLoginType(LoginType.GOOGLE);
                        }
                        else
                        {
                            print("신규 회원으로 로그인 성공!!");

                            TitleManager.Instance.ShowUIView_NickName();

                            GameManager.Instance.SetLoginType(LoginType.GOOGLE);
                        }
                    }

                }
                else
                {
                    // 로그인 실패
                    Debug.Log("Login failed for some reason");
                }
            });
        }
#endif
    }

    public void LogOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();

        var bro = Backend.BMember.Logout();
        if (bro.IsSuccess())
        {
            Debug.Log("로그 아웃 성공");

        }
        else
        {
            Debug.Log("로그아웃 실패 - " + bro.GetErrorCode());
        }

        Debug.Log("Log Out !!!!");
    }



    // 구글 토큰 받아옴
    public string GetTokens()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // 유저 토큰 받기 첫번째 방법
            string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();

            print("UserToken : " + _IDtoken);

            // 두번째 방법
            // string _IDtoken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
            return _IDtoken;
        }
        else
        {
            Debug.Log("접속되어있지 않습니다. PlayGamesPlatform.Instance.localUser.authenticated :  fail");
            return null;
        }
    }
}
