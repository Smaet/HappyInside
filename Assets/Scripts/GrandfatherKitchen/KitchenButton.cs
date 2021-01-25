using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitchenButton : MonoBehaviour
{

    public Image ButtonImage;

    public int FoodIndex;

    // Start is called before the first frame update
    void Start()
    {
        ButtonImage = gameObject.GetComponent<Image>();



        if (ButtonImage == null)
        {
            Debug.LogWarning("KitChenButton have some issues");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    // 버튼에 음식 할당  /  매니저->버튼
    public void SetFoodToButton(int foodidx, Sprite foodSprite)
    {
        FoodIndex = foodidx;
        ButtonImage.sprite = foodSprite;

        Debug.Log(FoodIndex);
    }



    // 버튼 클릭   /  버튼->매니저
    public void OnClick()
    {
        KitchenGameManager.instance.CorrectCheck(FoodIndex);
    }


}
