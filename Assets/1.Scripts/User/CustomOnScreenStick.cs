using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

public class CustomOnScreenStick : OnScreenStick, IEndDragHandler
{
    // Start is called before the first frame update
    public void Start()
    {
        Debug.Log("22");
    //    base.Start();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drag End!!");
    }
}
