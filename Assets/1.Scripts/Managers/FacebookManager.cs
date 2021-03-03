using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FacebookManager : MonoBehaviour
{

    private void Start()
    {
        FB.Init(this.OnInitComplete, this.OnHideUnity);

        TitleManager.Instance.Facebook_LoginClick += FacebookLogin;
        //TitleManager.Instance.GPGS_LoginOutClick += LogOut;
    }

    public void FacebookLogin()
    {
        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, this.HandleResult);
    }
    private void OnInitComplete()
    {
        print("Init Success!!");
        //초기화에 성공시에 로그인 화면으로 넘어감
    }

    private void OnHideUnity(bool isGameShown)
    {
        //this.Status = "Success - Check log for details";
        //this.LastResponse = string.Format("Success Response: OnHideUnity Called {0}\n", isGameShown);
        //LogView.AddLog("Is game shown: " + isGameShown);
    }


    protected void HandleResult(IResult result)
    {
        if (result == null)
        {
            print("Null Response\n");
            return;
        }


        // Some platforms return the empty string instead of null.
        if (!string.IsNullOrEmpty(result.Error))
        {
            print("Error - Check log for details");
            print("Error Response:\n" + result.Error);
        }
        else if (result.Cancelled)
        {
            print("Cancelled - Check log for details");
            print("Cancelled Response:\n" + result.RawResult);
        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            print("Success - Check log for details");
            print("Success Response:\n" + result.RawResult);
            TitleManager.Instance.ShowUIView_Login();
        }
        else
        {
            print("Empty Response\n");
        }

        //LogView.AddLog(result.ToString());
    }

}
