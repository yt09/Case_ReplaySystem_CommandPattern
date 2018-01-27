namespace Rainier.Framework.Function
{
    /// <summary>
    /// 命令接口
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 命令解析
        /// </summary>
        /// <param name="commandString">命令字符串</param>
        /// <param name="forward">是否为向前命令</param>
        /// <param name="param">命令参数</param>
        void ExecuteCommand(string commandString, bool forward, params object[] param);
    }
}