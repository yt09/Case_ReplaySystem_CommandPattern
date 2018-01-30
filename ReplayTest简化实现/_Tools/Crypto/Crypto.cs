using System.IO;
using System.IO.Compression;

namespace MyUnitySDK.Crypto
{
    /// <summary>
    /// 加密压缩处理接口
    /// </summary>
    public abstract class AbsCrypto
    {
        /// <summary>
        /// 加密,压缩
        /// </summary>
        /// <param name="content">需要加密内容</param>
        /// <returns></returns>
        public abstract byte[] Encrypt(byte[] content);

        /// <summary>
        /// 解密,解压缩
        /// </summary>
        /// <param name="content">需要解密内容</param>
        /// <returns></returns>
        public abstract byte[] Decrypt(byte[] content);
    }

    /// <summary>
    /// 加解密(压缩解压缩)处理工具
    /// </summary>
    public class Crypto
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

namespace MyUnitySDK.Crypto
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
    /// GZip压缩工具(不能用于压缩大于 4 GB )
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

    /// <summary>
    /// 不处理
    /// </summary>
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