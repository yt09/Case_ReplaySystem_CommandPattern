using UnityEngine;
using System.Collections;
using MyUnitySDK.IO;
using MyUnitySDK.Crypto;

/// <summary>
/// 测试加密解密,压缩与解压缩
/// </summary>
public class Test_Crypto : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        IO.ioTool = new FileStreamIOTool(Application.dataPath + "/Resources");

        //简单加密
        //Crypto.cryptoTool = new SimpleCryptoTool();
        //IO.Write("TestCryptoData.txt", Crypto.Encrypt(System.Text.Encoding.UTF8.GetBytes("这是测试Crypto(加密,解密)工具的脚本")));

        //压缩与解压缩
        Crypto.cryptoTool = new GZipCryptoTool();
        IO.Write("TestCryptoData.txt", Crypto.Encrypt(System.Text.Encoding.UTF8.GetBytes("这是测试Crypto(压缩,解压缩)工具的脚本")));
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            string data = System.Text.Encoding.UTF8.GetString(Crypto.Decrypt(IO.Read("TestCryptoData.txt")));
            Debug.Log("文件内容是: " + data);
        }
    }
}