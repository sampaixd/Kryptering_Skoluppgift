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
        int chatId;
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


        

        public void Login(Socket client)
        {
            int attempts = 0;
            while (attempts < 3)
            {
                string recvPassword = SocketComm.RecvMsg(client);
                recvPassword = EncryptPassword(recvPassword);
                if (recvPassword != password)
                {
                    attempts++;
                    SocketComm.SendMsg(client, "incorrect");
                }
                else
                { 
                    SocketComm.SendMsg(client, "correct");
                    clientInfo = client;
                    LoggedIn();
                    clientInfo = null;    // will automatically reset client info when exiting the "LoggedIn" method
                    attempts = 4;   // ignores the coming if statement
                }
            }
            if (attempts == 3)
                SocketComm.SendMsg(client, "kicked out");
        }

        private void LoggedIn()
        {
            
            online = true;
            SocketComm.SendOnlineStatus(clientInfo, ID);


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
            SocketComm.SendChatRoomInfo(clientInfo, ChatRoomManager.FormatChatRoomsToString()); // sends chatroom info to user
            string selectedChatRoomString = SocketComm.RecvMsg(clientInfo);
            if (selectedChatRoomString != "back")
            {
                int selectedChatRoom = int.Parse(selectedChatRoomString);
                chatId = selectedChatRoom;
                ChatRoomManager.
            }
        }



    }
}
