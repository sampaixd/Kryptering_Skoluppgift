using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptering
{
    internal class User
    {
        static int idCount = 0;
        int id;
        int chatid;
        string name;
        string password;

        public User(string name, string password)
        {
            this.id = idCount++;
            this.name = name;
            this.password = password;
        }

        
        public void connectToChat(int connectedchat)
        {
            chatid = connectedchat;
        }
        public void sendMsg(string msg)
        {

        }
        public String Name { get; }
        public int Id { get; }

    }
}
