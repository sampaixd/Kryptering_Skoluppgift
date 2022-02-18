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
        int chatid;
        string name;
        string password;
        bool online;

        public User(string name, string password) : base()
        {
            this.id = idCount++;
            this.name = name;
            this.password = password;
            this.online = false;
        }

        // unclear if needed, will still be here for now
        /*public User(int id, string name, string password): base()
        {
            this.id = id;
            this.name = name;
            this.password= password;
            this.online = false;
        }*/

        public void ConnectToChat(int connectedchat)
        {
            chatid = connectedchat;
        }
        public string Name { get { return name; } }
        public int ID { get { return id; } }

        static public int IDCount{ get { return idCount; } }

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
                    LoggedIn(client);
                    attempts = 4;   // ignores the coming if statement
                }
            }
            if (attempts == 3)
                SocketComm.SendMsg(client, "kicked out");
        }

        private void LoggedIn(Socket client)
        {
            
            online = true;
            SocketComm.SendOnlineStatus(client, ID);


            while (online)
            {
                string command = SocketComm.RecvMsg(client);
                switch (command)
                {
                    case "chatroom":
                        break;

                    case "logout":
                        online = false;

                        break;

                    default:
                        break;
                }
            }
        }



    }
}
