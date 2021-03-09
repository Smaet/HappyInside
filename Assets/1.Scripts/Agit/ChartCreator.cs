using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChartCreator : MonoBehaviour
{
    public Image[] image_chartPoints;
    public Image[] image_chartSticks;
    public Image image_chartLastPoints;
    public Image image_chartBackground;

    float standardPointY;

    float maxPercent;
    float minPercent;


                     

    //private void Start()
    //{
    //    StartChart();


    //}

    //private void Update()
    //{
    //    if(Input.GetKeyUp(KeyCode.Q))
    //    {
    //        StartCoroutine(StartChartTest());
    //    }
    //}

    public void StartChart()
    {
        standardPointY = 0;
        maxPercent = ((image_chartBackground.rectTransform.sizeDelta.y * 0.5f) - (image_chartLastPoints.rectTransform.sizeDelta.y * image_chartLastPoints.rectTransform.localScale.x));
        minPercent = -maxPercent;       
        StartCoroutine(ChartAnimation());


    }

    IEnumerator StartChartTest()
    {
        for (int nextPointIndex = 0; nextPointIndex < 5; nextPointIndex++)
        {
            yield return StartCoroutine(GetRandomRange());

            print("nextPointIndex : " + nextPointIndex + "  value : " + standardPointY);
        }
    }

    IEnumerator GetRandomRange()
    {
        while(true)
        {

            float nextPointY = Random.Range(-100, 100);

            if(Mathf.Abs(nextPointY - standardPointY) >= 15)
            {
                standardPointY = nextPointY;
                yield break;
            }
            

            yield return null;
        }
    }

    IEnumerator ChartAnimation()
    {

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// 다음 포인트들의 설정
        /// 1~4번 점은 전 단계 값과 15%이상 차이가 나게 랜덤 도출
      
        //print("maxPercent : " + maxPercent);

        bool isLoop = false;
        float result = 0;

        for (int j = 0; j < 1; j++)
        {
            isLoop = true;
            for (int nextPointIndex = 0; nextPointIndex < 5; nextPointIndex++)
            {
               

                yield return StartCoroutine(GetRandomRange());

                //print("nextPointIndex : " + nextPointIndex + "  value : " + standardPointY);

                float yPos = 0;
                yPos = (maxPercent * standardPointY) / 100;
                //if (standardPointY < 0)
                //{
                //    yPos = -yPos;
                //}


                image_chartPoints[nextPointIndex].rectTransform.anchoredPosition = new Vector2(image_chartPoints[nextPointIndex].rectTransform.anchoredPosition.x, yPos);
                if (nextPointIndex != 0)
                {
                    image_chartSticks[nextPointIndex].rectTransform.anchoredPosition = image_chartPoints[nextPointIndex - 1].rectTransform.anchoredPosition;
                }



            }

            result = standardPointY;


            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// 다음 포인트에 대한 차트 설정
            float destTime = 1f;
            int i = 0;


            Vector3 dir = image_chartPoints[i].rectTransform.position - image_chartSticks[i].rectTransform.position;
            //print("dir : " + dir);

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float distance = Mathf.Abs(Vector3.Distance(image_chartPoints[i].rectTransform.localPosition, image_chartSticks[i].rectTransform.localPosition));
            float newDistance = distance / 0.3f;
            //print("newDistance : " + newDistance);
            float sizeX;
            float dest;
            float curTime = 0;

            sizeX = image_chartSticks[i].rectTransform.sizeDelta.x;
            image_chartSticks[i].rectTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            while (isLoop)
            {


                if (curTime < destTime)
                {

                    dest = Mathf.Lerp(sizeX, newDistance, curTime / destTime);
                    image_chartSticks[i].rectTransform.sizeDelta = new Vector2(dest, image_chartSticks[i].rectTransform.sizeDelta.y);
                }
                else
                {

                    i++;

                    if (i > 4)
                    {
                        ChartReset();
                        standardPointY = 0;
                        i = 0;
                        isLoop = false;
                    }

                    dir = image_chartPoints[i].rectTransform.position - image_chartSticks[i].rectTransform.position;
                    angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    image_chartSticks[i].rectTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


                    distance = Mathf.Abs(Vector3.Distance(image_chartPoints[i].rectTransform.position, image_chartSticks[i].rectTransform.position));
                    newDistance = distance / 0.3f;
                    sizeX = image_chartSticks[i].rectTransform.sizeDelta.x;
                    dest = 0;
                    curTime = 0;
                }

                curTime += Time.deltaTime;

                yield return null;
            }
        }


        print("결과 : " + result);
        
        
        
       
    }

    void ChartReset()
    {
        for(int i=0; i < image_chartSticks.Length; i++)
        {
            image_chartSticks[i].rectTransform.sizeDelta = new Vector2(100, 100);
        }
    }

}
