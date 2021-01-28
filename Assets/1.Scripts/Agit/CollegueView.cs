using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollegueView : MonoBehaviour
{
    public CollegueIndex curCollegue;



    public Image collegueImage;
    public TextMeshProUGUI name_TMP;
    public TextMeshProUGUI level_TMP;
    public TextMeshProUGUI SkillContext_TMP;
    public TextMeshProUGUI manipulateMoney_TMP;

    public TextMeshProUGUI collegueProfile_TMP;
    public TextMeshProUGUI levelUpCost_TMP;
    public Image[] passiveSkills;
    public Sprite[] abilityDisableSprites;
    public Sprite[] abilityEnableSprites;

    public Button levelUp_button;

    public void OpenCollegueView(collegueInfo _info)
    {
        CollegueIndex index = _info.collegueIndex;
        curCollegue = index;

        switch (index)
        {
            case CollegueIndex.HACKER:
                SetName("X-다레");
                SetLevel(_info.Level);
                SetSkillContext(_info.collegueBasicSkill);
                SetLevelCost();
                SetManipulateMoney();
                Init_SetPassiveSkill();
                break;
            case CollegueIndex.MECHANIC:
                break;
            case CollegueIndex.CHEMIST:
                break;
            case CollegueIndex.COOK:
                break;
            case CollegueIndex.TRADER:
                break;
        }
    }

    public void SetName(string _name)
    {
        name_TMP.text = _name;
    }
    public void SetLevel(int _Level)
    {
        level_TMP.text = string.Format("Lv. {0}", _Level) ;
    }

    public void SetSkillContext(collegueBasicSkill _skill)
    {
        switch(curCollegue)
        {
            case CollegueIndex.HACKER:
                SkillContext_TMP.text = GameManager.Instance.GetMoneyFormat(_skill.money);
                break;
            case CollegueIndex.MECHANIC:
                break;
            case CollegueIndex.CHEMIST:
                break;
            case CollegueIndex.COOK:
                break;
            case CollegueIndex.TRADER:
                break;
        }
    }

    public void Init_SetPassiveSkill()
    {
        collegueInfo info = GameManager.Instance.user.userBaseProperties.collegueInfos[(int)curCollegue];
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

    public void SetPassiveSkill(bool _isActive, int _index)
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

    public void SetLevelCost()
    {
        collegueInfo info = GameManager.Instance.user.userBaseProperties.collegueInfos[(int)curCollegue];
        levelUpCost_TMP.text = "x" +info.Level;

        if (GameManager.Instance.user.userBaseProperties.pinkChip >= info.Level)
        {
            levelUp_button.interactable = true;

            if(info.Level >= 30)
            {
                levelUp_button.interactable = false;
            }
        }
        else
        {
            levelUp_button.interactable = false;
        }
    }

    public void OnButtonLevelUpClick()
    {
        collegueInfo info = GameManager.Instance.user.userBaseProperties.collegueInfos[(int)curCollegue];
        //스킬업 가능 조건 레벨보다 핑크칩의 갯수가 크거나 같으면
        if (GameManager.Instance.user.userBaseProperties.pinkChip >= info.Level)
        {
          
            if(info.Level < 30)
            {
                //핑크칩 갯수 다운
                GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.PINKCHIP, (float)-info.Level);

                //스킬 레벨 업
                info.Level++;

                //스킬 레벨 업에 따른 조작된 재산 증가량 증가
                info.collegueBasicSkill.money += 5000000;


                //스킬레벨업에 따른 패시브 스킬 확인
                if (info.Level == 10)
                {
                    info.colleguePassiveSkills[0].isActive = true;
                    //패시브 스킬 창 갱신
                    SetPassiveSkill(true, 0);
                    //아지트 마다 레벨에 따른 UI 갱신
                    if (curCollegue == CollegueIndex.HACKER)
                    {
                        HomeManager.Instance.agitManager.agit_A.colleguePanels[0].SetUI(info);
                     
                    }

                }
                else if (info.Level == 20)
                {
                    info.colleguePassiveSkills[1].isActive = true;
                    //패시브 스킬 창 갱신
                    SetPassiveSkill(true, 1);
                    //아지트 마다 레벨에 따른 UI 갱신
                    if (curCollegue == CollegueIndex.HACKER)
                    {
                        HomeManager.Instance.agitManager.agit_A.colleguePanels[0].SetUI(info);
                      
                    }
                }
                else if (info.Level == 30)
                {
                    info.colleguePassiveSkills[2].isActive = true;
                    //패시브 스킬 창 갱신
                    SetPassiveSkill(true, 2);
                    //아지트 마다 레벨에 따른 UI 갱신
                    if (curCollegue == CollegueIndex.HACKER)
                    {
                        HomeManager.Instance.agitManager.agit_A.colleguePanels[0].SetUI(info);
                    }

                    levelUp_button.interactable = false;
                }

                //유저 정보에 반영
                GameManager.Instance.user.SetUserInfo(curCollegue, info);

                Debug.Log("현재 해커의 능력으로 추가되는 돈의 증가량 : " + info.collegueBasicSkill.money);

                //UI 반영
                SetLevel(info.Level);
                SetSkillContext(info.collegueBasicSkill);
                SetLevelCost();
            }
        }
        else
        {
            levelUp_button.interactable = false;
            Debug.Log("핑크칩의 개수가 부족합니다.");
        }
       
    }



    public void SetManipulateMoney()
    {
        manipulateMoney_TMP.text = string.Format("{0:#,0}", GameManager.Instance.user.userBaseProperties.manipulatedMoney);
    }
}
