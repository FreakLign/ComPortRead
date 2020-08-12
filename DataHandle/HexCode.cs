using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataHandle
{
    public class HexCode
    {
        /// <summary>
        /// 十六进制数据字节数组转字符串（Hex To String format.）
        /// </summary>
        /// <param name="hexData"></param>
        /// <returns></returns>
        public static string GetString(byte[] hexData)
        {
            if (hexData == null) return null;
            if (hexData.Length == 0) return "";
            string rebackData = "";
            foreach (byte hex in hexData)
            {
                rebackData += string.Format("{0:X2} ", hex);
            }
            return rebackData;
        }
        /// <summary>
        /// 十六进制数据字节数组转字符串数组（Hex To Strings format.）
        /// </summary>
        /// <param name="hexData"></param>
        /// <returns></returns>
        public static string[] GetStrings(byte[] hexData)
        {
            if (hexData == null || hexData.Length == 0) return null;
            List<string> rebackData = new List<string>();
            foreach (byte hex in hexData)
            {
                rebackData.Add(string.Format("{0:X2} ", hex));
            }
            return rebackData.ToArray();
        }
        /// <summary>
        /// 字符串转十六进制字节数据数组（String to Hex）
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public static byte[] GetHex(string strData)
        {
            if (strData == null || strData == "") return null;
            MatchCollection matchs = Regex.Matches(strData, @"[0-f][0-f]");
            if(matchs != null && matchs.Count > 0)
            {
                List<byte> Hexs = new List<byte>();
                foreach(Match match in matchs)
                {
                    Hexs.Add((byte)Convert.ToUInt32(match.Value, 16));
                }
                return Hexs.ToArray();
            }
            return null;
        }
    }
}
