using Encryption;
using HexDataHandle;
using System.Runtime.Serialization;

namespace FileHandle
{

    /// <summary>
    /// 报文类型
    /// </summary>
    [DataContract]
    public class MessageType
    {
        /// <summary>
        /// 报文名字
        /// </summary>
        [DataMember]
        public string TypeName { get; set; }
        /// <summary>
        /// 报文头
        /// </summary>
        [DataMember]
        public string TypeHead { get; set; }
        /// <summary>
        /// 报文头部长度
        /// </summary>
        [DataMember]
        public int TypeHeadLength { get; set; }
        /// <summary>
        /// 报文尾部
        /// </summary>
        [DataMember]
        public string TypeFoot { get; set; }
        /// <summary>
        /// 报文尾部长度
        /// </summary>
        [DataMember]
        public int TypeFootLength { get; set; }
        /// <summary>
        /// 报文内容长度
        /// </summary>
        [DataMember]
        public int MessageLength { get; set; }
        /// <summary>
        /// 报文头部
        /// </summary>
        public byte[] Head
        {
            get
            {
                return AESEncrypter.Decrypt(TypeHead, TypeName,TypeHeadLength);
            }
        }
        /// <summary>
        /// 报文尾部
        /// </summary>
        public byte[] Foot
        {
            get
            {
                return AESEncrypter.Decrypt(TypeFoot, TypeName, TypeFootLength);
            }
        }
    }
}
