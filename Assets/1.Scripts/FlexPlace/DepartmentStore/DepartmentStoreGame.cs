using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Polo;
using TMPro;

public class DepartmentStoreGame : BaseFlexPlaceGame
{
    #region Events
    public static Action OnCurtainComplete;
    #endregion

    public bool isGamePlaying = false;

    [Header("Start")]
    public StartPanel startPanel;

    [Header("ShoppingList")]
    [SerializeField]
    private GameObject ShoppingListObject;
    [SerializeField]
    private int shoppingCounts = 0;
    [SerializeField]
    private Image ShoppingItem;
    public ShoppingItem[] shoppingItems;
    public List<int> curShoppingIndexList;      //현재 상단에 보여지는 쇼핑 리스트의 인덱스들
    private bool isCurio = false;               //진품인지 아닌지 판단
    private int curShoppingItemPrice = 0;
    private string prevShoppingItem = "";
    private int prevShoppingKey = -1;
    private int prevShoppingValue = -1;

    [Header("Curtain")]
    public CurtainPanel curtainPanel;

    [Header("Clerk")]
    public ClerkPanel clerkPanel;

    [Header("Combo")]
    [SerializeField]
    private int combo;                          //콤보
    private bool isSpecialComboActive = false;
    public SimpleObjectPool combo_ObjectPool;
    public GameObject comboLocation;


    [Header("Score")]
    [SerializeField]
    private TextMeshProUGUI score_Text;                    
    [SerializeField]
    private long score;                        //점수(결제 금액 +)
    private bool isScoreDouble;
    public SimpleObjectPool getMoneyText_ObjectPool;
    public GameObject getMoneyTextShowingLocation;


    [Header("Time")]
    public DepartmentGameTimer departmentGameTimer;
    [SerializeField]
    private int totalPlayTime;            //전체 플레이 시간
    private bool isPauseTimerOn = false;
    public Button freezeButton;


    [Header("CardReader")]
    [SerializeField]
    private Slider cardReader_Slider;
    [SerializeField]
    private Image cardReaderChecker_Image;
    [SerializeField]
    private bool isCardReaderOn;            //카드 리더기를 사용할수 있는지
    [SerializeField]
    private bool isCardReset;               //카드 리더기를 사용할수 있는지
    [SerializeField]
    private float cardResetToOneTime;      //카드가 원래대로 되돌아가는데 까지의 시간     
    [SerializeField]
    private float cardResetToZeroTime;      //카드가 원래대로 되돌아가는데 까지의 시간             
    [SerializeField]
    private Color cardReaderOnColor;
    [SerializeField]
    private Color cardReaderOffColor;

    [Header("Result")]
    [SerializeField]
    private ResultPanel resultPanel;


  

    private void OnEnable()
    {
        //이벤트 설정
        //시작 이벤트
        OnClickDepartmentMiniGameStart += StartGame;
        //난이도 설정이 추가 됨

        //시작 카운트
        StartSign();



    }

    private void OnDisable()
    {
        //이벤트 설정
        OnClickDepartmentMiniGameStart -= StartGame;
    }


    public override void Init()
    {
        score = 0;
        score_Text.text = "0";

        curtainPanel.Init(this);
        InitCombo();
        InitMoney();
        InitCardReader();
        InitShoppingList();

        departmentGameTimer.Init(this);

        resultPanel.Init();

        SetCardReader(false);

        ShoppingListObject.SetActive(false);
    }

    
    //private void Awake()
    //{
    //    //각종 초기화
    //    Init();
    //    //시작 신호
    //    startPanel.StartSign(this);
    //}

    public override void StartSign()
    {
        Init();
        startPanel.StartSign(this);
    }


    public void StartGame()
    {
        ShoppingListObject.SetActive(true);
        isGamePlaying = true;
        StartTimer();
        curtainPanel.StartGameCurtain();
    }

    public void SetEndGame()
    {
        isGamePlaying = false;
    }

    public void EndGame()
    {
        Debug.Log("게임종료!");
        //각종 실행중인 것들 종료
        //콤보 2배 이벤트
        isSpecialComboActive = false;
        GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.CONSUMPTION, score);
        GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.FLEXCONSUMPTION, score);
        GameManager.Instance.user.SetUserInfo(ChangeableUserProperties.PINKCHIP, (float)UnityEngine.Random.Range(5,10)); 
        //팝업창 생성 밑 확인 버튼으로 되돌아 가기.
        resultPanel.OnResultPanel(string.Format("{0:#,0}원", score));
    }

    public void EndGame(string _str)
    {
        Debug.Log(_str);
    }



    #region ShoppingList
    public void InitShoppingList()
    {
        isCurio = false;

        SetShoppingLists();
        SetShoppingItem();
    }

    //상단에 띄워 지는 쇼핑 리스트
    public void SetShoppingLists()
    {
        List<Dictionary<string, object>> shoppingList = CSVReader.Read(CSVReadType.RESOURCE, "ShoppingList"); 
        int Count = shoppingList.Count;

        curShoppingIndexList.Clear();

        for (int i = 0; i < shoppingItems.Length; i++)
        {
            int RandomIndex = UnityEngine.Random.Range(0, Count);

            shoppingItems[i].SetItem(string.Format("{0:#,0}원",shoppingList[RandomIndex]["Price"]), shoppingList[RandomIndex]["Curio"].ToString());
            //Debug.Log(shoppingList[RandomIndex]["Item"].ToString());
            int index = int.Parse(shoppingList[RandomIndex]["Index"].ToString());

            curShoppingIndexList.Add(index);

            //Debug.Log("index : " + curShoppingIndexList[i]);

            shoppingList.RemoveAt(RandomIndex);

            Count = shoppingList.Count;
         
        }
    }



    //위의 쇼핑리스트에 뜬 3가지 중 총 15가지수 중에 나온다.
    public void SetShoppingItem()
    {
        //이전에 나온 쇼핑 목록만 안겹치게 하기


        List<Dictionary<string, object>> shoppingList = CSVReader.Read(CSVReadType.RESOURCE, "ShoppingList");

        List<Dictionary<string, object>> curshoppingList = new List<Dictionary<string, object>>();


        for (int i = 0; i < curShoppingIndexList.Count; i++)
        {
            int curIndex = curShoppingIndexList[i] - 1;
            //Debug.Log("CurIndex : " + curIndex);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Price", shoppingList[curIndex]["Price"]);
            dic.Add("Curio", shoppingList[curIndex]["Curio"]);
            dic.Add("Fake_1", shoppingList[curIndex]["Fake_1"]);
            dic.Add("Fake_2", shoppingList[curIndex]["Fake_2"]);
            dic.Add("Fake_3", shoppingList[curIndex]["Fake_3"]);
            dic.Add("Fake_4", shoppingList[curIndex]["Fake_4"]);

            //Debug.Log("Index : " + curIndex + " Price : " + shoppingList[curIndex]["Price"].ToString() + "Curio : " +
            //    shoppingList[curIndex]["Curio"] + "Fake_1 : " + shoppingList[curIndex]["Fake_1"] + "Fake_2 : " + shoppingList[curIndex]["Fake_2"]
            //    + "Fake_3 : " + shoppingList[curIndex]["Fake_3"] + "Fake_4 : " + shoppingList[curIndex]["Fake_4"]);

            curshoppingList.Add(dic);
        }

        List<int> valueList = new List<int>();

        int index_Key = UnityEngine.Random.Range(0, 3);
        int index_Value = 0;

        //이전에 나왔던 쇼핑 아이템 제거
        if (prevShoppingKey != -1 && prevShoppingItem != "")
        {
            
            for (int i = 1; i < 6; i++)
            {
                if (i != prevShoppingValue)
                {
                    valueList.Add(i);
                }
                   
            }

            index_Value = UnityEngine.Random.Range(0, 4);
        }
        else
        {
            for (int i = 1; i < 6; i++)
            {
                valueList.Add(i);
            }

            index_Value = UnityEngine.Random.Range(0, 5);
        }



        index_Value = valueList[index_Value];
        //Debug.Log("PrevValue : " + prevShoppingKey +  "  index Key : " + index_Key + "  Index Value : " + index_Value);



        switch (index_Value)
        {
            case 1:
                ShoppingItem.sprite = GetSprite(curshoppingList[index_Key]["Curio"].ToString());
                curShoppingItemPrice = int.Parse(curshoppingList[index_Key]["Price"].ToString());
                isCurio = true;
                prevShoppingItem = "Curio";
                break;
            case 2:
                ShoppingItem.sprite = GetSprite(curshoppingList[index_Key]["Fake_1"].ToString());
                curShoppingItemPrice = int.Parse(curshoppingList[index_Key]["Price"].ToString());
                isCurio = false;
                prevShoppingItem = "Fake_1";
                break;
            case 3:
                ShoppingItem.sprite = GetSprite(curshoppingList[index_Key]["Fake_2"].ToString());
                curShoppingItemPrice = int.Parse(curshoppingList[index_Key]["Price"].ToString());
                isCurio = false;
                prevShoppingItem = "Fake_2";
                break;
            case 4:
                ShoppingItem.sprite = GetSprite(curshoppingList[index_Key]["Fake_3"].ToString());
                curShoppingItemPrice = int.Parse(curshoppingList[index_Key]["Price"].ToString());
                isCurio = false;
                prevShoppingItem = "Fake_3";
                break;
            case 5:
                ShoppingItem.sprite = GetSprite(curshoppingList[index_Key]["Fake_4"].ToString());
                curShoppingItemPrice = int.Parse(curshoppingList[index_Key]["Price"].ToString());
                isCurio = false;
                prevShoppingItem = "Fake_4";
                break;
        }

        prevShoppingKey = index_Key;

        //Debug.Log("Complete");
    }

    Sprite GetSprite(string _name)
    {
       return Resources.Load<Sprite>("ShoppingItems/" + _name);
    }
    #endregion

    #region Timer


    public void StartTimer()
    {
        departmentGameTimer.StartCountDownTimer(totalPlayTime);

        //StartCoroutine(Test());
    }

    public void AddTime(int _time)
    {
        departmentGameTimer.AddTime(_time);
    }
    public void MinusTime(int _time)
    {
        departmentGameTimer.MinusTime(_time);
    }

    public void PauseTimer()
    {
        if(isPauseTimerOn)
        {
            SetFreezeButton(false);
            SetCardReader(false);
            departmentGameTimer.SetPause(true);

            PoloSFX.Instance.Play_Miss();

            //가품인지 진품인지 판단 
            //진품
            if (isCurio)
            {
                InitCombo();    //콤보 리셋
            }
            //가품
            else
            { 
                AddCombo();     //콤보 추가
            }
        }
    }

    #endregion
    
    #region CardReader
    //카드 리더기를 밑으로 끝까지 땅기면(1) 자동으로 0으로 되돌아 감
    public void SetCardReader(bool _on)
    {
        //카드리더기 사용가능
        if (_on)
        {
            cardReader_Slider.interactable = true;
            cardReaderChecker_Image.color = cardReaderOnColor;
            isCardReaderOn = true;
        }
        //불가능
        else
        {
            cardReader_Slider.interactable = false;
            cardReaderChecker_Image.color = cardReaderOffColor;
            isCardReaderOn = false;
        }
    }

    public void SetFreezeButton(bool _on)
    {
        
        if (_on)
        {
            //시간 일시정기 버튼도 넣음
            freezeButton.interactable = true;
            //freezeButton.image.color = cardReaderOnColor;
            isPauseTimerOn = true;
        }
        //불가능
        else
        {
      
            freezeButton.interactable = false;
            //freezeButton.image.color = cardReaderOffColor;
            isPauseTimerOn = false;
        }
    }

    private void InitCardReader()
    {
        //Debug.Log("CardReset!!");
        cardReader_Slider.interactable = false;
        cardReader_Slider.value = 0;
        isCardReaderOn = false;
        isCardReset = true;
        cardReaderChecker_Image.color = cardReaderOffColor;

    }

    public void OnSlashCardReader(float _value)
    {
        if(isCardReaderOn && isCardReset)
        {
            PoloSFX.Instance.Play_CardSlice();
            if (_value >= 0.1f) //20% 이상일때
            {
                //입력 불가
                SetFreezeButton(false);
                SetCardReader(false);

                //가품인지 진품인지 판단 
                //진품
                if (isCurio)     
                {
                    AddCombo();     //콤보 추가
                    AddMoney(curShoppingItemPrice);
                    AddTime(1);
                }
                //가품
                else
                {
                    int price = curShoppingItemPrice / 100;     //1%만 추가
                    InitCombo();    //콤보 리셋
                    AddMoney(price);
                    MinusTime(2);
                }

                //카드 리더기 초기화
                isCardReset = false;
                isCardReaderOn = false;
                SetCardReader(false);
                StartCoroutine(ResetCardToZero());
               //자동으로 0으로 돌아감
            }
        }

    }

    private IEnumerator ResetCardToZero()
    {
        float curValue = cardReader_Slider.value;
        float resetValue = 0;

        float timeToOne = 0;
        float timeToZero = 0;

        bool isSlashComplete = false;

     

        //1로 갔다가 0으로 가야함.

        while (true)
        {
            if (timeToOne >= cardResetToOneTime && isSlashComplete == false)
            {
                isSlashComplete = true;
            }
            
            if(timeToZero >= cardResetToZeroTime)
            {
                cardReader_Slider.value = 0;
                isCardReset = true;
                yield break;
            }



            //1로 감
            if (isSlashComplete == false)
            {
                resetValue = Mathf.Lerp(curValue, 1, timeToOne / cardResetToOneTime);
                timeToOne += Time.deltaTime;
            }
            //0으로 감
            else
            {
                resetValue = Mathf.Lerp(1, 0, timeToZero / cardResetToZeroTime);
                timeToZero += Time.deltaTime;
                //Debug.Log(timeToZero);
            }


            cardReader_Slider.value = resetValue;


            yield return null;
        }
    }

    #endregion

    #region Combo
    public void InitCombo()
    {
        //현재 콤보 초기화
        combo = 0; 
    }

    void AddCombo()
    {
        combo++;
        //10 콤보 이상일때 금액 두배 발생

        if(combo >= 10 && isSpecialComboActive == false)
        {
            Debug.Log("스페셜 콤보 실행!");
            isScoreDouble = true;
            isSpecialComboActive = true;
            StartCoroutine(SpecialComboEvent());
        }

        GameObject comboObject = combo_ObjectPool.GetObject();
        BaseCombo bCombo = comboObject.GetComponent<BaseCombo>();

        bCombo.SetInfo("Combo " + combo.ToString() + "!!", combo_ObjectPool.transform, comboLocation.transform);

        comboObject.SetActive(true);
    }

    IEnumerator SpecialComboEvent()
    {

        float time = 0;
        isScoreDouble = true;
        while (isSpecialComboActive)
        {
            if(time >= 5)
            {
                isSpecialComboActive = false;
                isScoreDouble = false;
                yield break;
            }
            time += Time.deltaTime;
            yield return null;
        }
    }

    #endregion

    #region Score(Money)
    public void InitMoney()
    {
        //현재 콤보 초기화
        score = 0;
    }

    void AddMoney(int _score)
    {
        string addScore = "";
        if (isScoreDouble)
        {
            //Debug.Log("점수 2배!");
            addScore = (_score * 2).ToString();
            score += (_score * 2);
        }
        else
        {
            score += _score;
            addScore = _score.ToString();
        }
        //화면에 보여지는 스코어
        score_Text.text = string.Format("{0:#,0}원" , score);// GameManager.Instance.GetMoneyFormat(score);
       

        GameObject moneyObject = getMoneyText_ObjectPool.GetObject();
        MoneyCombo bMoneyCombo = moneyObject.GetComponent<MoneyCombo>();

        bMoneyCombo.SetInfo("+" + addScore , getMoneyText_ObjectPool.transform, getMoneyTextShowingLocation.transform);

        moneyObject.SetActive(true);
    }


    #endregion
}
