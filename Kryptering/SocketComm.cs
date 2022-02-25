using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Kryptering
{
    /* 
     * This class handles all of the socket connections. 
     * This includes sending and recieveving data, 
     * as well as doing some more advanced actions
     * such as sending all of the user names and online
     * statuses to the client
     */
    static class SocketComm
    {
        public static string RecvMsg(Socket client)
        {
            try
            { 
            byte[] msgB = new byte[256];
            int msgSize = client.Receive(msgB);
            string msg = "";
            for (int i = 0; i < msgSize; i++)
                msg += Convert.ToChar(msgB[i]);
            Console.WriteLine($"{client.RemoteEndPoint} says {msg}");

            return msg;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                client.Close();
                return "";
            }
        }

        public static void SendMsg(Socket client, string msg)
        {
            byte[] bSend = Encoding.UTF8.GetBytes(msg);
            client.Send(bSend);
        }
        public static void SendOnlineStatus(Socket client, int ownId)
        {
            List<string> allOnlineStatus = new List<string>();
            allOnlineStatus = UserInfo.GetAllOnlineStatus();
            int currentUserID = 0;
            foreach (string onlineStatus in allOnlineStatus)
            {
                if (currentUserID != ownId) // does not send the client information about themselves
                    SendMsg(client, onlineStatus);
            }
            SendMsg(client, "end");
        }

        public static void SendChatLogs(Socket client, List<Message> chatLog)
        {
            foreach (Message message in chatLog)
            {
                SendMsg(client, message.ConvertInfoToString());
            }
            SendMsg(client, "end");
        }

        public static void SendAllChatRoomInfo(Socket client)
        {
            List<string> chatRoomInfo = ChatRoomManager.FormatChatRoomsToString();
            foreach (string roomInfo in chatRoomInfo)
                SendMsg(client, roomInfo);
            SendMsg(client, "end");
        }
    }
}
