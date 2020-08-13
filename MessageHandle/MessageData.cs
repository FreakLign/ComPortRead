using Encryption;
using FileHandle;
using System.Collections.Generic;

namespace MessageHandle
{
    public class MessageData
    {
        /// <summary>
        /// 报文内容（string）
        /// </summary>
        public string BodyInStrs { get; set; }
        /// <summary>
        /// 报文内容
        /// </summary>
        public byte[] BodyInbyte { get; }
        /// <summary>
        /// 报文类型
        /// </summary>
        public MessageType MessageType { get; set; }
        /// <summary>
        /// 完整字节数组
        /// </summary>
        public byte[] CompleteData
        {
            get
            {
                List<byte> completeData = new List<byte>();
                completeData.AddRange(MessageType.Head);
                completeData.AddRange(BodyInbyte);
                completeData.AddRange(MessageType.Foot);
                return completeData.ToArray();
            }
        }
        public MessageData(byte[] origionData)
        {

        }
    }
}
