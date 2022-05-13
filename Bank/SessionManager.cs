using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class SessionManager
    {
        public static ISession Create(Socket socket, UserManager um, AccountManager am)
        {
            Communication communication = new Communication(socket);

            communication.Send("Welcome to the bank. Please log in. If no customer has been created, type 'Admin'", ServerMessageEnum.Text);
            User user = null;

            //Repeatedly asking client for name existing in Xml-file
            while(user == null)
            {
                string username = communication.Prompt("Name: ");

                user = um.GetByName(username);
            }

            //Depending on what type user is, client will be presented with different options
            if(user.Type == UserType.Customer)
            {
                return new CustomerSession(communication, user, um, am);
            }
            else if (user.Type == UserType.Admin)
            {
                return new AdminSession(communication, user, um);
            }

            return null;
        }
    }
}
