using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour, IEndDragHandler
{
    public static Character instance;


    public enum State
    {
        None,
        Idle,
        Run,
        Jump,
        Dead
    }

    [SerializeField] private bool CharacterAvailable;
    public State CurrenState;
    [SerializeField] public Animator CharacterAnimator;

    
    // Character Movement
    public float CharacterSpeed;
    private float HorizontalMove;
    private Vector2 Movement;
    [SerializeField] private Rigidbody2D rigidbody2D;


    // Character Jump
    public float CharacterJumpPower;
    [SerializeField]private bool isJumping;

    // Character Position
    private Vector3 InitPostion;
    
    //Events
    public static Action OnUIEvents;



    public void Init()
    {
        CurrenState = State.None;
        Movement = Vector2.zero;
        isJumping = false;
        transform.position = InitPostion;
        rigidbody2D.velocity = Vector2.zero;
        CharacterDeactivate();
    }


    private void Awake()
    {
        instance = this;
        InitPostion = transform.position;



       
    }

    // Start is called before the first frame update
    void Start()
    {
        CharacterAnimator = GetComponent<Animator>();
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();


        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (CharacterAvailable)
        {
            Run();

            AnimatorSync();
        }

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.dragging);
    }  

    public void CharacterActivate()
    {
        CharacterAvailable = true;
        rigidbody2D.gravityScale = 10.0f;

    }

    public void CharacterDeactivate()
    {
        CharacterAvailable = false;
        rigidbody2D.gravityScale = 0.0f;
    }


    #region Input Event Receive

    public void OnJump()
    {
        if (isJumping == false && rigidbody2D.velocity.y <= 0)
        {
            StartCoroutine(JumpCoroutine());
        }

    }

   

    public void OnMove(InputAction.CallbackContext context)
    {
        //Debug.Log(context.phase);
        
        OnUIEvents();

        if(context.phase != InputActionPhase.Canceled)
        {
            //Debug.Log(context.ReadValue<Vector2>());
            HorizontalMove = context.ReadValue<Vector2>().x;
        }
        else
        {
            //HorizontalMove = 0;
            //Debug.Log(context.ReadValue<Vector2>());
        }
       
      

        //HorizontalMove = context.ReadValue<Vector2>().x;
        Movement.Set(HorizontalMove, 0);

        GUIManager.instance.SetDebugMessage(HorizontalMove.ToString());

        //Texture flip
        if (HorizontalMove > 0)
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        else
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
    }

   

    public void OnMoveMobile(bool Left)
    {
        if (Left)
        {
            HorizontalMove = -1;
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            HorizontalMove = 1;
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        Movement.Set(HorizontalMove, 0);
    }
    #endregion

    private void Run()
    {
        Movement = Movement.normalized * CharacterSpeed * Time.deltaTime;
        rigidbody2D.MovePosition((Vector2)transform.position + Movement);
    }

   

    private void AnimatorSync()
    {
        CharacterAnimator.SetBool("Jump", isJumping);
        CharacterAnimator.SetFloat("Movement", Math.Abs(Movement.x));
    }



    public void GamePlayStart()
    {
        ChangeCurrentCharacterState(State.Idle);

        CharacterActivate();
    }


    private void ChangeCurrentCharacterState(State state)
    {
        CurrenState = state;
    }

    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    throw new NotImplementedException();
    //}

    IEnumerator JumpCoroutine()
    {
        isJumping = true;

        rigidbody2D.AddForce(Vector2.up * CharacterJumpPower);

        yield return new WaitForSecondsRealtime(1f);

        isJumping = false;

        Debug.Log("JUMP!!!!");
    }
}
 