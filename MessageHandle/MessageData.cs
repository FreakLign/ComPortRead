using Encryption;
using FileHandle;
using System.Collections.Generic;

namespace MessageHandle
{
    public class MessageData
    {
        private byte[] _bodyINBytes;
        private MessageType _type;
        /// <summary>
        /// 报文内容
        /// </summary>
        public byte[] BodyInbyte { get { return _bodyINBytes; } }
        /// <summary>
        /// 报文类型
        /// </summary>
        public MessageType MessageType { get { return _type; } }
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
        public MessageData(MessageType type, byte[] data)
        {
            this._bodyINBytes = data;
            this._type = type;
        }
        //public static MessageData CreateMessageData(MessageType messageType, byte[] Data)
        //{

        //}
    }
}
