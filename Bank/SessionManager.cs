using System;
using System.Collections.Generic;
using System.Linq;
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
        public static ISession Create(UserManager um)
        {
            Console.WriteLine("Welcome to the bank. Please log in");
            User user = null;

            while(user == null)
            {
                string username = UserInput.Prompt("Name: ");

                user = um.GetByName(username);
            }

            if(user.Type == UserType.Customer)
            {
                return new CustomerSession(user);
            }
            else if (user.Type == UserType.Admin)
            {
                return new AdminSession(user);
            }

            return null;
        }
    }
}
