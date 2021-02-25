using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollegueView_Sad : CollegueView
{
    public override void Init()
    {
        base.Init();
        print(gameObject.name + " Init!!!");
        HomeManager.Instance.agitManager.OnClickCollegueButton_Sad += OpenCollegueView;
    }


    public override void OpenCollegueView(CollegueIndex _index)
    {
        base.OpenCollegueView(_index);
        print(_index + "View 활성화!!");

        SetName("쌔드");
    }
    protected override void SetSkillContext(CollegueBasicSkill _skill)
    {

    }

    protected override void OnButtonLevelUpClick()
    {
        base.OnButtonLevelUpClick();

        CollegueInfo info = GameManager.Instance.user.userBaseProperties.collegueInfos[(int)curCollegue];
        //스킬업 가능 조건 레벨보다 핑크칩의 갯수가 크거나 같으면
        if (GameManager.Instance.user.userBaseProperties.blackChip >= info.Level)
        {

            if (info.Level < 30)
            {
                //핑크칩 갯수 다운
                GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.BLACKCHIP, (long)-info.Level);

                //스킬 레벨 업
                info.Level++;

                //스킬레벨업에 따른 패시브 스킬 확인
                if (info.Level == 10)
                {
                    info.colleguePassiveSkills[0].isActive = true;
                    //패시브 스킬 창 갱신
                    SetPassiveSkill(true, 0);
                    //아지트 마다 레벨에 따른 UI 갱신
                    HomeManager.Instance.agitManager.agitOffice.colleguePanels[0].SetUI(info);

                    //패시브 1 -> 작동시간 1시간 감소
                    if (info.colleguePassiveSkills[0].isApply == false)
                    {
                        info.collegueBasicSkill.hour -= info.colleguePassiveSkills[0].hour;
                        info.colleguePassiveSkills[0].isApply = true;

                        print("쌔드 패시브 1 활성화!!");
                    }
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

    //아이템 레벨업 버튼
    protected override void OnButtonItemLevelUpClick()
    {
        base.OnButtonItemLevelUpClick();
        CollegueInfo info = GameManager.Instance.user.userBaseProperties.collegueInfos[(int)curCollegue];

        //스킬업 가능 조건 레벨보다 블랙칩의 갯수가 크거나 같으면
        if (GameManager.Instance.user.userBaseProperties.blackChip >= info.Level)
        {
            if (info.itemLevel >= 30)
            {
                levelUpItem_button.interactable = false;
            }
            else
            {
                //작동시간 ??시간 감소
                //??% 씩 레벨이 오를 때 마다 상승 
                info.collegueItem.hour++;
            }
        }
    }
}
