using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using UnityEngine;

public class SelfConversation : MonoBehaviour
{
    public TextAnimatorPlayer textAP;

    [SerializeField] private float initDelay;
    [SerializeField] private float loopDelay;

    [TextArea(3, 50), SerializeField] private string[] scr_loopSet;


    // Start is called before the first frame update
    void Start()
    {
        initDelay = Random.Range(0f, 4.0f);

        StartCoroutine(SelfConversationCoroutine(loopDelay));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SelfConversationCoroutine(float looptime)
    {
        yield return new WaitForSecondsRealtime(initDelay);

        int prev = -1;
        int cur = -1;

        while (true)
        {
            cur = Random.Range(0, scr_loopSet.Length);

            // 이전과 같으면 거르기
            if (cur != prev) prev = cur;
            else continue;
            

            textAP.ShowText(scr_loopSet[cur]);

            yield return new WaitForSecondsRealtime(looptime);
        }
    }
}
