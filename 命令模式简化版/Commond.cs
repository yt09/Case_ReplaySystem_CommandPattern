using UnityEngine;
using System.Collections;

/// <summary>
/// 命令类
/// </summary>
public class Command
{
    public IReceiver receiver;

    public Command(IReceiver receiver)
    {
        this.receiver = receiver;
    }

    public void Execute()
    {
        receiver.Action();
    }
}