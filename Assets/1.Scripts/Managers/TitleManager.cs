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

public class TitleManager : MonoBehaviour
{

    public void LoadScene()
    {
        Debug.Log("Load GameScene!!");
        SceneLoader.Instance.LoadScene("1.GameScene");
    }
}
