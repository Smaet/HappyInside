using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class BaseSlideButtonPanel : MonoBehaviour
{

    [SerializeField]
    public HomeSlideButton slideActiveButton;
    [SerializeField]
    public HomeSlideButton[] slideButtons;
    [SerializeField]
    private ScrollRect scrollRect;          

    private bool isButtonAnimSliding;       //슬라이드 애니메이션 중인지
    private bool isButtonOnTop;             //슬라이드가 끝까지 갔는지

    public bool isButtonTopOn()
    {
        return isButtonOnTop;
    }

    public virtual void SetButton()
    {
        slideActiveButton.GetButton().onClick.RemoveAllListeners();

        for (int i = 0; i < slideButtons.Length; i++)
        {
            slideButtons[i].GetButton().onClick.RemoveAllListeners();
        }
    }

    public void StartButtonAutoSlide()
    {
        if (isButtonAnimSliding == false)
        {
            isButtonAnimSliding = true;
            StartCoroutine(ButtonAutoSlide());
        }
    }

    IEnumerator ButtonAutoSlide()
    {
        float startPoint = 0;
        float time = 0;
        float totalTime = 0.3f;

        if (isButtonOnTop == false)
        {
            startPoint = 0;
        }
        else
        {
            startPoint = 1;
        }
        scrollRect.verticalNormalizedPosition = startPoint;
        while (true)
        {
            if (time >= totalTime)
            {
                if (isButtonOnTop == false)
                {
                    isButtonOnTop = true;
                    scrollRect.verticalNormalizedPosition = 0;
                }
                else
                {
                    isButtonOnTop = false;
                    scrollRect.verticalNormalizedPosition = 1;
                }


                isButtonAnimSliding = false;
                yield break;
            }

            if (isButtonOnTop == false)
            {
                scrollRect.verticalNormalizedPosition = Mathf.Lerp(1, startPoint, time / totalTime);
            }
            else
            {
                scrollRect.verticalNormalizedPosition = Mathf.Lerp(0, startPoint, time / totalTime);
            }
            time += Time.deltaTime;
            yield return null;
        }
    }

}
