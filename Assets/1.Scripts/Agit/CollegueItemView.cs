﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollegueItemView : MonoBehaviour
{
    public CollegueIndex curCollegue;


    public Image collegueItemImage;
    public TextMeshProUGUI name_TMP;
    public TextMeshProUGUI level_TMP;
    public TextMeshProUGUI SkillContext_TMP;

    public TextMeshProUGUI collegueItemProfile_TMP;
    public TextMeshProUGUI levelUpCost_TMP;

    public Button levelUp_button;

    public void OpenCollegueItemView(collegueInfo _info)
    {
        CollegueIndex index = _info.collegueIndex;
        curCollegue = index;

        switch (index)
        {
            case CollegueIndex.HACKER:
                SetName("X-다레");
                SetLevel(_info.itemLevel);
                SetSkillContext();
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
        level_TMP.text = string.Format("{0}", _Level);
    }

    public void SetSkillContext()
    {
        collegueInfo info = GameManager.Instance.user.userBaseProperties.collegueInfos[(int)curCollegue];
        switch (curCollegue)
        {
            case CollegueIndex.HACKER:
                SkillContext_TMP.text = "해커의 조작된 재산 생성량 " + info.collegueItem.chance + "% 증가";
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
        levelUpCost_TMP.text = info.itemLevel + "개 필요";
    }

    public void OnButtonLevelUpClick()
    {
        collegueInfo info = GameManager.Instance.user.userBaseProperties.collegueInfos[(int)curCollegue];
        //스킬업 가능 조건 레벨보다 핑크칩의 갯수가 크거나 같으면
        if (GameManager.Instance.user.userBaseProperties.pinkChip >= info.itemLevel)
        {
            //핑크칩 갯수 다운
            GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.PINKCHIP, -info.itemLevel);

            //동료의 아이템 레벨 업
            info.itemLevel++;

            //아이템 레벨업에 따른 해커의 조작된 재산 생성량 증가
            info.collegueItem.chance += 1;



            //유저 정보에 반영
            GameManager.Instance.user.SetUserInfo(curCollegue, info);

            Debug.Log("현재 해커의 조작된 재산 생성량 추가 증가량 : " + info.collegueItem.chance);

            //UI 반영
            SetLevel(info.Level);
            SetSkillContext();
            SetLevelCost();
        }
        else
        {
            Debug.Log("핑크칩의 개수가 부족합니다.");
        }

    }
}
