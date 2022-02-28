using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
        static string path;
        static List<User> users;
        static XmlDocument userInfo;
        static UserInfo()
        {
            userInfo = new XmlDocument();
            path = "C:/Kryptering_Pr/userInfo.xml";
            if (!File.Exists(path))
                CreateXmlFile();

            userInfo.Load(path);
            users = ExtractUserInfo();            
        }

        static void CreateXmlFile()
        {
            XmlDeclaration xmldeclaration = userInfo.CreateXmlDeclaration("1.0", "utf-8", null);
            userInfo.AppendChild(xmldeclaration);
            XmlElement messages = userInfo.CreateElement("messages");
            messages.AppendChild(messages);
            userInfo.Save(path);
        }

        static List<User> ExtractUserInfo()
        {
            List<User> tempUsers = new List<User>();
            XmlNodeList extractUserInfo = userInfo.SelectNodes("users/user");
            foreach (XmlNode node in extractUserInfo)
            {
                Console.WriteLine("found user");
                string username = node.SelectSingleNode("username").InnerText;
                string encryptedPassword = node.SelectSingleNode("password").InnerText;

                tempUsers.Add(new User(username, encryptedPassword));
            }
            Console.WriteLine(tempUsers.Count);
            return tempUsers;
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
            XmlElement usersXml = userInfo.DocumentElement;

            XmlElement userXml = userInfo.CreateElement("user");
            usersXml.AppendChild(userXml);

            XmlElement userID = userInfo.CreateElement("userID");
            userID.InnerText = Convert.ToString(newUserID);
            userXml.AppendChild(userID);

            XmlElement username = userInfo.CreateElement("username");
            username.InnerText = newUsername;
            userXml.AppendChild(username); 

            XmlElement password = userInfo.CreateElement("password");
            password.InnerText = newPassword;
            userXml.AppendChild(password);
            userInfo.Save(path);
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
