using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;



namespace Kryptering
{
    internal class User : AesEncryption
    {
        static int idCount = 0;
        int id;
        int? chatId;
        string name;
        string password;
        bool online;
        Socket? clientInfo;

        public User(string name, string password) : base()
        {
            this.id = idCount++;
            this.name = name;
            this.password = password;
            this.online = false;
        }
        public string Name { get { return name; } }
        public int ID { get { return id; } }
        static public int IDCount { get { return idCount; } }
        public Socket ClientInfo { get { return clientInfo; } }
        public bool Online { get{ return online; } set { online = value; } }
        public int? ChatId { get { return chatId; } }
        

        public void Login(Socket client)
        {
            int passwordAttempts = 0;
            while (passwordAttempts < 3)
            {
                string recvPassword = SocketComm.RecvMsg(client);
                recvPassword = EncryptPassword(recvPassword);
                if (recvPassword != password)
                {
                    passwordAttempts++;
                    SocketComm.SendMsg(client, "denied");
                }
                else
                { 
                    SocketComm.SendMsg(client, "accepted");
                    clientInfo = client;
                    LoggedIn();
                    clientInfo = null;    // will automatically reset client info when exiting the "LoggedIn" method
                    passwordAttempts = 3;
                }
            }
        }

        private void LoggedIn()
        {
            
            online = true;
            SocketComm.SendOnlineStatus(clientInfo, ID);
            SocketComm.SendAllChatRoomInfo(clientInfo);

            while (online)
            {
                string command = SocketComm.RecvMsg(clientInfo);
                switch (command)
                {
                    case "chatroom":
                        SelectChatRoom();
                        break;

                    case "logout":
                        online = false;

                        break;

                    default:
                        break;
                }
            }
        }

        private void SelectChatRoom()
        {
            bool selectingChatRoom = true;
            while (selectingChatRoom)
            { 
                int selectedChatRoom = Convert.ToInt32(SocketComm.RecvMsg(clientInfo));
                if (selectedChatRoom == -2)
                    return;
                else if (selectedChatRoom == -1)
                { 
                    ChatRoomManager.CreateNewChatRoom();
                    continue;
                }

                chatId = ChatRoomManager.JoinChatRoom(this, selectedChatRoom);
                if (chatId == null)
                    SocketComm.SendMsg(clientInfo, "denied");
                else
                    SocketComm.SendMsg(clientInfo, "accepted");
            }
        }

        private void ListenForData()
        {
            string incomingData = SocketComm.RecvMsg(clientInfo);
            if (incomingData != "evael/")   // encrypted command for leaving chatroom
            {
                
                ChatRoomManager.SendMsgToChatRoom(this, incomingData);
            }
            else
            {
                AttemptToDisconnectFromChat();
            }
        }
        private void AttemptToDisconnectFromChat()
        {
            try
            {
                if (chatId == null)
                {
                    throw new Exception("Attempted disconnecting when not connected to a chat room");
                }
                else
                {
                    int tempChatId = Convert.ToInt32(chatId);   // program would not run if the sent int was nullable
                    ChatRoomManager.LeaveChatRoom(this, tempChatId);
                    chatId = null;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
