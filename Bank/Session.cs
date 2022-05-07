using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Session
    {
        User user;
        public void Start(UserManager um)
        {
            Console.WriteLine("Welcome to the bank. Please log in");
            
            while(user == null)
            {
                Console.Write("Name: ");
                string username = Console.ReadLine();

                user = um.GetByName(username);
            }

            if(user.Type == UserType.Customer)
            {
                MenyCustomer();
            }
            else if (user.Type == UserType.Admin)
            {
                MenyAdmin();
            }


        }

        public void MenyAdmin()
        {
            Console.WriteLine(@"Hello {0}. What would you like to do?
[C] Create customer 
[S] View customers
[D] Delete customers 
[L] Log out", user.Name);
        }

        public void MenyCustomer()
        {
            Console.WriteLine(@"Hello {0}. What would you like to do?
[C] Create new account
[D] Delete account
[V] View balance
[P] Put money in
[W] Withdraw money
[T] Transfer money
[L] Log out", user.Name);
        }
    }
}
