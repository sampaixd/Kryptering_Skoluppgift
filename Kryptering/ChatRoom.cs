using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Kryptering
{
    internal class ChatRoom
    {
        static int chatIdCounter = 0;
        int chatId;
        List<User> connectedUsers = new List<User>();
        List<Message> msgLog = new List<Message>();
        XmlDocument xmlChatLog;

        public ChatRoom()
        {
            this.chatId = chatIdCounter++;
            this.xmlChatLog = new XmlDocument();
            if (!File.Exists($"C:/Kryptering_Pr/chagLog{chatId}.xml"))
                CreateXmlFile();

            xmlChatLog.Load($"C:/Kryptering_Pr/chagLog{chatId}.xml");
            msgLog = GetChatLog();
        }
        /* gets all the messages from the chatlog and puts it into a list */
        private List<Message> GetChatLog()
        {
            List<Message> messages = new List<Message>();
            XmlNodeList extractChatLog = xmlChatLog.SelectNodes("messages/message");
            foreach (XmlNode node in extractChatLog)
            {

                string username = node.SelectSingleNode("senderName").InnerText;
                string msgData = node.SelectSingleNode("msgData").InnerText;
                int msgId = Convert.ToInt32(node.SelectSingleNode("msgId").InnerText);
                messages.Add(new Message(username, msgData, msgId));
            }
            return messages;
        }

        private void CreateXmlFile()
        {
            XmlTextWriter writer = new XmlTextWriter($"chatlog{chatId}.xml", Encoding.UTF8);
        }

        public void MsgTransaction(string msg)
        {

            foreach (User user in connectedUsers)
                SocketComm.SendMsg(user.ClientInfo, msg);
        }
        public void ConnectToChat(User connectedUser)
        {
            Console.WriteLine($"connected user {connectedUser.Name} to chatroom {chatId}");
            connectedUsers.Add(connectedUser);
        }        // recieves and sends messages between users in a chat room

        public void DisconnectFromChat(int id)
        {
            foreach(User user in connectedUsers)
            {
                if (user.ID == id)
                {
                    Console.WriteLine($"disconnected {user.Name} from chatroom {chatId}");
                    connectedUsers.Remove(user);
                    return;
                }
            }
            Console.WriteLine("ERROR! could not find user to disconnect with id " + id);
        }
        public string FormatChatRoomInfoToString()
        {
            return $"{chatId}|{connectedUsers.Count()}";
        }
    }
}