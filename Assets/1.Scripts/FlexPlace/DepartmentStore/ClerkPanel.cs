using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClerkPanel : MonoBehaviour
{

    [SerializeField]
    private Text clerkName_Text;

    [SerializeField]
    private Text clerkContext_Text;


    private void Awake()
    {
        Init();
        gameObject.SetActive(false);
    }

    public void Init()
    {
        clerkName_Text.text = "점원";
        clerkContext_Text.text = " 00고객님\n오늘도 오셨군요!";
    }

    public void SetInfo(string _clerkName, string _clerkContext)
    {
        clerkName_Text.text = _clerkName;
        clerkContext_Text.text = _clerkContext;
    }

    public void StartAnimation()
    {
        gameObject.SetActive(true);
    }

    public void OnAnimationEnd()
    {
        gameObject.SetActive(false);
    }

}
