using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 请求者类，设置并调用命令
/// 请求者为单例模式
/// </summary>
public class Invoker
{
    private List<Command> commands;
    private static Invoker _instance;

    public static Invoker GetInstance()
    {
        if (_instance == null)
        {
            _instance = new Invoker();
        }
        return _instance;
    }

    /// <summary>
    /// 设置命令
    /// </summary>
    /// <param name="command"></param>
    public void SetCommand(Command command)
    {
        commands.Add(command);
    }

    /// <summary>
    /// 执行命令
    /// </summary>
    public void ExecutrCommand()
    {
        foreach (var item in commands)
        {
            item.Execute();
        }
    }
}