using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;
    /// <summary>
    /// 命令管理类
    /// </summary>
    public partial class CommandManager : HideMonoSingleton<CommandManager>
    {
        /// <summary>
        /// 是否可控制
        /// </summary>
        public static bool Control { get => Instance.control; set => Instance.control = value; }
        private bool control = true;
        /// <summary>
        /// 现在命令索引
        /// </summary>
        public static int CommandIndex { get => Instance.commandIndex; }
        private int commandIndex = 0;
        /// <summary>
        /// 暂停
        /// </summary>
        private bool pause = false;
        /// <summary>
        /// 现在列表中的MonoBehaviour
        /// </summary>
        public static List<CommandInfo> Commands { get => Instance.commands; }
        private List<CommandInfo> commands = new List<CommandInfo>();
        /// <summary>
        /// 保存命令委托
        /// </summary>
        public static event Action<string> SaveCommandAction;
        /// <summary>
        /// 加载命令委托
        /// </summary>
        public static event Func<string> LoadCommandAction;
        /// <summary>
        /// 回放结束命令
        /// </summary>
        public static event Action UndoCommandComplete;
        /// <summary>
        /// 设置时间缩放方法
        /// </summary>
        public static event Action<float> SetTimeScaleAction = (p) => Time.timeScale = p;
        /// <summary>
        /// 解析时的方法
        /// </summary>
        public static event Action ExecuteCommandAction;

    }

    /// <summary>
    /// 方法
    /// </summary>
    public partial class CommandManager
    {
        /// <summary>
        /// 初始化方法
        /// </summary>
        protected override void Init()
        {
            InitSceneCommands();
            Debug.Log("CommandManager_Init", this);
        }
        /// <summary>
        /// 传送命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="auto">是否为连续命令</param>
        /// <param name="commandString">命令标识符</param>
        /// <param name="param">参数</param>
        public static void ExecuteCommand(ICommand command, bool auto, string commandString, params object[] param)
        {
            if (Control)
            {
                ExecuteCommandAction?.Invoke();
                command.ExecuteCommand(commandString, true, param);
                int[] idT = GameObjectManager.GetComponentID(command as Component);
                if (idT[0] == -1) Debug.LogWarning("CommandManager_No the ObjID", Instance);
                Instance.commands.Add(new CommandInfo(command, idT, auto, commandString, param, Time.frameCount));
                Instance.commandIndex = Instance.commands.Count;
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        public static void SaveCommands()
        {
            SaveCommandAction?.Invoke(Instance.CommandsToString(Instance.commands));
        }
        /// <summary>
        /// 加载
        /// </summary>
        public static void LoadCommands()
        {
            Instance.commands = Instance.StringToCommands(LoadCommandAction?.Invoke());
        }
        /// <summary>
        /// 自动
        /// </summary>
        public static void AutoCommands(bool forward)
        {
            Instance.StopAllCoroutines();
            Instance.pause = false;
            if (forward)
            {
                Instance.StartCoroutine(Instance.IRedoCommand(true));
            }
            else
            {
                Instance.StartCoroutine(Instance.IUndoCommand(true));
            }
        }
        /// <summary>
        /// 暂停
        /// </summary>
        public static void PauseCommand()
        {
            Instance.pause = true;
        }
        /// <summary>
        /// 撤销
        /// </summary>
        public static void UndoCommand()
        {
            if (!Control)
            {
                return;
            }
            Instance.StopAllCoroutines();
            Instance.StartCoroutine(Instance.IUndoCommand(false));
        }
        /// <summary>
        /// 重做
        /// </summary>
        public static void RedoCommand()
        {
            Instance.StopAllCoroutines();
            Instance.StartCoroutine(Instance.IRedoCommand(false));
        }
        /// <summary>
        /// 设置时间缩放
        /// </summary>
        /// <param name="scale"></param>
        public static void SetTimeScale(float scale)
        {
            SetTimeScaleAction?.Invoke(scale);
        }



        #region 内部方法

        /// <summary>
        /// 是否可撤销
        /// </summary>
        /// <returns></returns>
        private bool CheckUndoCommand()
        {
            return commandIndex > 0 && !pause;
        }

        /// <summary>
        /// 撤销一步动作
        /// </summary>
        /// <param name="auto"></param>
        /// <returns></returns>
        private IEnumerator IUndoCommand(bool auto)
        {
            Control = false;
            if (CheckUndoCommand())
            {
                commandIndex--;
                CommandInfo commandLast = commands[commandIndex];
                commandLast.command.ExecuteCommand(commandLast.commandString, false, commandLast.param);
                if ((commandLast.auto || auto) && CheckUndoCommand())
                {
                    int o = commandLast.frame - commands[commandIndex - 1].frame;
                    for (int j = 0; j < o; j++)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    StartCoroutine(IUndoCommand((auto)));
                }
                else
                {
                    Control = true;
                }
            }
        }

        /// <summary>
        /// 是否可重做
        /// </summary>
        /// <returns></returns>
        private bool CheckRedoCommand()
        {
            return commandIndex < commands.Count && !pause;
        }

        /// <summary>
        /// 重做一步动作
        /// </summary>
        /// <param name="auto"></param>
        /// <returns></returns>
        private IEnumerator IRedoCommand(bool auto)
        {
            Control = false;
            if (CheckRedoCommand())
            {
                CommandInfo commandLast = commands[commandIndex];
                commandLast.command.ExecuteCommand(commandLast.commandString, true, commandLast.param);
                commandIndex++;
                if ((commandLast.auto || auto) && CheckRedoCommand())
                {
                    int o = commands[commandIndex].frame - commandLast.frame;
                    for (int j = 0; j < o; j++)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    StartCoroutine(IRedoCommand(auto));
                }
                else
                {
                    Control = true;
                    if (auto)
                    {
                        UndoCommandComplete?.Invoke();
                    }
                }
            }
        }

        /// <summary>
        /// 场景还原
        /// </summary>
        /// <returns></returns>
        private IEnumerator IReturnScene()
        {
            SetTimeScale(100);
            for (int i = 0; i < commands.Count; i++)
            {
                int o = (i == 0) ? commands[i].frame : commands[i].frame - commands[i - 1].frame;
                for (int j = 0; j < o; j++)
                {
                    yield return new WaitForEndOfFrame();
                }
                commands[i].command.ExecuteCommand(commands[i].commandString, true, commands[i].param);
            }
            Control = true;
            commandIndex = commands.Count;
            yield return new WaitForSeconds(2);
            SetTimeScale(1);
        }

        /// <summary>
        /// 命令转化为字符串方法
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public string CommandsToString(List<CommandInfo> commands)
        {
#pragma warning disable IDE0028 // 简化集合初始化
            JObject json = new JObject();
#pragma warning restore IDE0028 // 简化集合初始化
            json.Add("commands", RSerialize.Serialize(commands));
            return json.ToString();
        }

        /// <summary>
        /// 字符串转化为命令方法
        /// </summary>
        /// <param name="commandStrs"></param>
        /// <returns></returns>
        public List<CommandInfo> StringToCommands(string commandStrs)
        {
            JObject json = JObject.Parse(commandStrs);
            List<CommandInfo> commands = RSerialize.Deserialize<List<CommandInfo>>(json["commands"].ToString());
            commands.ForEach(p => p.command = (ICommand)GameObjectManager.Instance[p.commandId]);
            commands.ForEach(p =>
            {
                for (int i = 0; i < p.param.Length; i++)
                {
                    if (p.param[i].GetType() == typeof(long))
                        p.param[i] = (int)(long)p.param[i];
                    else if (p.param[i].GetType() == typeof(double))
                        p.param[i] = (float)(double)p.param[i];
                }
            });
            return commands;
        }

        /// <summary>
        /// 初始化场景中所有ICommand，即sceneCommands
        /// </summary>
        void InitSceneCommands()
        {
        }
        #endregion

    }

    /// <summary>
    /// 内部类
    /// </summary>
    public partial class CommandManager
    {
        /// <summary>
        /// 命令信息
        /// </summary>
        [Serializable]
        public class CommandInfo
        {
            /// <summary>
            /// 命令
            /// </summary>
            [JsonIgnore]
            public ICommand command;
            /// <summary>
            /// 命令在场景命令中的ID
            /// </summary>
            public int[] commandId;
            /// <summary>
            /// 命令是否为连续命令
            /// </summary>
            public bool auto;
            /// <summary>
            /// 命令标识符
            /// </summary>
            public string commandString;
            /// <summary>
            /// 参数
            /// </summary>
            public object[] param;
            /// <summary>
            /// 帧数
            /// </summary>
            public int frame;

            /// <summary>
            /// CommandInfo构造方法
            /// </summary>
            /// <param name="command"></param>
            /// <param name="commandId"></param>
            /// <param name="auto"></param>
            /// <param name="commandString"></param>
            /// <param name="param"></param>
            /// <param name="frame"></param>
            public CommandInfo(ICommand command, int[] commandId, bool auto, string commandString, object[] param, int frame)
            {
                this.command = command;
                this.commandId = commandId;
                this.auto = auto;
                this.commandString = commandString;
                this.param = param;
                this.frame = frame;
            }
        }
    }
