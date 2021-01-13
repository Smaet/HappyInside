using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UDPSettingInfo
{
    public string ip;
    public string sendport;
    public string receiveport;
}


public class SettingInfo
{
    public string ip;
    public string sendport;
    public string receiveport;
    public string contentIndex;
    public string contentSendInfo;
    public int recruitTime;
    public int playTime;
    public int resultTime;
    public float distance;
    public float mindistance;
}

public class StreamingDataParser : MonoBehaviour
{
    public string[] settingName;

    public SettingInfo settingInfo;
    public bool bIsFinishData;


    // Start is called before the first frame update
    //void Awake()
    //{
  
    //}

    public void Init()
    {
        settingInfo = new SettingInfo();
        bIsFinishData = false;
        StartCoroutine(loadStreamingAsset(settingName[0] + ".txt"));
    }

    IEnumerator loadStreamingAsset(string fileName)
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

        string result;

        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            UnityWebRequest www = new UnityWebRequest(filePath);
            yield return www;
            result = www.downloadHandler.text;
            Debug.Log("Hokuyo option path : " + result);
        }
        else
        {
            result = System.IO.File.ReadAllText(filePath);
        }

        List<string> line = LineSplit(result);

        SettingInfo info = new SettingInfo();


        for (int i = 0; i < line.Count; i++)
        {
            if (line[i] == null) continue;

            string[] Cells = line[i].Split(" : "[0]);    // cell split, tab
            if (Cells[0] == "") continue;

            switch (i)
            {
                case 0:
                    info.ip = Cells[2];
                    break;
                case 1:
                    info.sendport = Cells[2];
                    break;
                case 2:
                    info.receiveport = Cells[2];
                    break;
                case 3:
                    info.contentIndex = Cells[2];
                    break;
                case 4:
                    info.contentSendInfo = Cells[2];
                    break;
                case 5:
                    info.recruitTime = int.Parse(Cells[2]);
                    break;
                case 6:
                    info.playTime = int.Parse(Cells[2]);
                    break;
                case 7:
                    info.resultTime = int.Parse(Cells[2]);
                    break;
                case 8:
                    info.distance = float.Parse(Cells[2]);
                    break;
                case 9:
                    info.mindistance = float.Parse(Cells[2]);
                    break;
            }
        }

        settingInfo = info;
        Debug.Log("IP : " + info.ip);
        Debug.Log("Send PORT : " + info.sendport);
        Debug.Log("컨텐츠 이름 : " + info.contentIndex);
        //ip_Text.text = info.ip;
        //port_Text.text = info.sendport;
        //contentsName_Text.text = info.contentIndex;

        //GameMgr.Instance.udpSender.Init(settingInfo.ip, int.Parse(settingInfo.sendport), settingInfo.contentIndex);
        //GameMgr.Instance.udpReceiver.port = int.Parse(settingInfo.receiveport);
        //GameMgr.Instance.udpReceiver.StartReceive();
        //GameMgr.Instance.alma_Kinect_DepthImage.Distance = info.distance;
        //GameMgr.Instance.alma_Kinect_DepthImage.minDistance = info.mindistance;
        //GameMgr.Instance.SetTimes(settingInfo.recruitTime, settingInfo.playTime, settingInfo.resultTime);

        //ASModuleManager.instance.ip = info.ip;
        //ASModuleManager.instance.sendPort = int.Parse(info.sendport);
        //ASModuleManager.instance.receivePort = int.Parse(info.receiveport);
        //ASModuleManager.instance.contentsIndex = info.contentIndex;
        //ASModuleManager.instance.contentSendInfo = info.contentSendInfo;
        Debug.Log("<color=red> ip, port, contentIndex 파싱 완료!</color>");
        bIsFinishData = true;

        //Debug.Log("Loaded file: " + result);
    }

    public List<string> LineSplit(string text)
    {
        char[] text_buff = text.ToCharArray();

        List<string> lines = new List<string>();

        int linenum = 0;
        bool makecell = false;

        StringBuilder sb = new StringBuilder("");

        for (int i = 0; i < text.Length; i++)
        {
            char c = text_buff[i];

            if (c == '"')
            {
                char nc = text_buff[i + 1];
                if (nc == '"') { i++; } //next char
                else
                {
                    if (makecell == false) { makecell = true; c = nc; i++; } //next char
                    else { makecell = false; c = nc; i++; } //next char
                }
            }

            //0x0a : LF ( Line Feed : 다음줄로 캐럿을 이동 '\n')
            //0x0d : CR ( Carrage Return : 캐럿을 제일 처음으로 복귀 )             
            if (c == '\n' && makecell == false)
            {
                char pc = text_buff[i - 1];
                if (pc != '\n') //file end
                {
                    lines.Add(sb.ToString()); sb.Remove(0, sb.Length);
                    linenum++;
                }
            }
            else if (c == '\r' && makecell == false)
            {
            }
            else
            {
                sb.Append(c.ToString());
            }
        }

        return lines;
    }
}
