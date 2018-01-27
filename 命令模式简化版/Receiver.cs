using UnityEngine;

/// <summary>
/// 命令接受者接口，回放系统涉及到的类都实现IReceiver接口
/// </summary>
public interface IReceiver
{
    void Action();
}