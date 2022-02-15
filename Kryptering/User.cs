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

        
        public void ConnectToChat(int connectedchat)
        {
            chatid = connectedchat;
        }
        public string Name { get { return name;} }
        public int Id { get { return id;} }

        public bool OnlineStatús { get{ return online; } }

        private bool Online { set { online = value; } }
        public void Login(Socket client)
        {
            int attempts = 0;
            while (attempts < 3)
            {
                string recvPassword = SocketComm.RecvMsg(client);
                recvPassword = EncryptPassword(recvPassword);
                if (recvPassword != this.password)
                {
                    attempts++;
                    SocketComm.SendMsg(client, "incorrect");
                }

                SocketComm.SendMsg(client, "correct");
                LoggedIn(client);
                attempts = 4;
            }
            if (attempts == 3)
                SocketComm.SendMsg(client, "kicked out");
        }

        private void LoggedIn(Socket client)
        {
            Online = true;
        }



    }
}
