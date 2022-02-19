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
        string message;
        string senderName;
        int chatId;

        public Message(string senderName, string message, int msgId)
        {
            this.messageId = msgId;
            this.message = message;
            this.senderName = senderName;
        }

        public string ConvertInfoToString() // used for sending 
        {
            return $"{messageId}|{message}|{senderName}";
        }
    }
}
