using UnityEngine;
using System.Collections;

public class YiLianTimeLimit : MonoBehaviour {

    public int year, month, day, hour, min, sec;

    public float limitYear=2018;
    public float limitMonth=5;

    public string timeURL = "http://cgi.im.qq.com/cgi-bin/cgi_svrtime";

    void Start()
    {
        StartCoroutine(GetTime());
    }

    IEnumerator GetTime()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            string[] times = System.DateTime.Now.ToShortDateString().Split('/');
            int systemYear = int.Parse(times[2]);
            int systemMonth = int.Parse(times[0]);
            if (systemYear < limitYear || systemYear == limitYear && systemMonth <= limitMonth)
            {
                Debug.Log("符合");
            }
            else
            {
                Debug.Log("时间戳不符合,退出游戏");
                Application.Quit();
            }
        }
        else
        {
            WWW www = new WWW(timeURL);
            while (!www.isDone)
            {
                yield return www;
            }
            Debug.Log("网络上获取到的时间:" + www.text);
            if (www.text != null && www.text != "")
            {
                SplitTime(www.text);
                if (year < limitYear || year == limitYear && month <= limitMonth)
                {
                    Debug.Log("符合");
                }
                else
                {
                    Debug.Log("时间戳不符合,退出游戏");
                    Application.Quit();
                }
            }
        }
    }

    void SplitTime(string dateTime)
    {
        dateTime = dateTime.Replace("-", "|");
        dateTime = dateTime.Replace(" ", "|");
        dateTime = dateTime.Replace(":", "|");
        string[] Times = dateTime.Split('|');
        year = int.Parse(Times[0]);
        month = int.Parse(Times[1]);
        day = int.Parse(Times[2]);
        hour = int.Parse(Times[3]);
        min = int.Parse(Times[4]);
        sec = int.Parse(Times[5]);
    }
}
