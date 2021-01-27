using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColleguePanel : MonoBehaviour
{
    public CollegueIndex curIndex;


    public Image collegue_Image;
    public Image collegueItem_Image;

    public GameObject sampleEffect_location;
    public SimpleObjectPool simpleObjectPool;

    public void SetUI(collegueInfo _info)
    {
        if (_info.collegueIndex == CollegueIndex.HACKER)
        {
            if (_info.Level < 10)
            {
                Sprite sprite = Resources.Load<Sprite>("Agit/Transparency");
                collegueItem_Image.sprite = sprite;
            }
            else if (_info.Level < 19)
            {
                Sprite sprite = Resources.Load<Sprite>("Agit/Hacker/agitA_h_lv02");
                collegueItem_Image.sprite = sprite;
            }
            else if (_info.Level < 29)
            {
                Sprite sprite = Resources.Load<Sprite>("Agit/Hacker/agitA_h_lv03");
                collegueItem_Image.sprite = sprite;
            }
            else if (_info.Level < 30)
            {
                Sprite sprite = Resources.Load<Sprite>("Agit/Hacker/agitA_h_lv04");
                collegueItem_Image.sprite = sprite;
            }
            StartSampleEffect();
        }





    }

    public void StartSampleEffect()
    {
        GameObject moneyObject = simpleObjectPool.GetObject();
        BaseCombo combo = moneyObject.GetComponent<BaseCombo>();

        combo.SetInfo("+" + GameManager.Instance.user.userBaseProperties.collegueInfos[0]., simpleObjectPool.transform, sampleEffect_location.transform);

        moneyObject.SetActive(true);
    }
}
