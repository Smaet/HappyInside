using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColleguePanel : MonoBehaviour
{
    public CollegueIndex curIndex;


    public Image collegue_Image;
    public Image collegueItem_Image;


    public void SetUI(collegueInfo _info)
    {
        if (_info.Level < 10)
        {
            Sprite sprite = Resources.Load<Sprite>("Agit/Hacker/hacker_lv01");
            collegueItem_Image.sprite = sprite;
        }
        else if (_info.Level < 20)
        {
            Sprite sprite = Resources.Load<Sprite>("Agit/Hacker/hacker_lv02");
            collegueItem_Image.sprite = sprite;
        }
        else if (_info.Level < 30)
        {
            Sprite sprite = Resources.Load<Sprite>("Agit/Hacker/hacker_lv03");
            collegueItem_Image.sprite = sprite;
        }
    }
}
