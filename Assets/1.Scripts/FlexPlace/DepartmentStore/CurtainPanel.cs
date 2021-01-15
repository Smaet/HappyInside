using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CurtainState
{
    NONE = -1,
    CLOSE = 0,
    OPEN,
}



public class CurtainPanel : MonoBehaviour
{
    private DepartmentStoreGame dGame;
    [SerializeField]
    CurtainState curState = CurtainState.NONE;

    private bool isCurtainMove = false;
    private bool isCurtainOpen = false;
    [SerializeField]
    private RectTransform curtainRight;
    [SerializeField]
    private RectTransform curtainLeft;

    [SerializeField]
    private float closingTime = 0;      //닫히때 걸리는 시간
    [SerializeField]
    private float openingTime = 0;      //열릴때 걸리는 시간.
    [SerializeField]
    private float closingStandByTime = 0;   //닫히고 나서 기다리는 시간
    [SerializeField]
    private float openingStandByTime = 0;   //열리고 나서 기다리는 시간

    public void StartGameCurtain()
    {
        dGame.isGamePlaying = true;
        StartCoroutine(GameCurtain());
    }

    IEnumerator GameCurtain()
    {
        while(true)
        {
            if(dGame.isGamePlaying == false)
            {
                dGame.EndGame();
                yield break;
            }

            if (isCurtainOpen == false && isCurtainMove == false)
            {
                //닫히고 열리기 까지의 텀
                yield return new WaitForSeconds(closingStandByTime);

                StartCoroutine(NextShoppingItem());
            }

            yield return null;
        }
    }

    IEnumerator NextShoppingItem()
    {
        OpenCurtain();
        while(true)
        {
            if (isCurtainOpen == true && isCurtainMove == false)
            {
                if (openingStandByTime == 0)
                {
                    yield return null;
                }
                else
                {
                    yield return new WaitForSecondsRealtime(openingStandByTime);
                }
         

                CloseCurtain();
                break;
            }

            yield return null;
        }
    }



    public void Init(DepartmentStoreGame _game)
    {
        dGame = _game;
        if (curtainRight == null)
        {
            curtainRight = transform.GetChild(0).GetComponent<RectTransform>();
        }

        if(curtainLeft == null)
        {
            curtainLeft = transform.GetChild(1).GetComponent<RectTransform>();
        }

        isCurtainOpen = false;
        isCurtainMove = false;
    }

    public void CloseCurtain()
    {
        if(isCurtainMove == false)
        {
            curState = CurtainState.CLOSE;
            
            isCurtainMove = true;
            StartCoroutine(CurtainMove());
        }
     
    }


    public void OpenCurtain()
    {
        if (isCurtainMove == false)
        { 
            //카드리더기 가능하게 설정
            dGame.SetCardReader(true);
            //카드리더기 가능하게 설정
            dGame.SetFreezeButton(true);
            //시간이 멈춰있으면 다시 시간이 가게함
            dGame.departmentGameTimer.SetPause(false);
            //
            curState = CurtainState.OPEN;
            isCurtainMove = true;
            StartCoroutine(CurtainMove());
        }
    }


    IEnumerator CurtainMove()
    {
        float time = 0;

        float rightCurtainPosX = 0;
        float leftCurtainPosX = 0;
        float destRightCurtainPosX = 0;
        float destLeftCurtainPosX = 0;

        switch (curState)
        {
            case CurtainState.CLOSE:

                rightCurtainPosX      = curtainRight.anchoredPosition.x;
                leftCurtainPosX       = curtainLeft.anchoredPosition.x;

                destRightCurtainPosX  = 0;
                destLeftCurtainPosX   = 0;

                break;

            case CurtainState.OPEN:

                rightCurtainPosX = curtainRight.anchoredPosition.x;
                leftCurtainPosX = curtainLeft.anchoredPosition.x;

                destRightCurtainPosX = curtainRight.rect.width;
                destLeftCurtainPosX = -curtainLeft.rect.width;

                break;
        }

        
        while (true)
        {
            if(curState == CurtainState.CLOSE)
            {
                if (time >= closingTime + 0.1f)
                {
                    CloseCurtainComplte();
                    yield break;
                }
            }
            else if(curState == CurtainState.OPEN)
            {
                if (time >= openingTime)
                {
                    OpenCurtainComplete();
                    yield break;
                }
            }

            float newRightPosX = Mathf.Lerp(rightCurtainPosX, destRightCurtainPosX, time / closingTime);

            float newLeftPosX = Mathf.Lerp(leftCurtainPosX, destLeftCurtainPosX, time / closingTime);

            curtainRight.anchoredPosition = new Vector2(newRightPosX, curtainRight.anchoredPosition.y);
            curtainLeft.anchoredPosition = new Vector2(newLeftPosX, curtainLeft.anchoredPosition.y);

            time += Time.deltaTime;

            yield return null;
        }
    }

    public void OpenCurtainComplete()
    {
        //Debug.Log("커튼 열림!!");
        isCurtainOpen = true;
        isCurtainMove = false;
    }

    public void CloseCurtainComplte()
    {
        //Debug.Log("커튼 닫힘!!");
        isCurtainOpen = false;
        isCurtainMove = false;
        //카드리더기 가능하게 설정
        dGame.SetCardReader(false);
        //일시정기 기능 해제
        dGame.SetFreezeButton(false);
        //진품 가품 랜덤하게 받아야함
        dGame.SetShoppingItem();

        //닫혔을 때 시간이 끝나있다면
        if (dGame.departmentGameTimer.TimerOn() == false)
        {
            dGame.SetEndGame();
        }
    }

}


