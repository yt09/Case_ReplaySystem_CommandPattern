
//includes for Unity
using UnityEngine;

//includes for System
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Collections;
/// <summary>
/// 名称空间定义：Replay
/// </summary>
namespace Replay
{

    /// <summary>
    /// 类 名 称：Replay.ReplayManager
    /// 类 功 能：
    /// 主要接口：
    /// </summary>
    public  class ReplayManager : MonoBehaviour
    {
        private static ReplayManager instance;
        public static ReplayManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ReplayManager();
                return instance;
            }
            set
            {

                instance = value;
            }
        }
        public List<OperationInfo> operationInfo = new List<OperationInfo>();
        private int playStep=0;
        private bool replay;
        public bool Replay
        {
            get
            { return replay; }
            set
            {
                if (value == replay)
                    return;
                if (value)
                {
                    StartCoroutine(ReplayEvent());
                   
                }
                else
                {
                    StopCoroutine(ReplayEvent());
                }

            }
        }
        private void Awake()
        {

            instance = this;
        }
        private void Start()
        {
            Init();
        }
        private void Init()
        {
            //序列化工具
            RSerialize.serializeTool = new CommonSerializeTool();
            //文件操作
            RIO.ioTool = new FileStreamIOTool(Application.dataPath + "/Resources");
            RCrypto.cryptoTool = new NoCryptoTool();
            UnityObjectManager.RefreshGos();
            UnityObjectManager.RefreshCos();
        }
        /// <summary>
        /// 统一调用具体实例化的接口方法
        /// </summary>
        /// <param name="_replayentity"></param>
        /// <param name="_replayinfo"></param>
        /// <param name="_param"></param>
        public void ExcuteIRePlay(IRePlayEvent _replayentity, string _replayinfo, object[] _param)
        {
            _replayentity.ExcuteMethod(_replayinfo, _param);
            operationInfo.Add(new OperationInfo (Time.frameCount,UnityObjectManager.GetComponentID(_replayentity as Component), _replayinfo,_param));

        }
        /// <summary>
        /// 保存数据
        /// </summary>
        public void SaveReplayData()
        {

            JObject jobect = new JObject();
            jobect.Add("ReplayInfo", RSerialize.Serialize(operationInfo));
            RIO.Write("ReplayData.txt", System.Text.Encoding.UTF8.GetBytes(jobect.ToString()));
            Debug.Log("Save Success");
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        public void LoadReplayData()
        {
            operationInfo.Clear();
       
            string data = System.Text.Encoding.UTF8.GetString(RIO.Read("ReplayData.txt"));
            JObject jobect = JObject.Parse(data);
            operationInfo =  RSerialize.Deserialize<List<OperationInfo>>(jobect["ReplayInfo"].ToString());
         
            foreach (var item in operationInfo)
            {
                item.replayEntity = (IRePlayEvent)UnityObjectManager.GetComponentByID(item.comID);
            }
            Debug.Log("Load Success");
        }
        /// <summary>
        /// 回放数据
        /// </summary>
        public void ReplayRecord(int index )
        {
            operationInfo[index].replayEntity.ExcuteMethod(operationInfo[index].replayInfo, operationInfo[index].param);

        }
      
        IEnumerator ReplayEvent()
        {
            if (playStep < operationInfo.Count)
            {

                ReplayRecord(playStep);
                playStep++;
                if (playStep == operationInfo.Count)
                {
                    Debug.Log("回放结束");
                    StopCoroutine(ReplayEvent());
                }
                else
                {
                    int frameOff = operationInfo[playStep].frameCount - operationInfo[playStep - 1].frameCount;
                    for (int i = 0; i < frameOff; i++)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    StartCoroutine(ReplayEvent());
                }
            }
        }
      

    }
}

