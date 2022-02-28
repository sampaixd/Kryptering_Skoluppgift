using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net.Sockets;

namespace Kryptering
{
    internal class ChatRoomManager
    {
        static List<ChatRoom> chatRooms = new List<ChatRoom>();

        static ChatRoomManager()    // loads all the existing chat rooms
        {
            int existingChatRooms = 0;
            while (File.Exists($"C:/Kryptering_Pr/chatLog{existingChatRooms++}.xml"))   
                chatRooms.Add(new ChatRoom());

        }
        public static void CreateNewChatRoom()
        {
            chatRooms.Add(new ChatRoom());
        }

        public static List<string> FormatChatRoomsToString()
        {
            List<string> chatRoomInfo = new List<string>();
            foreach (ChatRoom chatRoom in chatRooms)
                chatRoomInfo.Add(chatRoom.FormatChatRoomInfoToString());
            return chatRoomInfo;
        }

        public static int? JoinChatRoom(User user, int chatRoomId)
        {
            return chatRooms[chatRoomId].ConnectToChat(user);
        }

        public static void LeaveChatRoom(User user, int chatRoomId)
        {
            chatRooms[chatRoomId].DisconnectFromChat(user);
        }

        public static void SendMsgToChatRoom(User user, string msg)
        {
            try
            { 
                if (user.ChatId ==  null)
                {
                    throw new Exception("User is not connected to a chat room");
                }
                else
                {
                    int tempChatId = Convert.ToInt32(user.ChatId);
                    chatRooms[tempChatId].MsgTransaction(user, msg);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


    }
}
