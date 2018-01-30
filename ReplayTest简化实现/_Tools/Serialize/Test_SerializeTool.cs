using UnityEngine;
using System.Collections;
using MyUnitySDK.IO;
using MyUnitySDK.SerializeTool;
using System;

public class Test_SerializeTool : MonoBehaviour

{
    // Use this for initialization
    private void Start()
    {
        People yt = new People("杨涛", 24, Gender.Male);

        // Unity自带序列化工具
        //SerializeTool.serializeTool = new JsonUtilitySerializeTool();
        //yt.Describe = "这是使用 Unity自带序列化工具 测试SerializeTool序列化(反序列化)工具的脚本";

        // JsonDotNet插件序列化工具
        SerializeTool.serializeTool = new JsonDotNetSerializeTool();
        yt.Describe = "这是使用  JsonDotNet插件序列化工具 测试SerializeTool序列化(反序列化)工具的脚本";

        IO.ioTool = new FileStreamIOTool(Application.dataPath + "/Resources");
        IO.Write("Test_SerializeTool_Data.txt", System.Text.Encoding.UTF8.GetBytes(SerializeTool.Serialize(yt)));
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            People getFromFile = SerializeTool.Deserialize<People>(System.Text.Encoding.UTF8.GetString(IO.Read("Test_SerializeTool_Data.txt")));
            Debug.Log("姓名:" + getFromFile.Name);
            Debug.Log("年龄:" + getFromFile.Age);
            Debug.Log("性别:" + getFromFile.Gender.ToString());
            Debug.Log("描述:" + getFromFile.Describe);
        }
    }
}

[Serializable]
public class People
{
    public string Name;
    public int Age;
    public Gender Gender;

    //描述
    public string Describe;

    public People()
    {
    }

    public People(string name, int age, Gender gender)
    {
        this.Name = name;
        this.Age = age;
        this.Gender = gender;
    }

    public People(string name, int age, Gender gender, string describe)
    {
        this.Name = name;
        this.Age = age;
        this.Gender = gender;
        this.Describe = describe;
    }
}

public enum Gender { Male, FeMale }