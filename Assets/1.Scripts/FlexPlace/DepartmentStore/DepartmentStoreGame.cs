using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Polo;

public class DepartmentStoreGame : BaseFlexPlaceGame
{
    #region Events
    public static Action OnCurtainComplete;
    #endregion

    [Header("Start")]
    public StartPanel startPanel;

    [Header("ShoppingList")]
    [SerializeField]
    private int shoppingCounts = 0;
    [SerializeField]
    private Image ShoppingItem;
    public ShoppingItem[] shoppingItems;
    public List<int> curShoppingIndexList;      //현재 상단에 보여지는 쇼핑 리스트의 인덱스들
    private bool isCurio = false;               //진품인지 아닌지 판단
    private int curShoppingItemPrice = 0;
    private string prevShoppingItem = "";

    [Header("Curtain")]
    public CurtainPanel curtainPanel;

    [Header("Clerk")]
    public ClerkPanel clerkPanel;

    [Header("Combo")]
    [SerializeField]
    private int combo;                          //콤보
    public SimpleObjectPool combo_ObjectPool;
    public GameObject comboLocation;

    [Header("Score")]
    [SerializeField]
    private Text score_Text;                    
    [SerializeField]
    private int score;                        //점수(결제 금액 +)
    public SimpleObjectPool getMoneyText_ObjectPool;
    public GameObject getMoneyTextShowingLocation;


    [Header("Time")]
    public TimeComponent timeComponent;
    [SerializeField]
    private int totalPlayTime;            //전체 플레이 시간


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
    private float cardResetToZeroTime;      //카드가 원래대로 되돌아가는데 까지의 시간             
    [SerializeField]
    private Color cardReaderOnColor;
    [SerializeField]
    private Color cardReaderOffColor;

    protected override void Init()
    {
        curtainPanel.Init(this);
        InitCombo();
        InitMoney();
        InitCardReader();
        InitShoppingList();


        SetCardReader(false);
    }

    public override void OpenFlexPlaceGame()
    {
        Debug.Log("DepartmentStore Game Call!!!");
        base.OpenFlexPlaceGame();
        //각종 초기화
        Init();
        //시작 신호
        startPanel.StartSign(this);
    }

    private void Awake()
    {
        //각종 초기화
        Init();
        //시작 신호
        startPanel.StartSign(this);
    }

    public void StartGame()
    {
        curtainPanel.OnSkipButton();
        StartTimer();
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

        if (shoppingCounts >= 8)
        {
            for (int i = 0; i < shoppingItems.Length; i++)
            {
                int RandomIndex = UnityEngine.Random.Range(0, Count);

                shoppingItems[i].SetItem(shoppingList[shoppingCounts + i]["Price"].ToString(), shoppingList[shoppingCounts + i]["Curio"].ToString());

                shoppingList.RemoveAt(RandomIndex);

                Count = shoppingList.Count;

                curShoppingIndexList.Add(RandomIndex);
            }
        }
        else
        {
            //123 -> 234 -> 345 -> 456
            for (int i = 0; i < shoppingItems.Length; i++)
            {
                int index = shoppingCounts + i;
                shoppingItems[i].SetItem(shoppingList[index]["Price"].ToString(), shoppingList[index]["Curio"].ToString());
                curShoppingIndexList.Add(index);
            }

            shoppingCounts++;
        }
    }

    struct ShoppingItemTemp
    {
        public int price;
        public string Name;
    }


    //위의 쇼핑리스트에 뜬 3가지 중 총 15가지수 중에 나온다.
    public void SetShoppingItem()
    {
        List<Dictionary<string, object>> shoppingList = CSVReader.Read(CSVReadType.RESOURCE, "ShoppingList");

        List<Dictionary<string, object>> curshoppingList = new List<Dictionary<string, object>>();


        for (int i=0; i < curShoppingIndexList.Count; i++)
        {
            int curIndex = curShoppingIndexList[i];
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Price", shoppingList[curIndex]["Price"]);
            if (prevShoppingItem != shoppingList[curIndex]["Curio"].ToString())
            {
                dic.Add("Curio", shoppingList[curIndex]["Curio"]);
            }
            if (prevShoppingItem != shoppingList[curIndex]["Fake_1"].ToString())
            {
                dic.Add("Fake_1", shoppingList[curIndex]["Fake_1"]);
            }
            if (prevShoppingItem != shoppingList[curIndex]["Fake_2"].ToString())
            {
                dic.Add("Fake_2", shoppingList[curIndex]["Fake_2"]);
            }
            if (prevShoppingItem != shoppingList[curIndex]["Fake_3"].ToString())
            {
                dic.Add("Fake_3", shoppingList[curIndex]["Fake_3"]);
            }
            if (prevShoppingItem != shoppingList[curIndex]["Fake_4"].ToString())
            {
                dic.Add("Fake_4", shoppingList[curIndex]["Fake_4"]);
            }

            curshoppingList.Add(dic);
        }

        int index_Key = UnityEngine.Random.Range(0, 3);
        int index_Value = UnityEngine.Random.Range(1, 6);

        switch(index_Value)
        {
            case 1:
                ShoppingItem.sprite = GetSprite(curshoppingList[index_Key]["Curio"].ToString());
                curShoppingItemPrice = int.Parse(curshoppingList[index_Key]["Price"].ToString());
                isCurio = true;
                prevShoppingItem = curshoppingList[index_Key]["Curio"].ToString();
                break;
            case 2:
                ShoppingItem.sprite = GetSprite(curshoppingList[index_Key]["Fake_1"].ToString());
                curShoppingItemPrice = int.Parse(curshoppingList[index_Key]["Price"].ToString());
                isCurio = false;
                prevShoppingItem = curshoppingList[index_Key]["Curio"].ToString();
                break;
            case 3:
                ShoppingItem.sprite = GetSprite(curshoppingList[index_Key]["Fake_2"].ToString());
                isCurio = false;
                prevShoppingItem = curshoppingList[index_Key]["Curio"].ToString();
                break;
            case 4:
                ShoppingItem.sprite = GetSprite(curshoppingList[index_Key]["Fake_3"].ToString());
                isCurio = false;
                prevShoppingItem = curshoppingList[index_Key]["Curio"].ToString();
                break;
            case 5:
                ShoppingItem.sprite = GetSprite(curshoppingList[index_Key]["Fake_4"].ToString());
                isCurio = false;
                prevShoppingItem = curshoppingList[index_Key]["Curio"].ToString();
                break;
        }
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
        timeComponent.StartCountDownTimer(totalPlayTime);

        //StartCoroutine(Test());
    }

    public void AddTime(int _time)
    {
        timeComponent.AddTime(_time);
    }
    public void MinusTime(int _time)
    {
        timeComponent.MinusTime(_time);
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

    private void InitCardReader()
    {
        Debug.Log("CardReset!!");
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
            if (_value >= 0.2f) //20% 이상일때
            {
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

        InitCardReader();
        //다음 랜덤 상품
        curtainPanel.OnSkipButton();

        //1로 갔다가 0으로 가야함.

        while (true)
        {
            if (timeToOne >= cardResetToZeroTime)
            {
                isSlashComplete = true;
            }
            else if(timeToOne >= cardResetToZeroTime)
            {
                yield break;
            }



            //1로 감
            if (isSlashComplete == false)
            {
                resetValue = Mathf.Lerp(curValue, 1, timeToOne / cardResetToZeroTime);
                timeToOne += Time.deltaTime;
            }
            //0으로 감
            else
            {
                resetValue = Mathf.Lerp(1, 0, timeToZero / cardResetToZeroTime);
                timeToZero += Time.deltaTime;
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

        GameObject comboObject = combo_ObjectPool.GetObject();
        BaseCombo bCombo = comboObject.GetComponent<BaseCombo>();

        bCombo.SetInfo("Combo " + combo.ToString() + "!!", combo_ObjectPool.transform, comboLocation.transform);

        comboObject.SetActive(true);
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
        score += _score;
        score_Text.text = score.ToString();
        string addScore = _score.ToString();

        GameObject moneyObject = getMoneyText_ObjectPool.GetObject();
        MoneyCombo bMoneyCombo = moneyObject.GetComponent<MoneyCombo>();

        bMoneyCombo.SetInfo("+" + addScore , getMoneyText_ObjectPool.transform, getMoneyTextShowingLocation.transform);

        moneyObject.SetActive(true);
    }

    IEnumerator TestMoneyCombo()
    {
        while(true)
        {
            AddMoney(100000000);

            yield return new WaitForSeconds(1);
        }
    }

    #endregion
}
