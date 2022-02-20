﻿using System;
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
        static PasswordEncryptor Encryptor = new PasswordEncryptor();

        static void Main(string[] args)
        {
            WaitForConnection();
        }

        static void WaitForConnection()
        {
            IPAddress myIP = IPAddress.Parse("127.0.0.1");
            tcplistener = new TcpListener(myIP, 8001);
            tcplistener.Start();
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
        static void CreateUser(Socket client)
        {
            bool settingUserName = true;
            while (settingUserName)
            { 
                string username = SocketComm.RecvMsg(client);
                if (username == "exit")
                    break;

                if (!UserInfo.CheckIfNameIsTaken(username))
                {
                    settingUserName = false;
                    SocketComm.SendMsg(client, "accepted");
                }
                else
                    SocketComm.SendMsg(client, "denied");   
            }
            string password = SocketComm.RecvMsg(client);
            password = Encryptor.PublicEncryptPassword(password);


        }

        static void handleClient(Socket client)
        {
            bool connected = true;
            while (connected)
            {
                string msg = SocketComm.RecvMsg(client);
                switch(msg)
                {
                    case "create account":
                        CreateUser(client);
                        break;

                    case "login":
                        Login(client);
                        break;

                    case "quit":
                        tcplistener.Stop();
                        connected = false;
                        break;

                }
            }
        }

        static void Login(Socket client)
        {
            bool loggingin = true;
            while (loggingin)
            {
                string username = SocketComm.RecvMsg(client);
                if (username == "back")
                    break;
                User? user = UserInfo.FindUser(username);
                if (user == null)
                    SocketComm.SendMsg(client, "user not found");
                else           
                {              
                    SocketComm.SendMsg(client, "user found");
                    user.Login(client);
                    
                }

            }
        }

        static void MsgTransaction(string name, string senderADDR, string recvADDR)
        {

        }
    }
}
