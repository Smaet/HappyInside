using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using BackEnd;

public class FacebookManager : MonoBehaviour
{

    private void Start()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(OnInitComplete, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
        TitleManager.Instance.Facebook_LoginClick += FacebookLogin;
        TitleManager.Instance.Facebook_GetLoginInfo += GetFacebookLoginInfo;
        TitleManager.Instance.Facebook_LoginOutClick += FacebookLogOut;
    }

    public void FacebookLogOut()
    {
        FB.LogOut();
    }

    public void FacebookLogin()
    {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);

        //FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, this.HandleResult);
    }

    private void AuthCallback(ILoginResult result)
    {
        print("LoginSuccess!!");

        // 로그인 성공
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User IDDebug.Log(aToken);
            string facebookToken = aToken.TokenString;
            // 뒤끝 서버에 획득한 페이스북 토큰으로 가입요청
            BackendReturnObject bro = Backend.BMember.AuthorizeFederation(facebookToken, FederationType.Facebook);


            if(bro.IsSuccess())
            {
                //기존 회원 로그인 성공
                if (bro.GetStatusCode() == "200")
                {
                    print("기존 회원으로 로그인 성공!!");

                    GetFacebookLoginInfo();

                    GameManager.Instance.SetLoginType(LoginType.FACEBOOK);
                }
                //신규 회원 로그인 성공
                else if(bro.GetStatusCode() == "201")
                {
                    print("신규 회원으로 로그인 성공!!");

                    TitleManager.Instance.ShowUIView_NickName();

                    GameManager.Instance.SetLoginType(LoginType.FACEBOOK);
                }
            }
           

        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }


    public void GetFacebookLoginInfo()
    {
        BackendReturnObject BRO = Backend.BMember.LoginWithTheBackendToken();


        if (BRO.IsSuccess())
        {
            BackendReturnObject bro = Backend.BMember.GetUserInfo();


            string nickname = bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();

            print("저장된 닉네임 : " + nickname);

            TitleManager.Instance.uiView_FingerPrint.Show();

        }
    }

    private void OnInitComplete()
    {
        print("Init Success!!");
        //초기화에 성공시에 로그인 화면으로 넘어감

        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }


}
