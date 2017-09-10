using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class YiLianZheZhao : MonoBehaviour {

    public static YiLianZheZhao instance;
    public GameObject YiLianCube;
    private float timer = 0;
    void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        transform.Find("Text").GetComponent<Text>().text = "请稍等...";
        if (YiLianCube)
        {
            YiLianCube.gameObject.SetActive(true);
        }
        timer = 0;
        Time.timeScale = 1;
    }
    public void Close(bool success, string deduct_integral)
    {
        transform.Find("Text").GetComponent<Text>().text = deduct_integral;
        StartCoroutine("WaitClose");
    }
    IEnumerator WaitClose()
    {
        yield return new WaitForSeconds(0.6f);
        if (YiLianCube)
        {
            YiLianCube.gameObject.SetActive(false);
        }
        transform.gameObject.SetActive(false);
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 5)
        {
            timer = 0;
            Close(false, "兑换失败，请检查是否登录或者网络问题！");
        }
    }
}
