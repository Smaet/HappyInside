
/*
  2021.01.05 Created 
  FlexPlaceManager.cs   
  
  
  기능
  1. FlexPlace 와 FlexPlaceGame에 접근 할수 있는 기능
  2. FlexPlace에서 발생하는 이벤트 관리

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum FlexPlaceIndex
{
    DEPARTMENTSTORE = 0,
}

public class FlexPlaceManager : MonoBehaviour
{
    [Header("Status")]
    [SerializeField]
    private BaseFlexPlace curFlexPlace;
    [SerializeField]
    private BaseFlexPlaceGame curFlexPlaceGame;

    [Header("FlexPlaces")]
    public BaseFlexPlace [] FlexPlaces;
    [Header("FlexPlaceGames")]
    public BaseFlexPlaceGame[] FlexPlaceGames;


    //각종 이벤트 및 초기화 셋팅
    public void Init()
    {
        DepartmentInit();
    }

    #region 백화점
  

    private void DepartmentInit()
    {
       
     
    }

   

    #endregion


}
