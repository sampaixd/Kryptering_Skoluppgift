using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptering
{
    internal class ChatRoom
    {
        static int chatIdCounter = 0;
        int chatId;
        List<User> connectedUsers = new List<User>();
        List<Message> IncomingMsg = new List<Message>();
        
        public ChatRoom()
        {
            this.chatId = chatIdCounter++;
        }

        public void ConnectToChat(User connectedUser)
        {
            connectedUsers.Add(connectedUser);
        }

        public void CheckForIncomingMsg()
        {

        }



        // recieves and sends messages between users in a chat room
    }
}