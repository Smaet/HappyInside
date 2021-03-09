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


  
   
    [Header("Common")]
    public Image image_TouchToScreen;
    public Button button_TouchToLogin;


    [Header("UIView")]
    public UIView uiView_CustomLogin;



    public Button button_NickNameConfirm;

    bool isLoadScene = false;

    public TextMeshProUGUI TMP_LogText;

    [Header("NickName")]
    public TMP_InputField inputField_Nick;
    public UIView uiView_NickName;


    [Header("FingerPrint")]
    public UIView uiView_FingerPrint;
    public Slider slider_FingerPrintProgress;
    public TextMeshProUGUI tmp_FingerPrintProgress_TextAnim;
    public TextMeshProUGUI tmp_FingerPrintProgress_Percent;
    private int fingerPrintProgressAnimateCount = 0;
    private int fingerPrintProgress = 0;
    public bool isFingerPrintProgressTextAnimating;

    [Header("GPGSLogin")]
    public Button button_Google;
    public event Action GPGS_LoginClick;
    public event Action GPGS_GetLoginInfo;
    public event Action GPGS_LoginOutClick;

    [Header("FacebookLogin")]
    public Button button_FaceBook;
    public event Action Facebook_LoginClick;
    public event Action Facebook_LoginOutClick;
    public event Action Facebook_GetLoginInfo;

    [Header("CustomLogin")]
    public Button button_CustomLogin;
    public Button button_CustomLoginConfirm;
    public Button button_CustomLoginCancel;

    public TMP_InputField tmpIF_ID;
    public TMP_InputField tmpIF_PW;

    public event Action<string, string> Custom_LoginClick;
    public event Action Custom_LoginOutClick;
    public event Action Custom_GetLoginInfo;

    protected override  void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        isLoadScene = false;

        //Button Init
        button_FaceBook.onClick.RemoveAllListeners();
        button_Google.onClick.RemoveAllListeners();
        button_NickNameConfirm.onClick.RemoveAllListeners();
        button_TouchToLogin.onClick.RemoveAllListeners();
        button_CustomLogin.onClick.RemoveAllListeners();


        button_FaceBook.onClick.AddListener(ClickFacebookLogin);
        button_Google.onClick.AddListener(ClickGPGSLogin);
        button_TouchToLogin.onClick.AddListener(OnClickLoginByType);
        button_NickNameConfirm.onClick.AddListener(ShowUIView_FingerPrint);

       
        tmp_FingerPrintProgress_TextAnim.text = "지문 인식중.";
        tmp_FingerPrintProgress_Percent.text = "0%";
        isFingerPrintProgressTextAnimating = false;
        TMP_LogText.text = "";


        //CustomLogin
        button_CustomLogin.onClick.AddListener(ShowCustomLogin);
        button_CustomLoginCancel.onClick.AddListener(HideCustomLogin);
        button_CustomLoginConfirm.onClick.AddListener(()=> ClickCustomLogin(tmpIF_ID.text, tmpIF_PW.text));


    }


    public void OnClickLoginByType()
    {
        LoginType ltype = (LoginType)PlayerPrefs.GetInt("Login");

        switch(ltype)
        {
            case LoginType.NONE:
                break;
            case LoginType.GOOGLE:
                ClickGetGPGSLoginInfo();
                break;
            case LoginType.FACEBOOK:
                ClickGetFacebookInfo();
                break;
            case LoginType.CUSTOM:
                ClickGetCustomInfo();
                break;
        }
    }

    public void SetTouchScreen(bool isOpen)
    {
        button_TouchToLogin.gameObject.SetActive(isOpen);
        image_TouchToScreen.gameObject.SetActive(isOpen);
    }


    public void ShowLoginButtons()
    {
        button_Google.gameObject.SetActive(true);
        button_FaceBook.gameObject.SetActive(true);
        button_CustomLogin.gameObject.SetActive(true);
    }

    public void HideLoginButtons()
    {
        button_Google.gameObject.SetActive(false);
        button_FaceBook.gameObject.SetActive(false);
        button_CustomLogin.gameObject.SetActive(false);
    }

    public void ClickGetFacebookInfo()
    {
        if(Facebook_GetLoginInfo != null)
        {
            Facebook_GetLoginInfo();
        }
    }

    public void ClickGetGPGSLoginInfo()
    {
        if (GPGS_GetLoginInfo != null)
        {
            GPGS_GetLoginInfo();
        }
    }
    public void ClickGPGSLogin()
    {
        if(GPGS_LoginClick != null)
        {
#if UNITY_EDITOR
            ShowUIView_NickName();

#elif UNITY_ANDROID
            GPGS_LoginClick();
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

#if UNITY_EDITOR
            ShowUIView_NickName();

#elif UNITY_ANDROID
            Facebook_LoginClick();
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

    public void ClickCustomLogin(string _id, string _pw)
    {
        if(Custom_LoginClick != null)
        {
            Custom_LoginClick(_id, _pw);
        }
    }

    public void ClickGetCustomInfo()
    {
        if (Custom_GetLoginInfo != null)
        {
            Custom_GetLoginInfo();
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

    public void ShowUIView_NickName()
    {
        uiView_NickName.Show();
    }

    public void ShowUIView_FingerPrint()
    {

#if UNITY_EDITOR
        uiView_NickName.Hide();
        uiView_FingerPrint.Show();

        BackendReturnObject bro = Backend.BMember.CreateNickname(inputField_Nick.text);
        // 이후 처리
        string statusCode = bro.GetStatusCode();

        if (bro.IsSuccess())
        {
            uiView_NickName.Hide();
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

        inputField_Nick.text = "";

#elif UNITY_ANDROID

        BackendReturnObject bro =  Backend.BMember.CreateNickname(inputField_Nick.text);
     

        // 이후 처리
        string statusCode = bro.GetStatusCode();

        print(statusCode);

        if (bro.IsSuccess())
        {
            uiView_NickName.Hide();
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

        inputField_Nick.text = "";

#endif

    }


    #region Login


    #region CustomLogin
    public void ShowCustomLogin()
    {
        uiView_CustomLogin.Show();
    }

    public void HideCustomLogin()
    {
        uiView_CustomLogin.Hide();
    }

    public void ConfirmCustomLogin()
    {

    }

    #endregion

    #endregion


}
