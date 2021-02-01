
/*
  2021.01.05 Created 
  FlexPlaceManager.cs   
  
  
  기능
  

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void StartDepartmentMiniGame()
    {
        FlexPlaceGames[(int)FlexPlaceIndex.DEPARTMENTSTORE].OpenFlexPlaceGame();
    }

    
}
