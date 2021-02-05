using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollegueDeviceView_ProductionMachine : CollegueDeviceView
{
    [SerializeField]
    private TextMeshProUGUI tmp_RemainTime;
    [SerializeField]
    private Slider slider_RemainTime;

    //아이템 마다 풀 존재
    public SimpleObjectPool[] itemPools;
    [SerializeField]
    private Transform transform_wattingList;
    [SerializeField]
    private Transform transform_completeList;
    [SerializeField]
    //현재 생산중인 아이템
    private ProductionItem curProductItem;
    [SerializeField]
    private List<ProductionItem> inProductionItems = new List<ProductionItem>();           //생산중인 아이템들
    [SerializeField]
    private List<ProductionItem> completeProductionItems = new List<ProductionItem>();     //생산이 완료된 아이템들

    private bool isRunningProduct = false;
    private bool isMaking = false;

    private void Start()
    {
        itemPools[0].PreloadPool();
    }



    public void AddProductionItem()
    {
        GameObject pItemObject = itemPools[0].GetObject();
        ProductionItem pItem = pItemObject.GetComponent<ProductionItem>();
        pItemObject.transform.localScale = Vector3.one;
        pItemObject.transform.SetParent(transform_wattingList, false);


        inProductionItems.Add(pItem);
        //맨 마지막에 들어간 
        if(curProductItem == null)
        {
            curProductItem = inProductionItems[0];
        }

        if(isRunningProduct == false)
        {
            isRunningProduct = true;
            StartItemProduction();
        }
    }

    public void AddCompleteItem()
    {
       
        completeProductionItems.Add(curProductItem);

        curProductItem.gameObject.transform.SetParent(transform_completeList, false);

        inProductionItems.RemoveAt(0);
        if(inProductionItems.Count != 0)
        {
            curProductItem = inProductionItems[0];
        }

    }

    public void StartItemProduction()
    {
        StartCoroutine(ItemProuct(1));
    }

    IEnumerator ItemProuct(int _time)
    {
        float timeMagnification = 5;  //시간 배율
        int totalTime = _time;
        float time = 0;
        int timeChecker = 1;
        float finTIme = timeMagnification * totalTime;
        slider_RemainTime.maxValue = finTIme;
    
        time = finTIme / 240f;

        if(finTIme < 24 * timeMagnification)
        {
            tmp_RemainTime.text = string.Format("남은 시간 : {0}시간", totalTime);
        }
        else
        {
            int day = totalTime / 24;
            int hour = totalTime % 24;

            tmp_RemainTime.text = string.Format("남은 시간 : {0}시간 {1}일", hour, day);
        }

        while (true)
        {
            if(inProductionItems.Count != 0)
            {
                if(isMaking == false)
                {
                    isMaking = true;
                    slider_RemainTime.value = 0;
                }
                else
                {
                    if (slider_RemainTime.value >= finTIme)
                    {
                        AddCompleteItem();
                        if(inProductionItems.Count == 0)
                        {
                            Debug.Log("생산 끝");
                         
                            tmp_RemainTime.text = "생산 완료!";
                        }
                        else
                        {
                            timeChecker = 1;

                            if (finTIme < 24 * timeMagnification)
                            {
                                tmp_RemainTime.text = string.Format("남은 시간 : {0}시간", totalTime);
                            }
                            else
                            {
                                int day = totalTime / 24;
                                int hour = totalTime % 24;

                                tmp_RemainTime.text = string.Format("남은 시간 : {0}시간 {1}일", hour, day);
                            }


                            Debug.Log("다음 아이템 생산 시작");
                        }

                        isMaking = false;
                    }
                    //남은 시간 텍스트 갱신
                    else
                    {
                        
                        if(slider_RemainTime.value >= timeChecker * timeMagnification)
                        {
                           
                            int remainTime = totalTime - (timeChecker);

                            if (remainTime < 24)
                            {
                                tmp_RemainTime.text = string.Format("남은 시간 : {0}시간", remainTime);
                            }
                            else
                            {
                                int day = remainTime / 24;
                                int hour = remainTime % 24;

                                tmp_RemainTime.text = string.Format("남은 시간 : {0}시간 {1}일", hour, day);
                            }

                            timeChecker++;

                        }
                    }

                    slider_RemainTime.value += time;
                    //print(slider_RemainTime.value);
                    yield return new WaitForSeconds(time);
                }
               
            }
            else
            {
                yield return null;
            }
           
        }
    }

    public void GetItems()
    {
        for(int i=0; i < completeProductionItems.Count; i++)
        {
            completeProductionItems[i].GetComponent<PooledObject>().pool.ReturnObject(completeProductionItems[i].gameObject);
        }
        completeProductionItems.Clear();
        print("인벤토리에 추가!");
    }

 
    public void OnClickItemGet()
    {

    }
}
