using System;
using System.IO;
using System.Runtime.Serialization.Json;
using Microsoft.Win32;
using System.Text;
using System.Windows.Documents;
using System.Collections.Generic;

namespace FileHandle
{
    public class FileHandler
    {
        #region JSON
        /// <summary>
        /// 读取Json（反序列化）
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="readCallback">读取回调</param>
        /// <returns>文件路径不对返回null</returns>
        public static MessageType ReadJson(string path,Func<string,bool> readCallback)
        {
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    using(MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(reader.ReadToEnd())))
                    {
                        DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(MessageType));
                        try
                        {
                            return (MessageType)deserializer.ReadObject(memoryStream);
                        }
                        catch(Exception ex)
                        {
                            readCallback(ex.Message);
                        }
                    }
                }                
            }
            return null;
        }
        /// <summary>
        /// 导出JSON（序列化）
        /// </summary>
        /// <param name="messageType">报文类型</param>
        /// <param name="writeJsonCallback">导出回调</param>
        public static void WriteJson(MessageType messageType,Func<string, string> writeJsonCallback)
        {
            DataContractJsonSerializer typeJson = new DataContractJsonSerializer(typeof(MessageType));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                typeJson.WriteObject(memoryStream, messageType);
                memoryStream.Position = 0;
                if (!Directory.Exists("./MessageType"))
                {
                    Directory.CreateDirectory("./MessageType");
                }
                if (!File.Exists(messageType.TypeName))
                {
                    using (StreamWriter streamWriter = new StreamWriter(messageType.TypeName))
                    {
                        streamWriter.Write(Encoding.UTF8.GetString(memoryStream.ToArray()));
                        writeJsonCallback("成功导出！");
                    }
                }
                else
                {
                    writeJsonCallback("该类型已存在");
                }
            }
        }
        #endregion

        #region 显示数据输出
        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayData"></param>
        public static void ExportData(Func<int,string[]> displayData)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "文本文档|*.txt|逗号分隔符|*.csv";
            saveFileDialog.FileName = string.Format("测试数据yyyy_MM_dd_HH_mm_ss");
            if ((bool)saveFileDialog.ShowDialog())
            {
                string[] dataList = displayData(saveFileDialog.FilterIndex);
                if (dataList == null || dataList.Length <= 0) return;
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                {
                    for (int i = 0; i < dataList.Length; i++)
                    {
                        if (dataList.Length > i)
                        {
                            writer.WriteLine(dataList[i]);
                        }
                    }
                }
            }
        }
        #endregion

        #region 数据读入
        public static void GetDataFromFile(string path)
        {

        }
        #endregion
    }
}
