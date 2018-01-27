
//includes for System
using Newtonsoft.Json;

/// <summary>
/// 名称空间定义：Replay
/// </summary>
namespace Replay
{
    public interface IRePlayEvent
    {
        void ExcuteMethod(string _replayinfo, object[] _param);
    }
	/// <summary>
    /// 类 名 称：Replay.OperationInfo
	/// 类 功 能：
	/// 主要接口：
    /// </summary>
    [System.Serializable]
	public class OperationInfo  
	{

        //public List<OperationInfo> ActionList = new List<OperationInfo>();
        [JsonIgnore]
        public IRePlayEvent replayEntity;
        /// <summary>
        /// 方法标识
        /// </summary>
        public string replayInfo;
        /// <summary>
        /// 方法参数
        /// </summary>
        public object[] param;
        /// <summary>
        /// 帧数
        /// </summary>
        public int frameCount;
        public int comID;
      
        public OperationInfo()
        {
          
        }
        public OperationInfo(int _framecount,int _comID,string _replayinfo, object[] _param)
        {
            this.frameCount = _framecount;
            this.comID = _comID;
            this.replayInfo = _replayinfo;
            this.param = _param;
        }

	}

    
}

