using UnityEngine;
using System.Collections;

public class Test_EventTriggerListener : MonoBehaviour
{
    public GameObject Click;

    public GameObject DoubleClick;

    public GameObject OnHoverImage;

    public GameObject Obecjt_2D;

    // Use this for initialization
    private void Start()
    {
        Click.SetClick((p) => { Debug.Log("测试EventTriggerListener脚本 单击 功能"); });
        DoubleClick.SetDoubleClick((p) => { Debug.Log("测试EventTriggerListener脚本 双击 功能"); });
        OnHoverImage.SetHover((P) => { Debug.Log("测试EventTriggerListener脚本 悬浮 功能"); });
        Obecjt_2D.SetEnter((p) => { Debug.Log("测试EventTriggerListener脚本 2D物体进入 功能"); });
    }

    // Update is called once per frame
    private void Update()
    {
    }
}