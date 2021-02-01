using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgitSlideButtonPanel : BaseSlideButtonPanel
{
    public override void SetButton()
    {
        base.SetButton();


        slideActiveButton.GetButton().onClick.AddListener(StartButtonAutoSlide);

        for (int i = 0; i < slideButtons.Length; i++)
        {
            slideButtons[i].GetButton().onClick.AddListener(StartButtonAutoSlide);

            if (slideButtons[i].GetButtonIndex() == SlideButtonIndex.AGIT_A)
            {
                slideButtons[i].GetButton().onClick.AddListener(PoloSFX.Instance.PlayAgitBGM);
            }
        }

    }
}
