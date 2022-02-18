using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Kryptering
{
    internal class ChatRoomManager
    {
        static List<ChatRoom> chatRooms = new List<ChatRoom>();
        static List<Message> incomgMsg = new List<Message>();


        public void LoadAllChatLogs()
        {
            XmlDocument chatLogs = new XmlDocument();
            chatLogs.Load("C:/Kryptering_Pr/chatLogs.xml");
            List<User> users = new List<User>();
            XmlNodeList extractUserInfo = chatLogs.SelectNodes("users/user");
            foreach (ChatRoom chatRoom in chatRooms)
            {

            }
        }

    }
}
