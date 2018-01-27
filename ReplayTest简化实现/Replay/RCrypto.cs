
using System.IO;
using System.IO.Compression;

namespace Replay
{
    /// <summary>
    /// 加密处理接口
    /// </summary>
    public abstract class AbsCrypto
    {
        public abstract byte[] Encrypt(byte[] content);
        public abstract byte[] Decrypt(byte[] content);
    }

    /// <summary>
    /// 加密密处理工具
    /// </summary>
    public class RCrypto
    {
        public static AbsCrypto cryptoTool;

        public static byte[] Encrypt(byte[] content)
        {
            return cryptoTool.Encrypt(content);
        }

        public static byte[] Decrypt(byte[] content)
        {
            return cryptoTool.Decrypt(content);
        }
    }


}

namespace Replay
{
    /// <summary>
    /// 简单加解密工具
    /// </summary>
    public class SimpleCryptoTool : AbsCrypto
    {
        public override byte[] Decrypt(byte[] content)
        {
            for (int i = 0; i < content.Length; i++)
            {
                content[i] = (byte)~content[i];
            }
            return content;
        }

        public override byte[] Encrypt(byte[] content)
        {
            for (int i = 0; i < content.Length; i++)
            {
                content[i] = (byte)~content[i];
            }
            return content;
        }
    }

    /// <summary>
    /// GZip加密工具
    /// </summary>
    public class GZipCryptoTool : AbsCrypto
    {
        public override byte[] Decrypt(byte[] content)
        {
            using (MemoryStream inputStream = new MemoryStream(content))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    using (GZipStream zipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                    {
                        InternalCopyTo(zipStream, outStream, 4096);
                        zipStream.Close();
                        return outStream.ToArray();
                    }
                }

            }
        }

        public override byte[] Encrypt(byte[] content)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                using (GZipStream zipStream = new GZipStream(outStream, CompressionMode.Compress, true))
                {
                    zipStream.Write(content, 0, content.Length);
                    zipStream.Close(); //很重要，必须关闭，否则无法正确解压
                    return outStream.ToArray();
                }
            }
        }

        private void InternalCopyTo(Stream from, Stream destination, int bufferSize)
        {
            byte[] array = new byte[bufferSize];
            int count;
            while ((count = from.Read(array, 0, array.Length)) != 0)
            {
                destination.Write(array, 0, count);
            }
        }
    }

    public class NoCryptoTool : AbsCrypto
    {
        public override byte[] Decrypt(byte[] content)
        {
            return content;
        }

        public override byte[] Encrypt(byte[] content)
        {
            return content;
        }
    }

}