using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;
using System.Net.Sockets;
using System.Xml;
using System.IO;
using System.Threading;

namespace Kryptering
{
    internal class Program
    {
        static TcpListener tcplistener;
        static void Main(string[] args)
        {
            List<User> users = new List<User>();
            // gets password encryption key
            StreamReader keyFile = new StreamReader("C:/Kryptering_Pr/key.txt");
            int passwordKey = int.Parse(keyFile.ReadLine());
            keyFile.Close();
            
            XmlDocument userInfo = new XmlDocument();
            userInfo.Load("C:/Kryptering_Pr/userInfo.xml");
            Console.ReadLine();


        }

        static void waitForConnection(string[] serverADDR)
        {
            IPAddress myIP = IPAddress.Parse("127.0.0.1");
            tcplistener = new TcpListener(myIP, 8001);
            tcplistener.Start();
            List<Thread> connections = new List<Thread>(); 
            while (true)
            {
                try
                {
                    Console.WriteLine("Waiting for connection...");
                    Socket client = tcplistener.AcceptSocket();
                    connections.Add(new Thread(()=>handleClient(client)));
                    connections[-1].Start(client);
                }

                catch(Exception e)
                {
                    Console.WriteLine("An error occured! " + e.Message);
                }
            }


        }
        static void createUser()
        {

        }

        static void handleClient(Socket client)
        {
            Byte[] msgB = new byte[1024];
            int msgSize = client.Receive(msgB);
            string msg = "";
            for (int i = 0; i < msgSize; i++)
            {
                msg += Convert.ToChar(msgB[i]);
            }
            switch(msg)
            {
                case "create account":
                    break;

                case "login":
                    break;

                case "logout":
                    break;

                case "quit":
                    break;

                case "chat":
                    break;
            }
        }

        static void Login()
        {

        }
        static void msgTransaction(string name, string senderADDR, string recvADDR)
        {

        }

    }
}
