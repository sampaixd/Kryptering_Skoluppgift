using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace Kryptering
{
    internal class User
    {
        private static string keyString = GetKey();

        static int idCount = 0;
        int id;
        int chatid;
        string name;
        string password;
        bool loggedin;

        public User(string name, string password)
        {
            this.id = idCount++;
            this.name = name;
            this.password = password;
            this.loggedin = false;
        }

        
        public void ConnectToChat(int connectedchat)
        {
            chatid = connectedchat;
        }
        public String Name { get { return name;} }
        public int Id { get { return id;} }

        public void Login(Socket client)
        {
            int attempts = 0;
            while (attempts < 3)
            {
                password = SocketComm.RecvMsg(client);

                SocketComm.SendMsg(client, "correct");
            }
        }

        private void LoggedIn()


        private string GetKey()
        {
            // gets password encryption key
            StreamReader keyFile = new StreamReader("C:/Kryptering_Pr/key.txt");
            //byte[] key = new byte[256]; 
            string keyString = keyFile.ReadLine();
            keyFile.Close();
            //key = Encoding.UTF8.GetBytes(keyString);
            return keyString;
        }
    }
}
