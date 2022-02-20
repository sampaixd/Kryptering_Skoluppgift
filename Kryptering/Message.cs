using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptering
{
    internal class Message
    {
        int messageId;
        string messageContent;
        string senderName;

        public Message(string senderName, string messageContent, int messageId)
        {
            this.messageId = messageId;
            this.messageContent = messageContent;
            this.senderName = senderName;
        }
        public int MessageId { get { return messageId; } }
        public string MessageContent { get { return messageContent; } }
        public string SenderName { get { return senderName; } }

        public string ConvertInfoToString() // used for sending data via socket
        {
            return $"{messageId}|{messageContent}|{senderName}";
        }
    }
}
