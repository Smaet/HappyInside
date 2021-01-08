using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticMyFader : SimpleSingleton<StaticMyFader>
{
   
    protected override void Awake()
    {
        base.Awake();
    }

    public void StartFader(FaderState _state, Image _panel,  float _fTime, float _maxValue = 1)
    {

        if (_state == FaderState.FADEOUT)
        {
            Color c;
            c = _panel.color;
            c.a = 1;
            _panel.color = c;
        }
        else
        {
            Color c;
            c = _panel.color;
            c.a = 0;
            _panel.color = c;
        }

        StartCoroutine(FaderUpdate(_state, _panel, _fTime, _maxValue));
    }

    IEnumerator FaderUpdate(FaderState _state, Image _panel, float _fTime, float _maxValue = 1)
    {
        Color c;

        float curTime = 0;

        while (true)
        {
            if (_state == FaderState.NONE)
            {
                // if (this.gameObject.activeSelf) 
                //     this.gameObject.SetActive(false);

                yield break;
            }


            switch (_state)
            {
                case FaderState.NONE:
                    break;

                case FaderState.FADEIN:     // 검은색이 사라지고 서서히 컨텐치 내용이 나타난다.
                    //Debug.Log("FadeIn....");
                    c = _panel.color;
                    c.a = Mathf.Lerp(_maxValue, 0f, curTime / _fTime);
                    _panel.color = c;

                    //			Debug.Log ("curTIme="+curTime+", fadeTime=" + fadeTime);
                    if (c.a <= 0f)
                    {
                        _state = FaderState.NONE;
                        _panel.raycastTarget = false;
                    }
                    break;

                case FaderState.FADEOUT:    // 화면이 점점 검게 변한다.
                    //Debug.Log("FadeOut....");
                    c = _panel.color;
                    c.a = Mathf.Lerp(0f, _maxValue, curTime / _fTime);
                    _panel.color = c;

                    if (c.a >= 1f)
                    {
                        _state = FaderState.NONE;
                        //UIFader.Instance.FadeIn(0.3f);
                    }
                    break;
            }
            curTime += Time.deltaTime;
            yield return null;
        }

    }

    public void StartFader(FaderState _state, CanvasGroup _cg, float _fTime, float _maxValue = 1)
    {

        if (_state == FaderState.FADEOUT)
        {
            _cg.alpha = 1;
        }
        else
        {
            _cg.alpha = 0;
        }

        StartCoroutine(FaderUpdate(_state, _cg, _fTime, _maxValue));
    }

    IEnumerator FaderUpdate(FaderState _state, CanvasGroup _cg, float _fTime, float _maxValue = 1)
    {
        Color c;

        float curTime = 0;

        while (true)
        {
            if (_state == FaderState.NONE)
            {
                // if (this.gameObject.activeSelf) 
                //     this.gameObject.SetActive(false);
                if (_cg.alpha <= 0)
                {
                    _cg.gameObject.SetActive(false);
                }
               
                    
                yield break;
            }


            switch (_state)
            {
                case FaderState.NONE:
                    break;

                case FaderState.FADEIN:     // 검은색이 사라지고 서서히 컨텐치 내용이 나타난다.
                    //Debug.Log("FadeIn....");
                    
                    _cg.alpha = Mathf.Lerp(0f, _maxValue, curTime / _fTime);
    

                    //			Debug.Log ("curTIme="+curTime+", fadeTime=" + fadeTime);
                    if (_cg.alpha >= 1f)
                    {
                        _state = FaderState.NONE;
                        //_panel.raycastTarget = false;
                    }
                    break;

                case FaderState.FADEOUT:    // 화면이 점점 검게 변한다.
                    //Debug.Log("FadeOut....");
                    //c = _panel.color;
                    _cg.alpha = Mathf.Lerp(_maxValue, 0f , curTime / _fTime);
                    //_panel.color = c;

                    if (_cg.alpha <= 0f)
                    {
                        _state = FaderState.NONE;
                        //UIFader.Instance.FadeIn(0.3f);
                    }
                    break;
            }
            curTime += Time.deltaTime;
            yield return null;
        }

    }
}
