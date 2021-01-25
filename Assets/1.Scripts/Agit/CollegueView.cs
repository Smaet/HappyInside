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

    public TextMeshProUGUI collegueProfile_TMP;
    public TextMeshProUGUI levelUpCost_TMP;

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
        level_TMP.text = string.Format("{0}", _Level) ;
    }

    public void SetSkillContext(collegueBasicSkill _skill)
    {
        switch(curCollegue)
        {
            case CollegueIndex.HACKER:
                SkillContext_TMP.text = GameManager.Instance.GetMoneyFormat(_skill.money) + " / " + _skill.hour +"시간";
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

    public void SetLevelCost()
    {
        collegueInfo info = GameManager.Instance.user.userBaseProperties.collegueInfos[(int)curCollegue];
        levelUpCost_TMP.text = info.Level + "개 필요";
    }

    public void OnButtonLevelUpClick()
    {
        collegueInfo info = GameManager.Instance.user.userBaseProperties.collegueInfos[(int)curCollegue];
        //스킬업 가능 조건 레벨보다 핑크칩의 갯수가 크거나 같으면
        if (GameManager.Instance.user.userBaseProperties.pinkChip >= info.Level)
        {
            //핑크칩 갯수 다운
            GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.PINKCHIP, -info.Level);

            //스킬 레벨 업
            info.Level++;

            //스킬 레벨 업에 따른 조작된 재산 증가량 증가
            info.collegueBasicSkill.money += 5000000;


            //스킬레벨업에 따른 패시브 스킬 확인
            if (info.Level == 10)
            {
                info.colleguePassiveSkills[0].isActive = true;
                //아지트 마다 레벨에 따른 UI 갱신
                if(curCollegue == CollegueIndex.HACKER)
                {
                    HomeManager.Instance.agitManager.agit_A.colleguePanels[0].SetUI(info);
                }
           
            }
            else if (info.Level == 20)
            {
                info.colleguePassiveSkills[1].isActive = true;
                //아지트 마다 레벨에 따른 UI 갱신
                if (curCollegue == CollegueIndex.HACKER)
                {
                    HomeManager.Instance.agitManager.agit_A.colleguePanels[0].SetUI(info);
                }
            }
            else if (info.Level == 30)
            {
                info.colleguePassiveSkills[2].isActive = true;
                //아지트 마다 레벨에 따른 UI 갱신
                if (curCollegue == CollegueIndex.HACKER)
                {
                    HomeManager.Instance.agitManager.agit_A.colleguePanels[0].SetUI(info);
                }
            }
  
            //유저 정보에 반영
            GameManager.Instance.user.SetUserInfo(curCollegue, info);

            Debug.Log("현재 해커의 능력으로 추가되는 돈의 증가량 : " + info.collegueBasicSkill.money);

            //UI 반영
            SetLevel(info.Level);
            SetSkillContext(info.collegueBasicSkill);
            SetLevelCost();
        }
        else
        {
            Debug.Log("핑크칩의 개수가 부족합니다.");
        }
       
    }

}
