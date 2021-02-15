using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ColleguePanel : MonoBehaviour
{
    public CollegueIndex curIndex;

    public Image image_Collegue;
    public Image image_collegueItem;

    public Button button_Collegeue;
    public Button button_CollegeueItem;

    public GameObject sampleEffect_location;
    public SimpleObjectPool simpleObjectPool;

    public Action OnClickCollegueButton;

    public void OnEnable()
    {
        button_Collegeue.onClick.AddListener(() =>
                 HomeManager.Instance.agitManager.ClickCollegueButton(curIndex));

        button_CollegeueItem.onClick.AddListener(() =>
                HomeManager.Instance.agitManager.ClickCollegueItemButton(curIndex));
    }

    public void OnDisable()
    {
        button_Collegeue.onClick.RemoveAllListeners();
        button_CollegeueItem.onClick.RemoveAllListeners();
    }



    public void SetUI(CollegueInfo _info)
    {
        if (_info.collegueIndex == CollegueIndex.Dare)
        {
            if (_info.Level < 10)
            {
                Sprite sprite = Resources.Load<Sprite>("Agit/Transparency");
                image_collegueItem.sprite = sprite;
            }
            else if (_info.Level < 19)
            {
                Sprite sprite = Resources.Load<Sprite>("Agit/Hacker/agitA_h_lv02");
                image_collegueItem.sprite = sprite;
            }
            else if (_info.Level < 29)
            {
                Sprite sprite = Resources.Load<Sprite>("Agit/Hacker/agitA_h_lv03");
                image_collegueItem.sprite = sprite;
            }
            else if (_info.Level <= 30)
            {
                Sprite sprite = Resources.Load<Sprite>("Agit/Hacker/agitA_h_lv04");
                image_collegueItem.sprite = sprite;
            }
        }
    }

    public void StartSampleEffect()
    {
        GameObject moneyObject = simpleObjectPool.GetObject();
        BaseCombo combo = moneyObject.GetComponent<BaseCombo>();

        combo.SetInfo("+" + GameManager.Instance.user.userBaseProperties.collegueInfos[0].collegueBasicSkill.money, simpleObjectPool.transform, sampleEffect_location.transform);

        moneyObject.SetActive(true);

        //PoloSFX.Instance.Play_HackerUp();   
    }
}
