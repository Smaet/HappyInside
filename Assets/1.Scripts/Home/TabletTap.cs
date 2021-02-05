using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;

public class TabletTap : MonoBehaviour
{
    public UIView uiView;

    public void ShowTabletTap()
    {
        uiView.Show();
    }

    public void HideTabletTap()
    {
        uiView.Hide();
    }
}
