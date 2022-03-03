using System;
using System.IO;
using System.Collections.Generic;

namespace Kryptering
{
    internal class ChatRoomManager
    {
        static List<ChatRoom> chatRooms;

        static ChatRoomManager()    // loads all the existing chat rooms
        {
            chatRooms = new List<ChatRoom>();
            int existingChatRooms = 0;
            while (File.Exists($"C:/Kryptering_Pr/chatLog{existingChatRooms++}.xml"))   
                chatRooms.Add(new ChatRoom());

        }
        public static void CreateNewChatRoom()
        {
            chatRooms.Add(new ChatRoom());
            Console.WriteLine("Created a new chat room");
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
            if (user.ChatId ==  null)
            {
                throw new ClientNotConnectedToChatRoomException();
            }
            else
            {
                int tempChatId = Convert.ToInt32(user.ChatId);
                chatRooms[tempChatId].MsgTransaction(user, msg);
            }
        }

        public static List<Message> GetChatLog(int roomId)
        {
            return chatRooms[roomId].MsgLog;
        }


    }
}
