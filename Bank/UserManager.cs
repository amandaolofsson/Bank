﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Bank
{
    class UserManager
    {
        List<User> users = new List<User>();

        public User GetByName(string username)
        {
            foreach (User u in users)
            {
                if (u.Name == username)
                {
                    return u;
                }
            }

            return null;
        }

        public void Save()
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = xmlDoc.DocumentElement;
            xmlDoc.InsertBefore(xmlDeclaration, root);

            XmlElement usersElement = xmlDoc.CreateElement("", "users", "");

            foreach (User user in users)
            {
                XmlElement userElement = xmlDoc.CreateElement("", "user", "");

                XmlAttribute userType = xmlDoc.CreateAttribute("", "type", "");
                userType.Value = user.Type.ToString();
                userElement.Attributes.Append(userType);

                XmlAttribute userID = xmlDoc.CreateAttribute("", "id", "");
                userID.Value = user.ID.ToString();
                userElement.Attributes.Append(userID);

                XmlElement nameElement = xmlDoc.CreateElement("", "name", "");
                nameElement.InnerText = user.Name;
                userElement.AppendChild(nameElement);

                if (user.Type == UserType.Customer)
                {
                    Customer customer = user as Customer;
                    XmlElement ssnElement = xmlDoc.CreateElement("", "ssn", "");
                    ssnElement.InnerText = customer.SSN;
                    userElement.AppendChild(ssnElement);
                }
                else if(user.Type == UserType.Admin)
                {
                    //Admins currently doesn't have anything to save
                }

                usersElement.AppendChild(userElement);
            }

            xmlDoc.AppendChild(usersElement);
            xmlDoc.Save("users.xml");
        }

        public void Load()
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load("users.xml");
                users.Clear();
                XmlNodeList userList = xmlDoc.SelectNodes("users/user");

                foreach (XmlNode u in userList)
                {
                    int userID = int.Parse(u.Attributes["id"].Value);
                    UserType type = (UserType)Enum.Parse(typeof(UserType), u.Attributes["type"].Value);
                    string name = u.SelectSingleNode("name").InnerText;

                    User newUser; 

                    if(type == UserType.Customer)
                    {
                        string ssn = u.SelectSingleNode("ssn").InnerText;
                        newUser = new Customer(ssn, name, userID);
                    }
                    else if (type == UserType.Admin)
                    {
                        newUser = new Admin(name, userID);
                    }
                    else
                    {
                        throw new InvalidOperationException("Something is wrong in the User file. Fix now!");
                    }

                    users.Add(newUser);
                }
                
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("***Initializing System***");
                Admin admin = new Admin("Admin", 1);
                users.Add(admin);
                Save();
            }
        }
    }
}
