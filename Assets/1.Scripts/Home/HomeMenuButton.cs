using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeMenuButton : MonoBehaviour
{
    public Button button;

    public void Init()
    {
        if(button == null)
        {
            button = GetComponent<Button>();
        }
    }
}
