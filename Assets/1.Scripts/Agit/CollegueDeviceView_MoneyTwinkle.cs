using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollegueDeviceView_MoneyTwinkle : CollegueDeviceView
{
    public override void ClickLevelUp()
    {
        base.ClickLevelUp();

        CollegueInfo info = GameManager.Instance.user.userBaseProperties.collegueInfos[(int)CollegueIndex.Sad];
        //스킬업 가능 조건 레벨보다 핑크칩의 갯수가 크거나 같으면
        //if (GameManager.Instance.user.userBaseProperties.xCoin >= info.Level)
        //{

        //    if (info.Level < 30)
        //    {
        //        //핑크칩 갯수 다운
        //        GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.BLACKCHIP, (long)-info.Level);

        //        //스킬 레벨 업
        //        info.Level++;


        //        //스킬레벨업에 따른 패시브 스킬 확인
        //        if (info.Level == 10)
        //        {
        //            info.colleguePassiveSkills[0].isActive = true;
        //            //패시브 스킬 창 갱신
        //            //SetPassiveSkill(true, 0);
        //            //아지트 마다 레벨에 따른 UI 갱신
        //            //HomeManager.Instance.agitManager.agitOffice.colleguePanels[0].SetUI(info);

        //            //패시브 1 -> 해킹시간 1시간 감소
        //            if (info.colleguePassiveSkills[0].isApply == false)
        //            {
        //                info.collegueBasicSkill.hour -= info.colleguePassiveSkills[0].hour;
        //                info.colleguePassiveSkills[0].isApply = true;
        //            }
        //        }
        //        else if (info.Level == 20)
        //        {
        //            info.colleguePassiveSkills[1].isActive = true;
        //            //패시브 스킬 창 갱신
        //            //SetPassiveSkill(true, 1);
        //            //아지트 마다 레벨에 따른 UI 갱신
        //            //HomeManager.Instance.agitManager.agitOffice.colleguePanels[0].SetUI(info);

        //        }
        //        else if (info.Level == 30)
        //        {
        //            info.colleguePassiveSkills[2].isActive = true;
        //            //패시브 스킬 창 갱신
        //            //SetPassiveSkill(true, 2);
        //            //아지트 마다 레벨에 따른 UI 갱신
        //            HomeManager.Instance.agitManager.agitOffice.colleguePanels[0].SetUI(info);

        //            button_LevelUp.interactable = false;
        //        }

        //        //유저 정보에 반영
        //        //GameManager.Instance.user.SetUserInfo(curCollegue, info);

        //        Debug.Log("현재 해커의 능력으로 소멸되는 돈의 양 : " + info.collegueBasicSkill.money);

        //        //UI 반영
        //        //SetLevel(info.Level);
        //        //SetSkillContext(info.collegueBasicSkill);
        //        //SetLevelCost();
        //    }
        //}
        //else
        //{
        //    button_LevelUp.interactable = false;
        //    Debug.Log("블랙칩의 개수가 부족합니다.");
        //}
    }
}
