using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingItem : MonoBehaviour
{
    [SerializeField]
    private Image shoppingItem_Image;
    [SerializeField]
    private Text price_Text;

    public void SetItem(string _price, string _name )
    {
       shoppingItem_Image.sprite = null;

       Sprite sprite = Resources.Load<Sprite>("ShoppingItems/" + _name);
       shoppingItem_Image.sprite = sprite;
       price_Text.text = string.Format("{0 : 0,000,000}", _price);
    }
}
