using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class DepartmentStoreGame : BaseFlexPlaceGame
{
    public Text text;
    public TimeComponent timeComponent;
    [SerializeField]
    private float totalPlayTime;            //전체 플레이 시간
    [SerializeField]
    private float score;                    //점수(결제 금액 +)
    [SerializeField]
    private int combo;                      //콤보
    private int accumulatedConsumption;     //누적 소비액

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

    public override void OpenFlexPlaceGame()
    {
        Debug.Log("DepartmentStore Game Call!!!");
        base.OpenFlexPlaceGame();
    }

    #region Timer

    //시간을 추가하거나 빼야 한다면 한번 멈추고 다시 시작을 해야함.

    private bool isTimeOn = false;

    public void StartTimer()
    {
        timeComponent.StartCountDownTimer(30);

        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(3);

        timeComponent.AddTime(5);

        yield return new WaitForSeconds(2);

        timeComponent.MinusTime(5);
    }

    #endregion
    

    #region CardReader
    //카드 리더기를 밑으로 끝까지 땅기면(1) 자동으로 0으로 되돌아 감
    public void SetCardReader(bool _on)
    {
        StartTimer();

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
            //Debug.Log("Checking CardReader... : " + _value);

            if (_value >= 0.99f) //99% 이상일때
            {
                isCardReset = false;
                isCardReaderOn = false;
                StartCoroutine(ResetCardToZero());
               //자동으로 0으로 돌아감
            }
        }

    }

    private IEnumerator ResetCardToZero()
    {
        float curValue = cardReader_Slider.value;
        float resetValue = 0;
        float time = 0;
        while(true)
        {
            if(time >= cardResetToZeroTime)
            {
                InitCardReader();
                yield break;
            }

            resetValue = Mathf.Lerp(curValue, 0, time / cardResetToZeroTime);
            time += Time.deltaTime;
            cardReader_Slider.value = resetValue;
            yield return null;
        }
    }

    #endregion
}
