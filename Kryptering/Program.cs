using System;
using System.Net;
using System.Net.Sockets;
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
                    Thread thread = new Thread(() => HandleClient(client));
                    Console.WriteLine($"{client.RemoteEndPoint} connected");
                    thread.Start();
                }

                catch(Exception e)
                {
                    Console.WriteLine("An error occured! " + e.Message);
                }
            }


        }

        static void HandleClient(Socket client)
        {
            bool connected = true;
            while (connected)
            {
                try
                { 
                    string msg = SocketComm.RecvMsg(client);
                    switch (msg)
                    {
                        case "create account":
                            CreateUser(client);
                            break;

                        case "login":
                            Login(client);
                            break;

                        case "quit":
                            Console.WriteLine($"{client.RemoteEndPoint} disconnected");
                            client.Close();
                            connected = false;
                            break;

                    }
                }
                catch (ClientDisconnectedException)
                {
                    Console.WriteLine("CLient connection closed unexpectedly");
                    client.Close();
                    connected = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    client.Close();
                    connected = false;
                }
            }
        }
        static void CreateUser(Socket client)
        {
            string username = "";
            bool settingUserName = true;
            while (settingUserName)
            { 
                username = SocketComm.RecvMsg(client);
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
            UserInfo.AddUser(username, password);

        }

        static void Login(Socket client)
        {
            bool loggingIn = true;
            while (loggingIn)
            {
                string username = SocketComm.RecvMsg(client);
                if (username == "back")
                    break;
                User? user = UserInfo.FindUser(username);
                if (user == null)
                    SocketComm.SendMsg(client, "denied");
                else           
                {              
                    SocketComm.SendMsg(client, "accepted");
                    user.Login(client);
                    loggingIn = false;
                    
                }

            }
        }

        static void MsgTransaction(string name, string senderADDR, string recvADDR)
        {

        }
    }
}
