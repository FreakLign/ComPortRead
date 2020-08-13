using Encryption;
using HexDataHandle;
using System.Collections.Generic;
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
        /// 报文头部(未加入序列化)
        /// </summary>
        public byte[] Head
        {
            get
            {
                return AESEncrypter.Decrypt(TypeHead, TypeName,TypeHeadLength);
            }
        }
        /// <summary>
        /// 报文尾部(未加入序列化)
        /// </summary>
        public byte[] Foot
        {
            get
            {
                return AESEncrypter.Decrypt(TypeFoot, TypeName, TypeFootLength);
            }
        }
        /// <summary>
        /// 判断数据是否是本类型
        /// </summary>
        /// <param name="origionData"></param>
        /// <returns></returns>
        public bool GetCheck(byte[] origionData)
        {
            if (origionData != null) return false;
            if (origionData.Length != this.MessageLength + this.TypeFootLength + this.TypeHeadLength) return false;
            for(int i = 0; i < TypeHeadLength; i++)
            {
                if (origionData[i] != Head[i]) return false;
            }
            for (int i = 0; i < this.TypeFootLength; i++) 
            {
                if (origionData[i + this.TypeHeadLength + this.MessageLength] != Foot[i]) 
                {
                    return false;
                }
            }
            return true;
        }
    }
}
