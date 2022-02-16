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
     * this class handles all of the socket connections. 
     * This includes sending and recieveving data, 
     * as well as doing some more advanced actions
     * such as sending all of the user names and online
     * statuses to the client
     */
    static class SocketComm
    {
        public static string RecvMsg(Socket client)
        {
            Byte[] msgB = new byte[256];
            int msgSize = client.Receive(msgB);
            string msg = "";
            for (int i = 0; i < msgSize; i++)
                msg += Convert.ToChar(msgB[i]);

            return msg;
        }

        public static void SendMsg(Socket client, string msg)
        {
            Byte[] bSend = System.Text.Encoding.ASCII.GetBytes(msg);
            client.Send(bSend);
        }
        public static void SendOnlineStatus(Socket client, int ownId)
        {
            List<string> allOnlineStatus = new List<string>();
            allOnlineStatus = UserInfo.GetAllOnlineStatus();
            string userCount = Convert.ToString(User.IDCount - 1);
            SendMsg(client, userCount); // tells the client how many times they will recieve data
            int currentUserID = 0;
            foreach (string onlineStatus in allOnlineStatus)
            {
                if (currentUserID != ownId) // does not send the client information about themselves
                    SendMsg(client, onlineStatus);
            }


        }
    }
}
