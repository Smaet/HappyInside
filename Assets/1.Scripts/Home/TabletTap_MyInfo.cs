using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabletTap_MyInfo : TabletTap
{
    [SerializeField]
    private Button button_MyInfo;
    [SerializeField]
    private Button button_TargetInfo;
    [SerializeField]
    private ScrollRect scrollRect_MyInfo;
    [SerializeField]
    private ScrollRect scrollRect_TargetInfo;

    public override void Init()
    {
        base.Init();
    }

    public override void ShowTap()
    {
        base.ShowTap();

        button_MyInfo.interactable = false;
        button_TargetInfo.interactable = true;

        ShowMyInfo();
    }

    public override void HideTap()
    {
        base.HideTap();
    }

    private void OnEnable()
    {
        button_MyInfo.onClick.AddListener(ShowMyInfo);
        button_TargetInfo.onClick.AddListener(ShowTargetInfo);

    }

    private void OnDisable()
    {
        button_MyInfo.onClick.RemoveAllListeners();
        button_TargetInfo.onClick.RemoveAllListeners();
    }

    protected override void UpdateInfo()
    {
        base.UpdateInfo();

        //내정보 업데이트 
        //1. 뱃지
        //2. 조직이름, 랭킹, 등급, 효과,
        //3. 조직 멤버.
    }

    public void ShowMyInfo()
    {
        UpdateInfo();

        button_MyInfo.interactable = false;
        button_TargetInfo.interactable = true;

        scrollRect_MyInfo.gameObject.SetActive(true);
        scrollRect_TargetInfo.gameObject.SetActive(false);

    }

    public void ShowTargetInfo()
    {
        UpdateInfo();

        button_MyInfo.interactable = true;
        button_TargetInfo.interactable = false;

        scrollRect_MyInfo.gameObject.SetActive(false);
        scrollRect_TargetInfo.gameObject.SetActive(true);
    }

}
