using Encryption;
using FileHandle;
using System.Collections.Generic;

namespace MessageHandle
{
    public class MessageData
    {
        public string BodyInStrs { get; set; }
        public byte[] BodyInbyte { get; }
        public MessageType MessageType { get; set; }
        public byte[] CompleteData
        {
            get
            {
                List<byte> completeData = new List<byte>();
                completeData.AddRange(AESEncrypter.Decrypt(MessageType.TypeHead, MessageType.TypeName, MessageType.TypeHeadLength));
                completeData.AddRange(BodyInbyte);
                completeData.AddRange(AESEncrypter.Decrypt(MessageType.TypeFoot, MessageType.TypeName, MessageType.TypeFootLength));
                return completeData.ToArray();
            }
        }
    }
}
