/*
  2021.01.05 Created 
  TitleManager.cs   
  기능
  1. 로그인
  2. 타이틀 관련 UI
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Touch;
using Doozy.Engine.UI;
using TMPro;

using BackEnd;


public class TitleManager : SimpleSingleton<TitleManager>
{
    public TMP_InputField inputField_Nick;

    public Slider slider_FingerPrintProgress;
    public TextMeshProUGUI tmp_FingerPrintProgress_TextAnim;
    public TextMeshProUGUI tmp_FingerPrintProgress_Percent;
    private int fingerPrintProgressAnimateCount = 0;
    private int fingerPrintProgress = 0;
    public bool isFingerPrintProgressTextAnimating;

    public Button button_FaceBook;
    public Button button_Google;


    public UIView uiView_Login;
    public Button button_LoginConfirm;

    public UIView uiView_FingerPrint;

    bool isLoadScene = false;

    public TextMeshProUGUI TMP_LogText;

    public event Action GPGS_LoginClick;
    public event Action GPGS_LoginOutClick;


    public event Action Facebook_LoginClick;
    public event Action Facebook_LoginOutClick;

    protected override  void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        isLoadScene = false;
        button_FaceBook.onClick.RemoveAllListeners();
        button_Google.onClick.RemoveAllListeners();
        button_LoginConfirm.onClick.RemoveAllListeners();

        button_FaceBook.onClick.AddListener(ClickGPGSLogOut);
        button_Google.onClick.AddListener(ClickGPGSLogin);
        button_LoginConfirm.onClick.AddListener(ShowUIView_FingerPrint);

        //바로 터치 스크린
        if (GameManager.Instance.user.isFirst == false)
        {
            button_Google.gameObject.SetActive(false);
            button_FaceBook.gameObject.SetActive(false);
        }
        //로그인 버튼 생성
        else
        {
            button_Google.gameObject.SetActive(true);
            button_FaceBook.gameObject.SetActive(true);
        }
        tmp_FingerPrintProgress_TextAnim.text = "지문 인식중.";
        tmp_FingerPrintProgress_Percent.text = "0%";
        isFingerPrintProgressTextAnimating = false;
        TMP_LogText.text = "";

    }

    public void ClickGPGSLogin()
    {
        if(GPGS_LoginClick != null)
        {

#if UNITY_ANDROID
            GPGS_LoginClick();
#endif

#if UNITY_EDITOR
            ShowUIView_Login();
#endif

        }
    }
    public void ClickGPGSLogOut()
    {
        if (GPGS_LoginOutClick != null)
        {
            GPGS_LoginOutClick();
        }
    }

    public void ClickFacebookLogin()
    {
        if (Facebook_LoginClick != null)
        {

#if UNITY_ANDROID
            Facebook_LoginClick();
#endif

#if UNITY_EDITOR
            ShowUIView_Login();
#endif

        }
    }
    public void ClickFacebookLogOut()
    {
        if (Facebook_LoginOutClick != null)
        {
            Facebook_LoginOutClick();
        }
    }

    public void LoadScene()
    {
        Debug.Log("Load GameScene!!");
        MySceneLoader.Instance.LoadScene("1.GameScene_DoozyUI");
    }


    IEnumerator TextAnim()
    {
        print("Start TextAnim!!");

        while(isFingerPrintProgressTextAnimating)
        {
            if (fingerPrintProgressAnimateCount == 0)
            {
                tmp_FingerPrintProgress_TextAnim.text = "지문 인식중. ";
                fingerPrintProgressAnimateCount++;
            }
            else if (fingerPrintProgressAnimateCount == 1)
            {
                tmp_FingerPrintProgress_TextAnim.text = "지문 인식중.. ";
                fingerPrintProgressAnimateCount++;
            }
            else if (fingerPrintProgressAnimateCount == 2)
            {
                tmp_FingerPrintProgress_TextAnim.text = "지문 인식중... ";
                fingerPrintProgressAnimateCount = 0;
            }

            

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void PressFingerUpdate(LeanFinger _finger)
    {
        print("Press Finger!!");

        if(isFingerPrintProgressTextAnimating == false)
        {
            isFingerPrintProgressTextAnimating = true;
            StartCoroutine(TextAnim());
        }

        if (slider_FingerPrintProgress.value < 1f)
        {
            slider_FingerPrintProgress.value += Time.deltaTime / 3f;
            fingerPrintProgress = (int)(slider_FingerPrintProgress.value * 100);
            tmp_FingerPrintProgress_Percent.text = fingerPrintProgress + "%";
        }
        else
        {
            if(isLoadScene == false)
            {
                isLoadScene = true;
                print("Finger Print Complete!!");
                uiView_FingerPrint.Hide();
                LoadScene();
            }
         
        }
        
    }

    public void PressFingerDeSelect()
    {
        if(isFingerPrintProgressTextAnimating)
        {
            isFingerPrintProgressTextAnimating = false;
        }

        print("DeSelected!!");
        if(slider_FingerPrintProgress.value < 1f)
        {
            slider_FingerPrintProgress.value = 0;
            tmp_FingerPrintProgress_TextAnim.text = "지문 인식중.";
            tmp_FingerPrintProgress_Percent.text = "0%";
            fingerPrintProgressAnimateCount = 0;
        }
    }

    public void ShowUIView_Login()
    {
        uiView_Login.Show();
    }

    void ShowUIView_FingerPrint()
    {
        BackendReturnObject bro =  Backend.BMember.CreateNickname(inputField_Nick.text);

        inputField_Nick.text = "";

        // 이후 처리
        string statusCode = bro.GetStatusCode();

        print(statusCode);

        if (bro.IsSuccess())
        {
            uiView_Login.Hide();
            uiView_FingerPrint.Show();
            GameManager.Instance.SetUserInfo(inputField_Nick.text);
            inputField_Nick.text = "";

            TMP_LogText.text = "Nick Input Success!!";
        }
        else
        {
            //빈 닉네임 혹은 string.empty로 닉네임 생성&수정을 시도 한 경우
            //20자 이상의 닉네임인 경우
            //닉네임에 앞/뒤 공백이 있는 경우
            if (statusCode == "400")
            {
                TMP_LogText.text = "";
                print(bro.GetMessage());
                TMP_LogText.text = bro.GetMessage();
            }
            //이미 중복된 닉네임이 있는 경우
            else if (statusCode == "409")
            {
                TMP_LogText.text = "";
                print(bro.GetMessage());
                TMP_LogText.text = bro.GetMessage();
            }

        }


    }
}
