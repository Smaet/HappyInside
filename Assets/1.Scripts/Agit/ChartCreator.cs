using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChartCreator : MonoBehaviour
{
    public Image[] image_chartPoints;
    public Image[] image_chartSticks;

    private void Start()
    {
        StartChart();
    }

    public void StartChart()
    {
        
        //모든 stick 다음 point로 rotation 돌리기
        //Vector3 dir = image_chartPoints[0].rectTransform.position - image_chartSticks[0].rectTransform.position;


        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //image_chartSticks[0].rectTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        StartCoroutine(ChartAnimation());


    }

    IEnumerator ChartAnimation()
    {

        
        float speed = 1.2f;
        int i = 0;


        Vector3 dir = image_chartPoints[i].rectTransform.position - image_chartSticks[i].rectTransform.position;


        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        image_chartSticks[i].rectTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        print("11");
        float distance = Mathf.Abs(Vector3.Distance(image_chartPoints[i].rectTransform.position, image_chartSticks[i].rectTransform.position));
        print("distance : " + distance);
        float newDistance = distance / 0.3f;
        float sizeX = 0;
        float curTime = 0;
        float dest = 0;
        sizeX = image_chartSticks[i].rectTransform.sizeDelta.x;


        while (true)
        {
            if (i > 4)
            {
                yield break;
            }

            if (curTime < 1)
            {

                dest = Mathf.Lerp(sizeX, newDistance, curTime / 1);
                image_chartSticks[i].rectTransform.sizeDelta = new Vector2(dest, image_chartSticks[i].rectTransform.sizeDelta.y);
            }
            else
            {
          
                i++;

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

}
