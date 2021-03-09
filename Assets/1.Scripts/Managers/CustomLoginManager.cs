using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class CustomLoginManager : MonoBehaviour
{
    private void Start()
    {
        TitleManager.Instance.Custom_LoginClick += CustomLoginSignUp;
        TitleManager.Instance.Custom_GetLoginInfo += GetCustomLoginInfo;
    }

    public void GetCustomLoginInfo()
    {
        BackendReturnObject bro = Backend.BMember.LoginWithTheBackendToken();
        if (bro.IsSuccess())
        {
            Debug.Log("자동 로그인에 성공했습니다");

            BackendReturnObject bro_userInfo = Backend.BMember.GetUserInfo();
            string nickname = bro_userInfo.GetReturnValuetoJSON()["row"]["nickname"].ToString();
            print(nickname);

            ShowChart();



            GameManager.Instance.SetUserInfo(nickname);

            TitleManager.Instance.ShowUIView_FingerPrint();

        }
    }

    public void ShowChart()
    {
        BackendReturnObject bro_chart = Backend.Chart.GetChartList();

        string selectedChartFileId = "";

        if (bro_chart.IsSuccess())
        {
            selectedChartFileId = bro_chart.GetReturnValuetoJSON()["rows"][0]["selectedChartFileId"]["N"].ToString();
            print(selectedChartFileId);
        }
        BackendReturnObject bro_chart_get = Backend.Chart.GetChartContents(selectedChartFileId);

        if (bro_chart_get.IsSuccess())
        {

        }
    }

    public void CustomLoginSignUp(string _id, string _pw)
    {
        BackendReturnObject bro = Backend.BMember.CustomSignUp(_id, _pw);
        if (bro.IsSuccess())
        {
            Debug.Log("회원가입에 성공했습니다");

            //신규 회원 로그인 성공
            if (bro.GetStatusCode() == "201")
            {
                print("신규 회원으로 로그인 성공!!");

                CustomLogin(_id, _pw);

                PlayerPrefs.SetString("CustomID", _id);
                PlayerPrefs.SetString("CustomID", _pw);

                GameManager.Instance.SetLoginType(LoginType.CUSTOM);


                //기본 데이터 셋팅
            }
        }
    }

    public void CustomLogin(string _id , string _pw)
    {
        BackendReturnObject bro = Backend.BMember.CustomLogin(_id, _pw);
        if (bro.IsSuccess())
        {
            TitleManager.Instance.HideCustomLogin();

            TitleManager.Instance.ShowUIView_NickName();



            Debug.Log("커스텀 계정 로그인에 성공했습니다");
        }
    }

    
}
