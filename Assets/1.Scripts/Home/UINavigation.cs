using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavigation : MonoBehaviour
{
    [SerializeField]
    public Stack<UIView> historyUI;

    public void Init()
    {
        historyUI = new Stack<UIView>();
    }

    public void PushHistory(UIView _view)
    {
        historyUI.Push(_view);
    }

    public UIView PopHistory()
    {
        return historyUI.Pop();
    }

    
}
