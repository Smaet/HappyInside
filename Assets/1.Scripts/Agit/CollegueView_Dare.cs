﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollegueView_Dare : CollegueView
{
    public override void Init()
    {
        base.Init();
        print(gameObject.name + " Init!!!");
        HomeManager.Instance.agitManager.OnClickCollegueButton_Dare += OpenCollegueView;
    }

    public override void OpenCollegueView(CollegueIndex _index)
    {
        base.OpenCollegueView(_index);
        print(_index + "View 활성화!!");

        CollegueInfo info = GameManager.Instance.user.userBaseProperties.collegueInfos[(int)_index];
        CollegueIndex index = _index;
        curCollegue = index;

        SetName("X-다레");
        SetLevel(info.Level);
        SetSkillContext(info.collegueBasicSkill);
        SetLevelCost();
        SetManipulateMoney();
        Init_SetPassiveSkill();
    }

    protected override void SetSkillContext(CollegueBasicSkill _skill)
    {
        base.SetSkillContext(_skill);

        SkillContext_TMP.text = GameManager.Instance.GetMoneyFormat(_skill.money);
    }


    protected override void OnButtonLevelUpClick()
    {
        base.OnButtonLevelUpClick();

        CollegueInfo info = GameManager.Instance.user.userBaseProperties.collegueInfos[(int)curCollegue];
        //스킬업 가능 조건 레벨보다 핑크칩의 갯수가 크거나 같으면
        if (GameManager.Instance.user.userBaseProperties.xCoin >= info.Level)
        {

            if (info.Level < 30)
            {
                //핑크칩 갯수 다운
                GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.XCOIN, (long)-info.Level);

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
                    HomeManager.Instance.agitManager.agitOffice.colleguePanels[0].SetUI(info);

                }
                else if (info.Level == 20)
                {
                    info.colleguePassiveSkills[1].isActive = true;
                    //패시브 스킬 창 갱신
                    SetPassiveSkill(true, 1);
                    //아지트 마다 레벨에 따른 UI 갱신
                    HomeManager.Instance.agitManager.agitOffice.colleguePanels[0].SetUI(info);
                }
                else if (info.Level == 30)
                {
                    info.colleguePassiveSkills[2].isActive = true;
                    //패시브 스킬 창 갱신
                    SetPassiveSkill(true, 2);
                    //아지트 마다 레벨에 따른 UI 갱신
                    HomeManager.Instance.agitManager.agitOffice.colleguePanels[0].SetUI(info);

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
            Debug.Log("엑스코인의 개수가 부족합니다.");
        }

    }
}