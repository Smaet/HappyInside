using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public static GUIManager instance;

    public GameObject MainMenu;
    public GameObject GameMenu;
    public GameObject ResultMenuWin;
    public GameObject ResultMenuLose;



    private Text DepthScoreText;


    public Text DebugText;


    private void Awake()
    {
        instance = this;
        DepthScoreText = GameMenu.GetComponentInChildren<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // DebugText
    public void SetDebugMessage(string txt)
    {
        DebugText.text = txt;
    }


    public void SetDepthScoreText(string txt)
    {
        DepthScoreText.text = (int)(float.Parse(txt))  + "M";
    }


    #region GUI Control
    private void AllGUIOff()
    {
        MainMenu.SetActive(false);
        GameMenu.SetActive(false);
        ResultMenuWin.SetActive(false);
        ResultMenuLose.SetActive(false);

        SetDepthScoreText("0");
    }

    public void MainMenuOn()
    {
        AllGUIOff();
        MainMenu.SetActive(true);
    }

    public void GameMenuOn()
    {
        AllGUIOff();
        GameMenu.SetActive(true);
    }

    public void ResultMenuOn(bool win)
    {
        AllGUIOff();

        if (win) ResultMenuWin.SetActive(true);
        else ResultMenuLose.SetActive(true);
    }


    #endregion

}
