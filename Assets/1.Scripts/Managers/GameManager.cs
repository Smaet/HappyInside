/*
  2021.01.05 Created 
  GameManager.cs   
  규칙
  1.변수는 소문자로 시작해서 구분하기 위한 부분에 대문자를 넣어서 사용
  2.함수의 매개 변수는 '_'을 변수 앞에 붙이기 -> Example(int _index) {}
  
  기능
  1. 유저 정보 관리
  2. 서버와 통신
  3. 사운드

*/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class GameManager : SimpleSingleton<GameManager>
{


    [Header("User"), SerializeField] private User user;



    // Score
    [SerializeField] private float DepthScore;
    public float DepthMultiple;

    public enum GameState
    {
        Ready,
        Playing,
        NotPlaying
    }

    public GameState CurrentGameState;


    protected override void Awake()
    {
        base.Awake();

        Init();
    }




    void Init()
    {
        //유저 정보 초기화


    }
    #region GameControl

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion



}
