using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShoppingItem : MonoBehaviour
{
    [SerializeField]
    private Image shoppingItem_Image;
    [SerializeField]
    private TextMeshProUGUI price_Text;

    public void SetItem(string _price, string _name )
    {
       shoppingItem_Image.sprite = null;

       Sprite sprite = Resources.Load<Sprite>("ShoppingItems/" + _name);
       shoppingItem_Image.sprite = sprite;
       price_Text.text = _price;
    }
}
