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
        int msgIdCounter;
        List<User> connectedUsers = new List<User>();
        List<Message> msgLog = new List<Message>();
        XmlDocument xmlChatLog;
        XmlTextWriter xmlChatLogWriter;
        string path;


        public ChatRoom()
        {
            this.msgIdCounter = 0;
            this.chatId = chatIdCounter++;
            this.path = $"C:/Kryptering_Pr/chatLog{chatId}.xml";    // put in place for easier testing
            this.xmlChatLog = new XmlDocument();
            this.xmlChatLogWriter = new XmlTextWriter(path, Encoding.UTF8);

            if (!File.Exists(path))
                CreateXmlFile();

            xmlChatLog.Load(path);
            msgLog = GetChatLog();
        }
        /* gets all the messages from the chatlog and puts it into a list */
        private List<Message> GetChatLog()
        {
            List<Message> messages = new List<Message>();
            XmlNodeList extractChatLog = xmlChatLog.SelectNodes(path);
            foreach (XmlNode node in extractChatLog)
            {

                string username = node.SelectSingleNode("senderName").InnerText;
                string msgData = node.SelectSingleNode("msgData").InnerText;
                int msgId = Convert.ToInt32(node.SelectSingleNode("msgId").InnerText);
                messages.Add(new Message(username, msgData, msgIdCounter++));
            }
            return messages;
        }

        private void CreateXmlFile()
        {
            XmlDeclaration xmldeclaration = xmlChatLog.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlChatLog.AppendChild(xmldeclaration);
            xmlChatLog.Save(path);
        }

        public void MsgTransaction(User sender, string msg)
        {
            msgLog.Add(new Message(sender.Name, msg, msgIdCounter++));
            AddChatMsgToXml(msgLog.Last());
            foreach (User user in connectedUsers)
                if (user != sender)     // makes sure that the sender doesnt get their own message sent back
                    SocketComm.SendMsg(user.ClientInfo, msgLog.Last().ConvertInfoToString());
        }
        public int? ConnectToChat(User connectedUser)
        {
            try
            {
                connectedUsers.Add(connectedUser);
                Console.WriteLine($"connected user {connectedUser.Name} to chatroom {chatId}");
                return chatId;
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR! Could not connect user {connectedUser.Name} to chatroom {chatId}");
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public void DisconnectFromChat(User userToDisconnect)
        {
            foreach(User user in connectedUsers)
            {
                if (user == userToDisconnect)
                {
                    Console.WriteLine($"disconnected {user.Name} from chatroom {chatId}");
                    connectedUsers.Remove(user);
                    return;
                }
            }
            Console.WriteLine($"ERROR! could not find user to disconnect with id {userToDisconnect.ID}");
        }

        public string FormatChatRoomInfoToString()  // used for sending data to client
        {
            return $"{chatId}|{connectedUsers.Count()}";
        }

        public void AddChatMsgToXml(Message newMessage)
        {
            XmlElement messsage = xmlChatLog.CreateElement("message");
            xmlChatLog.AppendChild(messsage);

            XmlElement senderName = xmlChatLog.CreateElement("senderName");
            messsage.AppendChild(senderName);

            XmlElement msgContent = xmlChatLog.CreateElement("msgContent");
            msgContent.InnerText = newMessage.MessageContent;
            messsage.AppendChild(msgContent);

            XmlElement msgId = xmlChatLog.CreateElement("msgId");
            msgId.InnerText = Convert.ToString(newMessage.MessageId);
            messsage.AppendChild(msgId);

            xmlChatLog.Save(path);
        }
    }
}