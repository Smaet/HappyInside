/*
  2021.01.05 Created 
  TitleManager.cs   
  기능
  1. 로그인
  2. 타이틀 관련 UI
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Touch;
using Doozy.Engine.UI;
using TMPro;

public class TitleManager : MonoBehaviour
{
    public TMP_InputField inputField_Nick;

    public Slider slider_FingerPrintProgress;

    public Button button_FaceBook;
    public Button button_Google;


    public UIView uiView_Login;
    public Button button_LoginConfirm;

    public UIView uiView_FingerPrint;

    bool isLoadScene = false;


    private void Start()
    {
        isLoadScene = false;
        button_FaceBook.onClick.RemoveAllListeners();
        button_Google.onClick.RemoveAllListeners();
        button_LoginConfirm.onClick.RemoveAllListeners();

        button_FaceBook.onClick.AddListener(ShowUIView_Login);
        button_Google.onClick.AddListener(ShowUIView_Login);
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
        
    }

    public void LoadScene()
    {
        Debug.Log("Load GameScene!!");
        MySceneLoader.Instance.LoadScene("1.GameScene_DoozyUI");
    }

    public void PressFingerUpdate(LeanFinger _finger)
    {
        print("Press Finger!!");

        if (slider_FingerPrintProgress.value < 1f)
        {
            slider_FingerPrintProgress.value += Time.deltaTime / 3f;
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
        print("DeSelected!!");
        if(slider_FingerPrintProgress.value < 1f)
        {
            slider_FingerPrintProgress.value = 0;
        }
    }

    void ShowUIView_Login()
    {
        uiView_Login.Show();
    }

    void ShowUIView_FingerPrint()
    {
        uiView_Login.Hide();
        uiView_FingerPrint.Show();
        GameManager.Instance.SetUserInfo(inputField_Nick.text);
        inputField_Nick.text = "";
    }
}
