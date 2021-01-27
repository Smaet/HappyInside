using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseCombo : MonoBehaviour
{
    private Animator animator;
    private TextMeshProUGUI combo_Text;
    
    
    public virtual void Awake()
    {
        if(animator == null)
        {
            animator = gameObject.GetComponent<Animator>();
        }
        if(combo_Text == null)
        {
            combo_Text = gameObject.GetComponent<TextMeshProUGUI>(); 
        }

        float alpha = combo_Text.color.a;
        alpha = 0;
        Color color = new Color(combo_Text.color.r, combo_Text.color.g, combo_Text.color.b, alpha);
        combo_Text.color = color;

        gameObject.SetActive(false);
    }
    public virtual void SetInfo(string _comboInfo, Transform _parent, Transform _location)
    {
        combo_Text.text = _comboInfo;
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        gameObject.transform.SetParent(_parent);
        gameObject.transform.localPosition = _location.localPosition;
    }


    public void OnAnimationEnd()
    {
        GetComponent<PooledObject>().pool.ReturnObject(gameObject);
    }
}
