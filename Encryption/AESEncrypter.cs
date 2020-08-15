using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.IO;

namespace Encryption
{
    /// <summary>
    /// AES加密 | AES解密
    /// </summary>
    public class AESEncrypter
    {
        /// <summary>
        /// 密钥向量
        /// </summary>
        private static byte[] _encryptVector = new byte[]
        {
            0x11, 0x23, 0x78, 0x97,
            0x50, 0x11, 0x23, 0x78, 
            0x97, 0x50, 0x11, 0x23,
            0x78, 0x97, 0x50, 0x99 };
        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string Encrypt(byte[] data, string key)
        {
            if (data == null || data.Length == 0 || key == null || key == "") return null;
            SymmetricAlgorithm symmetric = Rijndael.Create();
            symmetric.IV = _encryptVector;
            char[] keySer = new char[16];
            for(int i = 0; i < 16; i++)
            {
                keySer[i] = i < key.Length ? key[i] : (char)i;
            }
            symmetric.Key = Encoding.UTF8.GetBytes(keySer);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(
                memoryStream,
                symmetric.CreateEncryptor(),
                CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
            byte[] resultData = memoryStream.ToArray();
            cryptoStream.Close();
            memoryStream.Close();
            return Convert.ToBase64String(resultData);
        }
        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] Decrypt(string data, string key)
        {
            if (data == null || data.Length == 0 || key == null || key == "") return null;
            byte[] toReadData = Convert.FromBase64String(data);
            byte[] resultData = new byte[toReadData.Length];
            SymmetricAlgorithm symmetric = Rijndael.Create();
            symmetric.IV = _encryptVector;
            char[] keySer = new char[16];
            for (int i = 0; i < 16; i++)
            {
                keySer[i] = i < key.Length ? key[i] : (char)i;
            }
            symmetric.Key = Encoding.UTF8.GetBytes(keySer);
            MemoryStream memoryStream = new MemoryStream(toReadData);
            CryptoStream cryptoStream = new CryptoStream(
                memoryStream,
                symmetric.CreateDecryptor(),
                CryptoStreamMode.Read);
            try
            {
              cryptoStream.Read(resultData, 0, resultData.Length);
            }
            catch
            {
                return null;
            }
            cryptoStream.Close();
            memoryStream.Close();
            List<byte> trulyData = new List<byte>();
            for(int i = 0; i < resultData.Length; i++)
            {
                if (resultData[i] == 0) break;
                trulyData.Add(resultData[i]);
            }
            return trulyData.ToArray();
        }
        /// <summary>
        ///  指定实际裁断长度（排除尾部 0 ）
        /// </summary>
        /// <param name="data">原数据</param>
        /// <param name="key">密钥</param>
        /// <param name="cutLength">截取长度</param>
        /// <returns>解密字节数组</returns>
        public static byte[] Decrypt(string data, string key, int cutLength)
        {
            if (data == null || data.Length == 0 || key == null || key == "") return null;
            byte[] toReadData = Convert.FromBase64String(data);
            byte[] resultData = new byte[toReadData.Length];
            SymmetricAlgorithm symmetric = Rijndael.Create();
            symmetric.IV = _encryptVector;
            char[] keySer = new char[16];
            for (int i = 0; i < 16; i++)
            {
                keySer[i] = i < key.Length ? key[i] : (char)i;
            }
            symmetric.Key = Encoding.UTF8.GetBytes(keySer);
            MemoryStream memoryStream = new MemoryStream(toReadData);
            CryptoStream cryptoStream = new CryptoStream(
                memoryStream,
                symmetric.CreateDecryptor(),
                CryptoStreamMode.Read);
            try
            {
                cryptoStream.Read(resultData, 0, resultData.Length);
            }
            catch
            {
                return null;
            }
            cryptoStream.Close();
            memoryStream.Close();
            List<byte> trulyData = new List<byte>();
            for (int i = 0; i < cutLength; i++)
            {
                trulyData.Add(resultData[i]);
            }
            return trulyData.ToArray();
        }
    }
}
