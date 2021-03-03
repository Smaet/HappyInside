
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
    }

  
    public void GPGSLogin()
    {
#if UNITY_ANDROID
        print("GPGS Login Button Clicked!!");


        // 이미 로그인 된 경우
        if (Social.localUser.authenticated == true)
        {
            BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");
        }
        else
        {
            Social.localUser.Authenticate((bool success) => {
                if (success)
                {
                    // 로그인 성공 -> 뒤끝 서버에 획득한 구글 토큰으로 가입요청
                    BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");
                   
                    TitleManager.Instance.ShowUIView_Login();

                    print("BRO Status : " + BRO.GetStatusCode());

                    print("Google Hash : " + Backend.Utils.GetGoogleHash());

                    //string nickname = BRO.GetReturnValuetoJSON()["row"]["nickname"].ToString();
                    //string countryCode = BRO.GetReturnValuetoJSON()["row"]["countryCode"].ToString();
                    //string inDate = BRO.GetReturnValuetoJSON()["row"]["inDate"].ToString();
                    //string emailForFindPassword = BRO.GetReturnValuetoJSON()["row"]["emailForFindPassword"].ToString();
                    //string subscriptionType = BRO.GetReturnValuetoJSON()["row"]["subscriptionType"].ToString();
                    //string federationId = BRO.GetReturnValuetoJSON()["row"]["federationId"].ToString();
                    //print("User NickName : " + nickname);
                    //print("User countryCode : " + countryCode);
                    //print("User inDate : " + inDate);
                    //print("User emailForFindPassword : " + emailForFindPassword);
                    //print("User subscriptionType : " + subscriptionType);
                    //print("User federationId : " + federationId);

                    //Backend.BMember.AuthorizeFederation(federationId, FederationType.Google, "GPGS로 가입함");
                }
                else
                {
                    // 로그인 실패
                    Debug.Log("Login failed for some reason");
                }
            });
        }



        //// 이미 로그인 된 경우
        //if (Social.localUser.authenticated == true)
        //{
           
        //}
        //else
        //{
        //    Social.localUser.Authenticate((bool success) => {
        //        if (success)
        //        {
        //            // 로그인 성공 -> 뒤끝 서버에 획득한 구글 토큰으로 가입요청
        //            Debug.Log("Login success!! " + Social.localUser.id + " / " + Social.localUser.userName );
        //            text.text += Social.localUser.userName;

        //            TitleManager.Instance.ShowUIView_Login();

        //        }
        //        else
        //        {
        //            // 로그인 실패
        //            Debug.Log("Login failed for some reason");
        //        }
        //    });
        //}
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
