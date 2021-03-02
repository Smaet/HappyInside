using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PIVOTENUM
{
    NONE = 0,
    FIT,
}


public class PivotSetter : MonoBehaviour
{
    
    public RectTransform rectTransform;
    public PIVOTENUM curPivotEnum;

    private void OnValidate()
    {
        float PosX = rectTransform.anchoredPosition.x;
        float PosY = rectTransform.anchoredPosition.y;

        float ResX = PosX / Screen.width;
        float ResY = PosY / Screen.height;

        //둘째 자리에서 반올림
        float X = Mathf.Round(ResX * 10) * 0.1f;
        float Y = Mathf.Round(ResY * 10) * 0.1f;

        //rectTransform.an

        Debug.Log("X : " + X + " Y : " + Y);

    }
}
