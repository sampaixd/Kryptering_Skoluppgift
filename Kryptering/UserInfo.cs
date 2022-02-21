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
            List<User> users = new List<User>();
            XmlNodeList extractUserInfo = userInfo.SelectNodes("media/user");
            foreach (XmlNode node in extractUserInfo)
            {
                string username = node.SelectSingleNode("username").InnerText;
                string encryptedPassword = node.SelectSingleNode("password").InnerText;
                
                    users.Add(new User(username, encryptedPassword));
            }
        }
        public static bool CheckIfNameIsTaken(string username)
        {

            foreach (User user in users)
            {
                if (user.Name == username)
                    return true;
            }
            return false;
        }

        public static User FindUser(string name)
        {
            return users.Find(user => user.Name.Contains(name));
        }
        public static void AddUser(string newUsername, string newPassword)
        {
            users.Add(new User(newUsername, newPassword));
            int newUserID = users.Last().ID;

            XmlElement userXml = userInfo.CreateElement("user");
            userInfo.AppendChild(userXml);

            XmlElement user = userInfo.CreateElement("user");
            userXml.AppendChild(user);

            XmlElement userID = userInfo.CreateElement("userID");
            userID.InnerText = Convert.ToString(newUserID);
            userXml.AppendChild(userID);

            XmlElement username = userInfo.CreateElement("username");
            username.InnerText = newUsername;
            userXml.AppendChild(username); 

            XmlElement password = userInfo.CreateElement("password");
            password.InnerText = newPassword;
            userXml.AppendChild(password);
            userInfo.Save("C:/Kryptering_Pr/userInfo.xml");
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
