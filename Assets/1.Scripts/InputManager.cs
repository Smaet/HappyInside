using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using UnityEngine.InputSystem.EnhancedTouch;
using Lean.Touch;


//스크립트 실행 순서
public class InputManager : SimpleSingleton<InputManager>
{
    #region  LeanTouch old Input System
    public void Touch(LeanFinger finger)
    {
        Debug.Log(finger.Index + " Finger Touch!!!");
    }

    public void Touching(LeanFinger finger)
    {
        Debug.Log(finger.Index + " Finger Touch!!!");
    }

    #endregion
    #region New Input System

    /*
    public delegate void StartTouchEvent(Vector2 screenPos, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 screenPos, float time);
    public event EndTouchEvent OnEndTouch;

    private HappyInsideInputs happyInsideInputs;

    protected override void Awake()
    {
        base.Awake();

        happyInsideInputs = new HappyInsideInputs();
    }

    private void OnEnable() 
    {
        happyInsideInputs.Enable();
        TouchSimulation.Enable();

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable() 
    {
        happyInsideInputs.Disable();
        TouchSimulation.Disable();

       UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void Start() {

        
        //happyInsideInputs.HappyInsidePlayer.TouchInput.performed += context => PerformTouch(context);
       


        happyInsideInputs.HappyInsidePlayer.TouchPress.started += context => StartTouch(context);
        //happyInsideInputs.HappyInsidePlayer.TouchPress.performed += context => PerformTouch(context);
        happyInsideInputs.HappyInsidePlayer.TouchPress.canceled += context => CancelTouch(context);
        
        happyInsideInputs.HappyInsidePlayer.TouchPress1.started += context => StartTouch2(context);
        //happyInsideInputs.HappyInsidePlayer.TouchPress1.performed += context => PerformTouch(context);
        happyInsideInputs.HappyInsidePlayer.TouchPress1.canceled += context => CancelTouch(context);

   
    }

    private void StartTouch(InputAction.CallbackContext context)
    { 
        
        if(EventSystem.current.gameObject.layer == LayerMask.NameToLayer("TouchArea"))
        {
            if(OnStartTouch != null)
            {
                OnStartTouch(happyInsideInputs.HappyInsidePlayer.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);
            }
        }
        //Debug.Log(context.control.path + " / " + happyInsideInputs.HappyInsidePlayer.TouchPosition.ReadValue<Vector2>());

      
    }

    private void StartTouch2(InputAction.CallbackContext context)
    { 

        //Debug.Log(context.control.path + " / " + happyInsideInputs.HappyInsidePlayer.TouchPosition1.ReadValue<Vector2>());

        if(OnStartTouch != null)
        {
            OnStartTouch(happyInsideInputs.HappyInsidePlayer.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);

        }
    }

    private void PerformTouch(InputAction.CallbackContext context)
    {
          Debug.Log("Hold");
    }

    private void CancelTouch(InputAction.CallbackContext context)
    {
        if(EventSystem.current.gameObject.layer != LayerMask.NameToLayer("UI"))
        {
            if(OnEndTouch != null)
            {
                OnEndTouch(happyInsideInputs.HappyInsidePlayer.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);
            }
        }
    }

    private void FingerDown(Finger finger)
    {
        if(OnStartTouch != null)
        {
            OnStartTouch(finger.screenPosition, Time.time);
        }
    }

    
    // private void Update() 
    // {
    //     //Debug.Log(UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count);

    //     Debug.Log(UnityEngine.InputSystem.EnhancedTouch.Touch.activeFingers);

    //     // foreach(UnityEngine.InputSystem.EnhancedTouch.Touch.activeFingers finger in UnityEngine.InputSystem.EnhancedTouch.Touch.activeFingers)
    //     // {
    //     //     Debug.Log(finger)
    //     // }

    //     //  Touchscreen touchscreen = Touchscreen.current;

    //     // if(touchscreen != null)
    //     // {
    //     //     for (int i = 0; i < touchscreen.touches.Count; i++)
    //     //     {
    //     //         TouchControl touch = touchscreen.touches[i];
    //     //         if(i < 2)
    //     //         {
    //     //         string touchInfo = touch.touchId.ReadValue() + "\n"
    //     //             + touch.phase.ReadValue().ToString() + "\n"
    //     //             + touch.position.ReadValue().ToString() + "\n"
    //     //             + touch.pressure.ReadValue().ToString() + "\n"
    //     //             + touch.radius.ReadValue().ToString() + "\n"
    //     //             + touch.delta.ReadValue().ToString();
               
    //     //        Debug.Log(touchInfo);
    //     //         }
              
    //     //     }
    //     // }
         
    // }
    */
    #endregion

}
