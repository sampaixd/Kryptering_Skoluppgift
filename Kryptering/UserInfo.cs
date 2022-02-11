using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Kryptering
{
    internal class UserInfo
    {
        List<User> users = new List<User> ();
        XmlDocument userInfo;
        public UserInfo()
        {
            userInfo = new XmlDocument();
            userInfo.Load("C:/Kryptering_Pr/userInfo.xml");
            Console.ReadLine();
            List<User> users = new List<User>();
            XmlNodeList extractUserInfo = userInfo.SelectNodes("users/user");
            foreach (XmlNode node in extractUserInfo)
            {
                string username = node.SelectSingleNode("username").InnerText;
                string encryptedPassword = node.SelectSingleNode("password").InnerText;
                users.Add(new User(username, encryptedPassword));
            }
        }
        public bool checkIfTaken(string username)
        {

            foreach (User user in users)
            {
                if (user.Name == username)
                    return false;
            }
            return true;
        }
        public void addUser(string newUsername, string newPassword)
        {
            XmlElement user = userInfo.CreateElement("user");
            userInfo.AppendChild(user);
            XmlElement username = userInfo.CreateElement("username");
            username.InnerText = newUsername;
            userInfo.AppendChild(username); 

            XmlElement password = userInfo.CreateElement("password");
            password.InnerText = newPassword;
            user.AppendChild(password);
            
        }
    }
}
