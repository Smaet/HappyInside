using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavigation : MonoBehaviour
{
    [SerializeField]
    public Stack<MyUIView> historyUI;

    public void Init()
    {
        historyUI = new Stack<MyUIView>();
    }

    public void PushHistory(MyUIView _view)
    {
        historyUI.Push(_view);
    }

    public MyUIView PopHistory()
    {
        return historyUI.Pop();
    }

    
}
