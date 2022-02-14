using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Kryptering
{
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
    }
}
