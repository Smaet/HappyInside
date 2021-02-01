using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum SlideButtonIndex
{
    ACTIVE = -1,
    AGIT_A = 0,
    AGIT_B,
    GRANDFATHER,
    DEPARTMENTSTORE,
    CASINO,
    AIRPORT,
    SPACELAB,
}


public class HomeSlideButton : MonoBehaviour
{
    [SerializeField]
    private SlideButtonIndex slideButtonIndex;

    [SerializeField]
    private Button button;
    


    public void Init()
    {
        if(button == null)
        {
            button = GetComponent<Button>();
        }
    }

    public SlideButtonIndex GetButtonIndex()
    {
        return slideButtonIndex;
    }

    public Button GetButton()
    {
        return button;
    }
}
