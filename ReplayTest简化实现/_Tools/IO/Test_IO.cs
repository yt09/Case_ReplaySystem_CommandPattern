using UnityEngine;
using System.Collections;
using MyUnitySDK.IO;
using System.IO;

public class Test_IO : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        IO.ioTool = new FileStreamIOTool(Application.dataPath + "/Resources");
        IO.Write("TestIOData.txt", System.Text.Encoding.UTF8.GetBytes("这是测试IO写入读取工具的脚本"));
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            string data = System.Text.Encoding.UTF8.GetString(IO.Read("TestIOData.txt"));
            Debug.Log("文件内容是: " + data);
        }
    }
}