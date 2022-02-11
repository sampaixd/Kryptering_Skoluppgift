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
        static UserInfo userinfo = new UserInfo();
        static void Main(string[] args)
        {
            // gets password encryption key
            StreamReader keyFile = new StreamReader("C:/Kryptering_Pr/key.txt");
            int passwordKey = int.Parse(keyFile.ReadLine());
            keyFile.Close();
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
                    Thread thread = new Thread(() => handleClient(client));
                    thread.Start();
                }

                catch(Exception e)
                {
                    Console.WriteLine("An error occured! " + e.Message);
                }
            }


        }
        static void createUser(Socket client)
        {
            bool settingUserName = true;
            while (settingUserName)
            { 
                string username = recvMsg(client);
                if (userinfo.checkIfTaken(username))
                {
                    settingUserName = false;
                    sendMsg(client, "accepted");
                }
                else
                    sendMsg(client, "denied");   
            }
            bool settingPassword = true;
            while (settingPassword)
            {

            }



        }

        static void handleClient(Socket client)
        {
            bool connected = true;
            while (connected)
            {
                string msg = recvMsg(client);
                switch(msg)
                {
                    case "create account":
                        break;

                    case "login":
                        break;

                    case "logout":
                        break;

                    case "quit":
                        tcplistener.Stop();
                        connected = false;
                        break;

                    case "chat":
                        break;
                }
            }
        }

        static void Login()
        {

        }
        static void msgTransaction(string name, string senderADDR, string recvADDR)
        {

        }

        static string recvMsg(Socket client)
        {
            Byte[] msgB = new byte[1024];
            int msgSize = client.Receive(msgB);
            string msg = "";
            for (int i = 0; i < msgSize; i++)
                msg += Convert.ToChar(msgB[i]);

            return msg;
        }

        static void sendMsg(Socket client, string msg)
        {
            Byte[] bSend = System.Text.Encoding.ASCII.GetBytes(msg);
            client.Send(bSend);
        }


    }
}
