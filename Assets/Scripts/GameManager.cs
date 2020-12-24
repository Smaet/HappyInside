using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [Header("Managing"), SerializeField] private PlayerInput playerInput;




    // Construction
    [SerializeField] private int MaxBlockPoolCount;


    // Balance
    [SerializeField] private float CurrentBlockSpeed;
    [SerializeField] private int CurrentBlockPoolCount;


    // Score
    [SerializeField] private float DepthScore;
    public float DepthMultiple;

    public enum GameState
    {
        Ready,
        Playing,
        NotPlaying
    }

    public GameState CurrentGameState;


    #region Getter/Setter

    public float GetBlockSpeed()
    {
        return CurrentBlockSpeed;
    }

    public int GetCurrentBlockPoolCount()
    {
        return CurrentBlockPoolCount;
    }

    public int GetMaxBlockPoolCount()
    {
        return MaxBlockPoolCount;
    }




    private void SetBlockSpeed(float val)
    {
        CurrentBlockSpeed = val;
    }

    private void SetCurrentBlockPoolCount(int val)
    {
        CurrentBlockPoolCount = val;
    }

    private void SetMaxBlockPoolCount(int val)
    {
        MaxBlockPoolCount = val;
    }

    #endregion




    void init()
    {
        // Component Setup
        playerInput = GetComponent<PlayerInput>();


        // Temporary Setting
        SetCurrentBlockPoolCount(100);
        SetBlockSpeed(0.035f);

        SetMaxBlockPoolCount(500);
        DepthScore = 0;

    }




    private void Awake()
    {
        Screen.SetResolution(540,960, false);

        instance = this;

        init();
    }

    // Start is called before the first frame update
    void Start()
    {
        BlockGenenrator.instance.CreateBlockPool();

        GamePlayReady();
    }

    // Update is called once per frame
    void Update()
    {


        // Debug Key end
    }

    private void FixedUpdate()
    {
        if (CurrentGameState == GameState.Playing)
        {
            DepthScore += Time.deltaTime * DepthMultiple;
            GUIManager.instance.SetDepthScoreText(DepthScore.ToString());
        }
    }


    #region GameState Manage

    private void ChangeCurrentState(GameState state)
    {
        CurrentGameState = state;
    }

    public void GamePlayReady()
    {
        CurrentGameState = GameState.Ready;
        GUIManager.instance.MainMenuOn();
        playerInput.DeactivateInput();

        Character.instance.Init();
        Character.instance.CharacterDeactivate();
        BlockGenenrator.instance.UpdateBlockPool();
    }

    public void GamePlayStart()
    {
        if (CurrentGameState == GameState.Ready)
        {
            ChangeCurrentState(GameState.Playing);
            GUIManager.instance.GameMenuOn();

            playerInput.ActivateInput();

            BlockGenenrator.instance.GeneratorStart();
            Character.instance.GamePlayStart();
        }
    }
    public void GameOver(bool win)
    {

        if (CurrentGameState == GameState.Playing)
        {
            // Winner
            if (win)
            {
                Debug.Log("WIN!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                GUIManager.instance.ResultMenuOn(true);
            }

            // Loser
            else
            {
                Debug.Log(("Lose!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"));
                GUIManager.instance.ResultMenuOn(false);
            }

            playerInput.DeactivateInput();

            BlockGenenrator.instance.GeneratorReset();
            Character.instance.Init();
           

            StartCoroutine(GameEndCoroutine());
        }
        
    }

    IEnumerator GameEndCoroutine()
    {
        yield return new WaitForSecondsRealtime(5.0f);
        GamePlayReady();
    }

    #endregion




    #region GameControl

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion



}
