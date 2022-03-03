using System;

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
        public Message(string formattedMessage)
        {
            char[] newMessageChar = formattedMessage.ToCharArray();
            this.messageId = GetMsgId(newMessageChar);
            int msgIdLength = Convert.ToString(messageId).Length + 1;   // used for upcoming methods
            this.messageContent = GetMsgContent(newMessageChar, msgIdLength);
            this.senderName = GetMsgSender(newMessageChar, msgIdLength + messageContent.Length + 1);
            Console.WriteLine($"ID: {messageId}, content: {messageContent}, sender: {senderName}");
        }


        public int MessageId { get { return messageId; } }
        public string MessageContent { get { return messageContent; } }
        public string SenderName { get { return senderName; } }

        public int GetMsgId(char[] formattedMessageChar)
        {
            string msgIdString = "";
            int currentChar = 0;
            while (formattedMessageChar[currentChar] != '|')
                msgIdString += formattedMessageChar[currentChar++];
            return Convert.ToInt32(msgIdString);
        }

        public string GetMsgContent(char[] formattedMessageChar, int charStartPosition)
        {
            string msgContent = "";
            int currentChar = charStartPosition;
            while (formattedMessageChar[currentChar] != '|')
                msgContent += formattedMessageChar[currentChar++];
            return msgContent;
        }

        public string GetMsgSender(char[] formattedMessageChar, int charStartPosition)
        {
            string msgSender = "";
            for (int i = charStartPosition; i < formattedMessageChar.Length; i++)
                msgSender += formattedMessageChar[i];
            return msgSender;
        }

        public string ConvertInfoToString() // used for sending data via socket
        {
            return $"{messageId}|{messageContent}|{senderName}";
        }
    }
}
