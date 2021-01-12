using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FaderState
{
    NONE = 0,
    FADEIN,
    FADEOUT,
}
public class MyFader : SimpleSingleton<MyFader>
{
    public Image panel;
    public CanvasGroup anotherCG;
    public CanvasGroup overlayUICG;

    float curTime = 0f;
    private float maxValue;
    private float fadeTime;
    private FaderState curFadeState;

  
    protected override void Awake()
    {
        base.Awake();
        this.GetComponent<GraphicRaycaster>().enabled = false;
    }

     public void StartAnotherCanvasFader(FaderState _state, float _fTime, float _maxValue = 1)
    {
        //if (this.gameObject.activeSelf == false)
        //    this.gameObject.SetActive(true);

        this.GetComponent<GraphicRaycaster>().enabled = true;
        curFadeState = _state;
        maxValue = _maxValue;
        fadeTime = _fTime;
        curTime = 0f;

        if(_state == FaderState.FADEOUT)
        {
             anotherCG.alpha = 0;
        }
        else
        {
             anotherCG.alpha = 1;
        }

        StartCoroutine(AnotherCanvasFaderUpdate());
    }
   
    IEnumerator AnotherCanvasFaderUpdate()
    {
     
        float alpha;
        while(true)
        {
            if (curFadeState == FaderState.NONE)
            {
               // if (this.gameObject.activeSelf) 
               //     this.gameObject.SetActive(false);

                yield break;
            }
          

            switch (curFadeState)
            {
                case FaderState.NONE:
                    break;

                case FaderState.FADEIN:     // 검은색이 사라지고 서서히 컨텐치 내용이 나타난다.
                   
                    alpha = Mathf.Lerp(maxValue, 0f, curTime / fadeTime);
                    anotherCG.alpha = alpha;
                    if (anotherCG.alpha <= 0f)
                    {
                        this.GetComponent<GraphicRaycaster>().enabled = false;
                        curFadeState = FaderState.NONE;
                    }
                    break;

                case FaderState.FADEOUT:    // 화면이 점점 검게 변한다.
 
                    alpha= Mathf.Lerp(0f, maxValue, curTime / fadeTime);
                    anotherCG.alpha = alpha;

                    if (anotherCG.alpha >= 1f)
                    {
                        this.GetComponent<GraphicRaycaster>().enabled = false;
                        curFadeState = FaderState.NONE;
                    }
                    break;
            }
            curTime += Time.deltaTime;
            yield return null;
        }
   
    }


    public void StartFader(FaderState _state, float _fTime, float _maxValue = 1)
    {
        //if (this.gameObject.activeSelf == false)
        //    this.gameObject.SetActive(true);

        curFadeState = _state;
        maxValue = _maxValue;
        fadeTime = _fTime;
        curTime = 0f;
        this.GetComponent<GraphicRaycaster>().enabled = true;
        
        if(_state == FaderState.FADEOUT)
        {
            Color c;
            c = panel.color;
            c.a = 0;
            panel.color = c;
        }
        else
        {
            Color c;
            c = panel.color;
            c.a = 1;
            panel.color = c;
        }

        StartCoroutine(FaderUpdate());
    }
   
    IEnumerator FaderUpdate()
    {
        Color c;
        while(true)
        {
            if (curFadeState == FaderState.NONE)
            {
               // if (this.gameObject.activeSelf) 
               //     this.gameObject.SetActive(false);

                yield break;
            }
          

            switch (curFadeState)
            {
                case FaderState.NONE:
                    break;

                case FaderState.FADEIN:     // 검은색이 사라지고 서서히 컨텐치 내용이 나타난다.
                    //Debug.Log("FadeIn....");
                    c = panel.color;
                    c.a = Mathf.Lerp(maxValue, 0f, curTime / fadeTime);
                    panel.color = c;

                    //			Debug.Log ("curTIme="+curTime+", fadeTime=" + fadeTime);
                    if (c.a <= 0f)
                    {
                        curFadeState = FaderState.NONE;
                        panel.raycastTarget = false;
                    }
                    break;

                case FaderState.FADEOUT:    // 화면이 점점 검게 변한다.
                    //Debug.Log("FadeOut....");
                    c = panel.color;
                    c.a = Mathf.Lerp(0f, maxValue, curTime / fadeTime);
                    panel.color = c;

                    if (c.a >= 1f)
                    {
                        curFadeState = FaderState.NONE;
                        //UIFader.Instance.FadeIn(0.3f);
                    }
                    break;
            }
            curTime += Time.deltaTime;
            yield return null;
        }
   
    }

    public void StartFaderWithUI(string _mode ,FaderState _state, float _fTime, float _maxValue = 1)
    {
        //if (panel.gameObject.activeSelf == false)
         //   panel.gameObject.SetActive(true);

        if(this.gameObject.name == _mode)
        {
            curFadeState = _state;
            maxValue = _maxValue;
            fadeTime = _fTime;
            curTime = 0f;
            overlayUICG.alpha = 0f;
            StartCoroutine(FaderUpdateWithUI());
        }

       
    }

    IEnumerator FaderUpdateWithUI()
    {
     

        Color c;
        CanvasGroup cg;

        while (true)
        {
            if (curFadeState == FaderState.NONE)
            {
                //if (panel.gameObject.activeSelf)
                 //   panel.gameObject.SetActive(false);

                yield break;
            }


            switch (curFadeState)
            {
                case FaderState.NONE:
                    break;

                case FaderState.FADEIN:     // 검은색이 사라지고 서서히 컨텐치 내용이 나타난다.
                    //Debug.Log("FadeIn....");
                    c = panel.color;
                    c.a = Mathf.Lerp(maxValue, 0f, curTime / fadeTime);
                    panel.color = c;

                    cg = overlayUICG;
                    cg.alpha = Mathf.Lerp(0f, maxValue, curTime / fadeTime);
                    overlayUICG.alpha = cg.alpha;

                    //			Debug.Log ("curTIme="+curTime+", fadeTime=" + fadeTime);
                    if (c.a <= 0f)
                    {
                        curFadeState = FaderState.NONE;
                        panel.raycastTarget = false;
                    }
                    break;

                case FaderState.FADEOUT:    // 화면이 점점 검게 변한다.
                   // Debug.Log("FadeOut....");
                    c = panel.color;
                    c.a = Mathf.Lerp(0f, maxValue, curTime / fadeTime);
                    panel.color = c;

                    //cg = overlayUICG;
                    //cg.alpha = Mathf.Lerp(maxValue, 0f, curTime / fadeTime);
                    //overlayUICG.alpha = cg.alpha;

                    if (c.a >= 1f)
                    {
                        curFadeState = FaderState.NONE;
                        //UIFader.Instance.FadeIn(0.3f);
                    }
                    break;
            }
            curTime += Time.deltaTime;
            yield return null;
        }

    }



}
