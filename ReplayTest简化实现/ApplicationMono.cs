//includes for Unity
using UnityEngine;

//includes for System
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 名称空间定义：Replay
/// </summary>
namespace Replay
{
    /// <summary>
    /// 类 名 称：Replay.ApplicationMono
    /// 类 功 能：
    /// 主要接口：
    /// </summary>
    public class ApplicationMono : MonoBehaviour
    {
        public Button loadBtn;
        public Button saveBtn;
        public Button playBtn;

        /// <summary>
        ///
        /// </summary>
        private void Start()
        {
            saveBtn.gameObject.SetClick((p) => ReplayManager.Instance.SaveReplayData());
            loadBtn.gameObject.SetClick((p) => ReplayManager.Instance.LoadReplayData());
            playBtn.gameObject.SetClick((p) => ReplayManager.Instance.Replay = true);
        }
    }
}