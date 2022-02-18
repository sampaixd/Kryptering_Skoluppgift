using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptering
{
    internal class Message
    {
        string message;
        string senderName;
        int chatId;

        public Message(string message, string senderName, int chatId)
        {
            this.message = message;
            this.senderName = senderName;
            this.chatId = chatId;
        }

    }
}
