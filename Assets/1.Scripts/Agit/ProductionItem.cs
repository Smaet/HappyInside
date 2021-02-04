using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ProductionItemIndex
{
    NONE = -1,
    SAMPLE = 0,
}



public class ProductionItem : MonoBehaviour
{
    public Image icon;              //생산중   이미지
   

    //완성중인지 아닌지 판단
    public virtual void SetItem(ProductionItemIndex _itemIndex, bool _isComplete)
    {
        switch(_itemIndex)
        {
            case ProductionItemIndex.SAMPLE:
                if(_isComplete)
                {
                    icon.sprite = Resources.Load<Sprite>("ProductionItem/flex_ice_temp");
                }
                else
                {
                    icon.sprite = Resources.Load<Sprite>("ProductionItem/flex_hell_temp");
                }

                break;
        }
    }
}
