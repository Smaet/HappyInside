
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Test")]
public class UserData : ScriptableObject, ISerializationCallbackReceiver
{
    public string nickName;            //닉네임
    public int crystal;              //크리스탈
    public int startMoney;          //시작재산 (고정된 재산)
    public int money;                  //현재 가지고 있는 돈 (실제 재산)
    public int manipulatedMoney;       //현재 조작된 돈
    public int resultMoney;         //현재 잔액
    public int recentChangeMoney;    //최근 변화된 돈
    public float gameTime;            //게임 시간
    public float daysElapsed;         //경과된 일수
    public float doubt;               //의심도
    public float blackCoin;         //블랙 코인


    //public float InitialValue;

    //[NonSerialized]
    //public float RuntimeValue;


    public void OnAfterDeserialize()
    {
        //RuntimeValue = InitialValue;
    }

    public void OnBeforeSerialize() 
    {

    }

}
