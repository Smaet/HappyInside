using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CollegueView : MonoBehaviour
{
    public CollegueIndex curCollegue;
    [SerializeField]
    protected Image collegueImage;
    [SerializeField]
    protected TextMeshProUGUI name_TMP;
    [SerializeField]
    protected TextMeshProUGUI level_TMP;
    [SerializeField]
    protected TextMeshProUGUI SkillContext_TMP;
    [SerializeField]
    protected TextMeshProUGUI manipulateMoney_TMP;
    [SerializeField]
    protected TextMeshProUGUI collegueProfile_TMP;
    [SerializeField]
    protected TextMeshProUGUI levelUpCost_TMP;
    [SerializeField]
    protected Image[] passiveSkills;
    [SerializeField]
    protected Sprite[] abilityDisableSprites;
    [SerializeField]
    protected Sprite[] abilityEnableSprites;
    [SerializeField]
    protected Button levelUp_button;
    [SerializeField]
    protected Button levelUpItem_button;


    public virtual void Init()
    {
        
    }

    public virtual void OpenCollegueView(CollegueIndex _index)
    {
      
    }

    protected virtual void SetSkillContext(CollegueBasicSkill _skill)
    {

    }

    public void SetName(string _name)
    {
        name_TMP.text = _name;
    }
    public void SetLevel(int _Level)
    {
        level_TMP.text = string.Format("Lv. {0}", _Level) ;
    }

  

    protected void Init_SetPassiveSkill()
    {
        CollegueInfo info = GameManager.Instance.user.userBaseProperties.collegueInfos[(int)curCollegue];
        if (info.Level < 30)
        {
            //스킬레벨업에 따른 패시브 스킬 확인
            if (info.Level >= 10)
            {
                //패시브 스킬 창 갱신
                SetPassiveSkill(true, 0);

            }
            if (info.Level >= 20)
            {
                //패시브 스킬 창 갱신
                SetPassiveSkill(true, 1);
            }
            if (info.Level >= 30)
            {
                //패시브 스킬 창 갱신
                SetPassiveSkill(true, 2);
            }
        }

       
    }

    protected void SetPassiveSkill(bool _isActive, int _index)
    {

        if (_isActive)
        {
            passiveSkills[_index].sprite = abilityEnableSprites[_index];
        }
        else
        {
            passiveSkills[_index].sprite = abilityDisableSprites[_index];
        }

    }

    protected void SetLevelCost()
    {
        CollegueInfo info = GameManager.Instance.user.userBaseProperties.collegueInfos[(int)curCollegue];
        levelUpCost_TMP.text = "x" +info.Level;

        if (GameManager.Instance.user.userBaseProperties.blackChip >= info.Level)
        {
            levelUp_button.interactable = true;
            levelUpItem_button.interactable = true;

            if (info.Level >= 30)
            {
                levelUp_button.interactable = false;
                levelUpItem_button.interactable = false;
            }
        }
        else
        {
            levelUp_button.interactable = false;
            levelUpItem_button.interactable = false; 
        }
    }

    protected virtual void OnButtonLevelUpClick()
    {
       
    }

    protected virtual void OnButtonItemLevelUpClick()
    {

    }

    public void SetManipulateMoney()
    {
        //manipulateMoney_TMP.text = string.Format("{0:#,0}", GameManager.Instance.user.userBaseProperties.manipulatedMoney);
    }
}
