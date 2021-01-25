using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandFatherHouseManager : MonoBehaviour
{
    public KitchenGameManager kitchenGame;

    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        kitchenGame.Initialize();
    }

}
