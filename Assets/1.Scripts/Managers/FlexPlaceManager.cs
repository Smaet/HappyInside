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

    [Header("Canvas")]
    [SerializeField]
    private CanvasGroup flexPlaceCanvasGroup;
    [SerializeField]
    private Canvas canvas;

    [Header("FlexPlaces")]
    public BaseFlexPlace [] FlexPlaces;
    [Header("FlexPlaceGames")]
    public BaseFlexPlaceGame[] FlexPlaceGames;

   


    public void OpenFlexPlaceUI(HomeMenuButtonIndex _index)
    {
        Debug.Log("플렉스 버튼 "+_index.ToString() + " 호출!!");

        switch (_index)
        {
            case HomeMenuButtonIndex.FLEX_01:
              
                FlexPlaces[(int)FlexPlaceIndex.DEPARTMENTSTORE].OpenFlexPlace();
                curFlexPlace = FlexPlaces[(int)FlexPlaceIndex.DEPARTMENTSTORE];
                break;
            case HomeMenuButtonIndex.FLEX_02:
              
                FlexPlaces[(int)FlexPlaceIndex.DEPARTMENTSTORE].OpenFlexPlace();
                curFlexPlace = FlexPlaces[(int)FlexPlaceIndex.DEPARTMENTSTORE];
                break;
            case HomeMenuButtonIndex.FLEX_03:
              
                FlexPlaces[(int)FlexPlaceIndex.DEPARTMENTSTORE].OpenFlexPlace();
                curFlexPlace = FlexPlaces[(int)FlexPlaceIndex.DEPARTMENTSTORE];
                break;

        }
    }



    public void StartDepartmentMiniGame()
    {
        //HomeManager.Instance.PushUIView(FlexPlaces[(int)FlexPlaceIndex.DEPARTMENTSTORE]);
        FlexPlaceGames[(int)FlexPlaceIndex.DEPARTMENTSTORE].OpenFlexPlaceGame();
        //curFlexPlace = FlexPlaces[(int)FlexPlaceIndex.DEPARTMENTSTORE];
    }




    public void CloseFlexPlaceUI(FlexPlaceIndex _index)
    {
        Debug.Log("플렉스 버튼 "+_index.ToString() + " 닫기!!");


        switch(_index)
        {
            case FlexPlaceIndex.DEPARTMENTSTORE:
                //해당되는 플렉스 생활 시작
                // DepartmentStore ds =  FlexPlaces[(int)_index] as DepartmentStore;
                // ds.StartFlexPlace();

                FlexPlaces[(int)_index].CloseFlexPlace();
                break;
        }
    }

    public void OpenFlexPlaceGameUI(FlexPlaceIndex _index)
    {
        Debug.Log("플렉스 게임 버튼 "+_index.ToString() + " 호출!!");

   

        switch(_index)
        {
            case FlexPlaceIndex.DEPARTMENTSTORE:
                //해당되는 플렉스 생활 시작
                // DepartmentStore ds =  FlexPlaces[(int)_index] as DepartmentStore;
                // ds.StartFlexPlace();

                FlexPlaceGames[(int)_index].OpenFlexPlaceGame();
                curFlexPlaceGame = FlexPlaceGames[(int)_index];
                break;
        }
    }

    

    public void CloseFlexPlaceGameUI(FlexPlaceIndex _index)
    {
        Debug.Log("플렉스 게임 버튼 "+_index.ToString() + " 닫기!!");

        switch(_index)
        {
            case FlexPlaceIndex.DEPARTMENTSTORE:
                //해당되는 플렉스 생활 시작
                // DepartmentStore ds =  FlexPlaces[(int)_index] as DepartmentStore;
                // ds.StartFlexPlace();

                FlexPlaceGames[(int)_index].CloseFlexPlaceGame();
                break;
        }
    }

    IEnumerator OpenFlexPlace()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public void TestButton()
    {
        Debug.Log("플렉스 테스트 버튼!!");
    }
}
