

//includes for Unity
using UnityEngine;

//includes for System
using System.Collections;

/// <summary>
/// 名称空间定义：Replay
/// </summary>
namespace Replay
{

	/// <summary>
    /// 类 名 称：Replay.ReplayInit
	/// 类 功 能：
	/// 主要接口：
    /// </summary>
	public class ReplayInstantiate : MonoBehaviour 
	{
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void InitReplay()
        {
            GameObject go = new GameObject("_ReplayManager");
            go.AddComponent<ReplayManager>();
        }
	}
}

