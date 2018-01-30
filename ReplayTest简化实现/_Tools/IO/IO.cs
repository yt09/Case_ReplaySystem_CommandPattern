using System.IO;

namespace MyUnitySDK.IO
{
    /// <summary>
    /// IO接口
    /// </summary>
    public abstract class AbsIO
    {
        public abstract void Write(string config, byte[] contents);

        public abstract byte[] Read(string config);
    }

    /// <summary>
    /// IO工具
    /// </summary>
    public class IO
    {
        /// <summary>
        /// 默认为普通text
        /// </summary>
        public static AbsIO ioTool;

        /// <summary>
        /// 存贮
        /// </summary>
        /// <param name="config"></param>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        public static void Write(string config, byte[] content)
        {
            ioTool.Write(config, content);
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static byte[] Read(string config)
        {
            return ioTool.Read(config);
        }
    }
}

namespace MyUnitySDK.IO
{
    /// <summary>
    /// 本地IO工具
    /// </summary>
    public class FileStreamIOTool : AbsIO
    {
        public string path;

        /// <summary>
        /// 本地io生成文件的位置
        /// </summary>
        /// <param name="path"></param>
        public FileStreamIOTool(string path)
        {
            this.path = path;
        }

        public override byte[] Read(string config)
        {
            FileStream stream = new FileStream(path + "/" + config, FileMode.Open);
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Close();
            return bytes;
        }

        public override void Write(string config, byte[] contents)
        {
            FileStream stream = new FileStream(path + "/" + config, FileMode.Create);
            stream.Write(contents, 0, contents.Length);
            stream.Close();
        }
    }
}