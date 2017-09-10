using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using System.Net;
using UnityEngine.SceneManagement;
using System;

public class YiLianSDK : MonoBehaviour
{

    public static YiLianSDK instance;
    public string lingPai;
  
    private UserMessage userMessage;
    private static bool init;
    private string getGame_id;
    public string GetGame_id
    {
        get
        {
            if (getGame_id == "" || getGame_id == "null"|| getGame_id==null)
            {
                return "1";
            }
            else
            {
                return getGame_id;
            }
        }
        set
        {
            getGame_id = value;
        }
    }

    private UniWebView _webView;

    

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("没有联网！！！");
			GameObject.Find ("Canvas/PlayMain").GetComponent<Button> ().interactable = true;
        }
        else
        {
            Debug.Log("已联网！！！");
            StartCoroutine("SignIn");
        }  
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Application.loadedLevelName == "menu")
            {
                Debug.Log("物理返回键······················");
                Application.Quit();
            }
            else if (Application.loadedLevelName == "map")
            {
                SceneManager.LoadScene("menu");
            }
        }
    }
    IEnumerator SignIn()
    {
        //330ed57436e2ed2c8714d5eb705f6f9184bfaa82
        WWW www = new WWW("https://yilian.lefenmall.net/open/v1/api.php?c=user/profile&access_token=" + PlayerPrefs.GetString("Token", "12"));
        if (!www.isDone)
        {
            yield return www;
        }
        if (!www.text.Contains("invalid_token"))
        {
            userMessage = JsonUtility.FromJson<UserMessage>(www.text);
            lingPai = PlayerPrefs.GetString("Token", "");
            GetGame_id = "1";
            Debug.Log(userMessage.msg);
            Debug.Log(userMessage.data.photo);
        }
        else
        {
                /*Debug.Log("开始初始化！！！");
                if (!init)
                {
                    Init();
                    init = true;
                }
                Debug.Log("已初始化成功！！！");
                string res = currActivity.Call<string>("GetYiLianMessage", new object[] { });//获取益联JWD
                string[] temp = res.Split('|');
                GetGame_id = temp[1];
                Debug.Log("jwt:" + temp[0]);
                string jwt = "";
                if (temp[0] == "null")
                {
                    jwt = "";
                }
                else
                {
                    jwt = temp[0];
                }*/
			Debug.Log("跳过了获取信息");
			string jwt = "";
			GetGame_id = "12";
			CreatWebView("https://yilian.lefenmall.net/oauth/authorize.php?response_type=token&client_id=testclient_implicit&redirect_uri=https%3a%2f%2fyilian.lefenmall.net%2foauth%2freceive_implicit_token.php&state=xyz&scope=basic+integral&jwt=" + jwt);
        }  
    }

    private void CreatWebView(string url)
    {
        if (_webView != null)
        {
            _webView.url = url;
            //TODO
            return;
        }

        _webView = CreateWebView();
        _webView.url = url;

        int bottomInset = UniWebViewHelper.screenHeight;
        _webView.insets = new UniWebViewEdgeInsets(0, 0, 0, 0);


        _webView.OnLoadComplete += _webView_OnLoadComplete;

        _webView.Load();
        _webView.Show();
    }
    private UniWebView CreateWebView()
    {
        var webViewGameObject = GameObject.Find("WebView");
        if (webViewGameObject == null)
        {
            webViewGameObject = new GameObject("WebView");
        }

        var webView = webViewGameObject.AddComponent<UniWebView>();

        webView.toolBarShow = true;
        return webView;
    }
    private string getAccessToken(string url)
    {
        string queryString = url;
        string[] queries = queryString.Split('&');
        string[] queries2 = queries[0].Split('=');
        string lingpai = queries2[1];
        Debug.Log(lingpai);
        return lingpai;
    }
    IEnumerator GetUserMessage()
    {
        WWW www = new WWW("https://yilian.lefenmall.net/open/v1/api.php?c=user/profile&access_token=" + lingPai);
        if (!www.isDone)
        {
            yield return www;
        }
        userMessage = JsonUtility.FromJson<UserMessage>(www.text);
        Debug.Log(userMessage.data.photo);
    }
    private void _webView_OnLoadComplete(UniWebView webView, bool success, string errorMessage)
    {
        if (success)
        {
            if (webView.currentUrl.Contains("#access_token="))
            {

                lingPai = getAccessToken(webView.currentUrl);
                StartCoroutine("GetUserMessage");
                PlayerPrefs.SetString("Token", lingPai);
				_webView.Hide(true,UniWebViewTransitionEdge.Bottom,0.3f);
				_webView.HideToolBar(true);
            }
        }
    }
    public void YiLianPay(string packageName)
    {
        StartCoroutine("CreatPayUrl", packageName);
    }

    //创建支付的Url
    IEnumerator CreatPayUrl(string packageName)
    {
        Debug.Log("lingpai:"+ lingPai);

        //lingPai = "8e4f83f559a392115bdd7eea80c7505aa350906a";

        GameObject.Find("Canvas").transform.Find("YiLianZheZhao").gameObject.SetActive(true);

        string dizhi1 = "https://yilian.lefenmall.net/open/v1/api.php?c=game/submit_order&";
        StringBuilder sb = new StringBuilder(dizhi1, 1000);

        string game_id = GetGame_id;
        string out_trade_no = System.DateTime.Now.ToShortDateString().Replace("/", "");
        string body = packageName;//商品ID
        string detail = "";
        string users = userMessage.data.user_id;
        string deduct_integral = "";//扣除积分

        switch (packageName)
        {
            case "package1":
                deduct_integral = "1000";
                break;
            case "package2":
                deduct_integral = "5000";
                break;
            case "package3":
                deduct_integral = "10000";
                break;
            case "package4":
                deduct_integral = "15000";
                break;
            case "package5":
                deduct_integral = "25000";
                break;
            case "package6":
                deduct_integral = "30000";
                break;
            case "package7":
                deduct_integral = "2000";
                break;
            case "package8":
                deduct_integral = "7000";
                break;
            default:
                break;
        }

        Dictionary<string, string> payDic1 = new Dictionary<string, string>();
        payDic1.Add("access_token=", lingPai);
        payDic1.Add("game_id=", game_id);//暂时随便填
        payDic1.Add("out_trade_no=", out_trade_no);//暂时随便填
        payDic1.Add("body=", body);
        payDic1.Add("detail=", detail);
        payDic1.Add("users=", users);

        foreach (var item in payDic1)
        {
            sb.AppendFormat(item.Key);
            sb.AppendFormat(item.Value);
            if (!item.Key.Contains("users"))
            {
                sb.AppendFormat("&");
            }
        }
        Debug.Log(sb.ToString());

        string order_ID = "";
        WWW getData = new WWW(sb.ToString());
        KaiGuDingDan kaijudingdan = null;
        if (getData.error != null)
        {
            Debug.Log(getData.error);
        }
        else
        {
            yield return getData;
            
            kaijudingdan = JsonUtility.FromJson<KaiGuDingDan>(getData.text);
            order_ID = kaijudingdan.data.order_id;
            Debug.Log("生成了订单号：" + order_ID);
            Debug.Log("商品ID：" + kaijudingdan.data.body);
            Debug.Log("users:"+ kaijudingdan.data.users);
            Debug.Log("game_id:" + kaijudingdan.data.game_id);
            Debug.Log("out_trade_no:" + kaijudingdan.data.out_trade_no);

        }

        if (kaijudingdan.code == "1")//所有code为1才正确
        {
            string dizhi2 = "https://yilian.lefenmall.net/open/v1/api.php?c=game/integral_deduct&";
            StringBuilder sb2 = new StringBuilder(dizhi2, 500);
            Dictionary<string, string> payDic2 = new Dictionary<string, string>();
            payDic2.Add("access_token=", lingPai);
            payDic2.Add("deduct_integral=", deduct_integral);//暂时随便填
            payDic2.Add("order_id=", order_ID);//暂时随便填
            foreach (var item in payDic2)
            {
                sb2.AppendFormat(item.Key);
                sb2.AppendFormat(item.Value);
                if (!item.Key.Contains("order_id"))
                {
                    sb2.AppendFormat("&");
                }
            }
            WWW getData2 = new WWW(sb2.ToString());

            if (getData2.error != null)
            {
                Debug.Log(getData2.error);
            }
            else
            {
                yield return getData2;
                PayResult payResult;
                payResult = JsonUtility.FromJson<PayResult>(getData2.text);
                string code = payResult.code;
                if (code == "1")
                {
                    YiLianZheZhao.instance.Close(true, payResult.msg);
                    int addGemCount = 0;
                    switch (packageName)
                    {
                        case "package1":
                            addGemCount = 10;
                            break;
                        case "package2":
                            addGemCount = 50;
                            break;
                        case "package3":
                            addGemCount = 100;
                            break;
                        case "package4":
                            addGemCount = 150;
                            break;
                        default:
                            break;
                    }
                    InitScriptName.InitScript.Instance.AddGems(addGemCount);

                    //GUIManager.Instance.BuySucceed(body + "|20170829");//潜水艇的购买成功方法
                    //if (int.Parse(packageName.Remove(0, 7)) <= 6)//农场的购买方法
                    //{
                    //    GameObject.Find("DialogInapp/OpenIABEventManager").SendMessage("OnPurchaseSucceeded", packageName + "|20170001");
                    //}
                    //else if (int.Parse(packageName.Remove(0, 7)) == 7)
                    //{
                    //    DialogGift.instance.ThreeStarGift(packageName + "|20170001");
                    //}
                    //else
                    //{
                    //    DialogGift.instance.AddHeartGift(packageName + "|20170001");
                    //}

                    Debug.Log("扣除成功" + deduct_integral);

                }
                else
                {
                    Debug.Log("扣除失败，url为：" + getData2.url);
                    Debug.Log("扣除失败，网上获得的text为：" + getData2.text);
                    Debug.Log("扣除失败，第二个Code为：" + code);
                    Debug.Log("扣除失败，message为：" + payResult.message);
                    //Debug.Log("扣除失败，msg为：" + payResult.msg);
                    //Debug.Log("扣除失败，request：" + payResult.request);
                    //Debug.Log("扣除失败，access_token：" + payResult.data.access_token);
                    //Debug.Log("扣除失败，order_id：" + payResult.data.order_id);
                    YiLianZheZhao.instance.Close(false, payResult.msg);
                }
            }
        }
        else
        {
            Debug.Log("扣除失败第一个Code不为1");
            YiLianZheZhao.instance.Close(false, kaijudingdan.msg);
        }
    }


}











[Serializable]
public class KaiGuDingDan
{
    public string code;
    public string message;
    public string msg;
    public DataClass data;
    public string request;
}
[Serializable]
public class DataClass
{
    public string game_id;
    public string order_id;
    public string out_trade_no;
    public string body;
    public string users;
}
[Serializable]
public class PayResult
{
    public string code;
    public string message;
    public string msg;
    public ResultData data;
    public string request;
}
[Serializable]
public class ResultData
{
    public string order_id;
    public string out_trade_no;
    public string user_id;
    public string access_token;
    public string deduct_integral;
    public string user_integral_before;
    public string user_integral_later;
}
[Serializable]
public class UserMessage
{
    public string code;
    public string message;
    public string msg;
    public UserMessageData data;
    public string request;
}
[Serializable]
public class UserMessageData
{
    public string user_id;
    public string name;
    public string name_initial;
    public string photo;
    public string integral;
    public string rank;
    public string referrer;
    public string sex;
    public string status;
}