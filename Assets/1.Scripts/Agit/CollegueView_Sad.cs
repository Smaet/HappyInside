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
    }
    protected override void SetSkillContext(CollegueBasicSkill _skill)
    {

    }

    protected override void OnButtonLevelUpClick()
    {
    }
}
