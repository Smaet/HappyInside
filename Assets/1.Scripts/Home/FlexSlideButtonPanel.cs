using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexSlideButtonPanel : BaseSlideButtonPanel
{
    public override void SetButton()
    {
        base.SetButton();

        slideActiveButton.GetButton().onClick.AddListener(StartButtonAutoSlide);

        for (int i = 0; i < slideButtons.Length; i++)
        {
            slideButtons[i].GetButton().onClick.AddListener(StartButtonAutoSlide);

            if (slideButtons[i].GetButtonIndex() == SlideButtonIndex.GRANDFATHER)
            {
                slideButtons[i].GetButton().onClick.AddListener(PoloSFX.Instance.PlayGrandFatherBGM);
            }
            if (slideButtons[i].GetButtonIndex() == SlideButtonIndex.DEPARTMENTSTORE)
            {
                slideButtons[i].GetButton().onClick.AddListener(PoloSFX.Instance.PlayDepartmentBGM);
            }
        }

       
    }
}
