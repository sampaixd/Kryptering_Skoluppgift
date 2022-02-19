using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

    }
}
