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

    public void Init(Canvas _canvas)
    {
        canvas = _canvas;

        if(flexPlaceCanvasGroup == null)
        {
            flexPlaceCanvasGroup =  HomeManager.Instance.CanvasParent.transform.GetChild(2).GetComponent<CanvasGroup>();
        }

        GameObject PlaceParent = canvas.transform.GetChild(1).gameObject;
        GameObject GameParent = canvas.transform.GetChild(2).gameObject;

        

        if(FlexPlaces == null || FlexPlaces.Length == 0)
        {
            FlexPlaces = new BaseFlexPlace[1];

            for(int i=0; i < 1; i++)
            {
                FlexPlaces[i] = PlaceParent.transform.GetChild(i).GetComponent<BaseFlexPlace>();
                FlexPlaces[i].gameObject.SetActive(false);
            }
        }

        if(FlexPlaceGames == null || FlexPlaceGames.Length == 0)
        {
            FlexPlaceGames = new BaseFlexPlaceGame[1];

            for(int i=0; i < 1; i++)
            {
                FlexPlaceGames[i] = GameParent.transform.GetChild(i).GetComponent<BaseFlexPlaceGame>();
                FlexPlaceGames[i].gameObject.SetActive(false); 
            }
        }


      
    }


    public void OpenFlexPlaceUI(FlexPlaceIndex _index)
    {
        Debug.Log("플렉스 버튼 "+_index.ToString() + " 호출!!");

        //Alpha 1
        flexPlaceCanvasGroup.alpha = 1;

        //Sorting Order 레이어 순서 높히기
        canvas.sortingOrder = 500;

        HomeManager.Instance.PushUIView(FlexPlaces[(int)_index]);

        switch(_index)
        {
            case FlexPlaceIndex.DEPARTMENTSTORE:
                //해당되는 플렉스 생활 시작
                // DepartmentStore ds =  FlexPlaces[(int)_index] as DepartmentStore;
                // ds.StartFlexPlace();

                FlexPlaces[(int)_index].OpenFlexPlace();
                curFlexPlace = FlexPlaces[(int)_index];
                break;
        }
    }

    public void CloseFlexPlaceUI(FlexPlaceIndex _index)
    {
        Debug.Log("플렉스 버튼 "+_index.ToString() + " 닫기!!");

        //Alpha 1
        flexPlaceCanvasGroup.alpha = 0;

        //Sorting Order 레이어 순서 높히기
        canvas.sortingOrder = 0;

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

        //Alpha 1
        flexPlaceCanvasGroup.alpha = 1;

        //Sorting Order 레이어 순서 높히기
        canvas.sortingOrder = 500;

        HomeManager.Instance.PushUIView(FlexPlaces[(int)_index]);

        switch(_index)
        {
            case FlexPlaceIndex.DEPARTMENTSTORE:
                //해당되는 플렉스 생활 시작
                // DepartmentStore ds =  FlexPlaces[(int)_index] as DepartmentStore;
                // ds.StartFlexPlace();

                FlexPlaceGames[(int)_index].OpenFlexPlaceGame();
                curFlexPlace = FlexPlaces[(int)_index];
                break;
        }
    }

    IEnumerator OpenFlexPlace()
    {
        yield return new WaitForSeconds(0.5f);
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



    public void TestButton()
    {
        Debug.Log("플렉스 테스트 버튼!!");
    }
}
