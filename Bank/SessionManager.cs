using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    interface ISession
    {
        void Start();
    }

    class SessionManager
    {
        public static ISession Create(Socket socket, UserManager um, AccountManager am)
        {
            Communication communication = new Communication(socket);

            communication.Send("Welcome to the bank. Please log in", ServerMessageEnum.Text);
            User user = null;

            while(user == null)
            {
                string username = communication.Prompt("Name: ");

                user = um.GetByName(username);
            }

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
