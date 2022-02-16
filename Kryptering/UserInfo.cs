using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Kryptering
{
    /*
     * this class is keeping track and storing data
     * about all the users, as well as extracting relevant
     * data and changing values in the XML file, such as online status
     */
    internal class UserInfo
    {
        static List<User> users = new List<User> ();
        static XmlDocument userInfo;
        static UserInfo()
        {
            userInfo = new XmlDocument();
            userInfo.Load("C:/Kryptering_Pr/userInfo.xml");
            Console.ReadLine();
            List<User> users = new List<User>();
            XmlNodeList extractUserInfo = userInfo.SelectNodes("users/user");
            foreach (XmlNode node in extractUserInfo)
            {
                int id = Convert.ToInt32(node.SelectSingleNode("userID").InnerText);
                string username = node.SelectSingleNode("username").InnerText;
                string encryptedPassword = node.SelectSingleNode("password").InnerText;
                users.Add(new User(id, username, encryptedPassword));
            }
        }
        public static bool CheckIfTaken(string username)
        {

            foreach (User user in users)
            {
                if (user.Name == username)
                    return false;
            }
            return true;
        }

        public static User FindUser(string name)
        {
            return users.Find(user => user.Name.Contains(name));
        }
        public static void AddUser(int newUserID, string newUsername, string newPassword)
        {
            XmlElement user = userInfo.CreateElement("user");
            userInfo.AppendChild(user);
            XmlElement userID = userInfo.CreateElement("userID");
            userID.InnerText = Convert.ToString(newUserID);
            userInfo.AppendChild(userID);

            XmlElement username = userInfo.CreateElement("username");
            username.InnerText = newUsername;
            userInfo.AppendChild(username); 

            XmlElement password = userInfo.CreateElement("password");
            password.InnerText = newPassword;
            user.AppendChild(password);
            
        }

        public static List<string> GetAllOnlineStatus()
        {
            List<string> userStatus = new List<string>();
            foreach (User user in users)
            {
                userStatus.Add(user.Name +  "|" + user.Online);
            }
            return userStatus;
        }

        public static void ChangeOnlineStatus(int userID, bool newStatus)
        {
            users[userID].Online = newStatus;
        }
    }
}
