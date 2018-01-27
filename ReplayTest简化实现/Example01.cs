
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
    /// 类 名 称：Replay.Example01
    /// 类 功 能：
    /// 主要接口：
    /// </summary>
    public class Example01 : MonoBehaviour, IRePlayEvent
    {


        public GameObject cube, spere, Cylinder;
        /// <summary>
        ///
        /// </summary>
        void Start()
        {
            cube.SetClick((p) => OperateCube());
            spere.SetClick((p) => OperateSpere());
            Cylinder.SetClick((p) => OperateCylinder());
        }

        /// <summary>
        ///
        /// </summary>
        void Update()
        {

        }

        void OperateCube()
        {
           
            ReplayManager.Instance.ExcuteIRePlay(this, "cube", null);
        }

        void OperateSpere()
        {
          
            ReplayManager.Instance.ExcuteIRePlay(this, "sphere", null);
        }
        void OperateCylinder()
        {
            ReplayManager.Instance.ExcuteIRePlay(this, "cylinder", null);
        }
        public void ExcuteMethod(string _replayinfo, object[] _param)
        {
            if(_replayinfo.Equals("cube"))
                cube.transform.Translate(Vector3.one * Random.Range(-1, 1f), Space.Self);
            if(_replayinfo.Equals("sphere"))
                spere.transform.Translate(Vector3.one * Random.Range(-1, 1f), Space.Self);
            if (_replayinfo.Equals("cylinder"))
                Cylinder.transform.localScale=Vector3.one * Random.Range(0, 2f);

        }
    }
}

