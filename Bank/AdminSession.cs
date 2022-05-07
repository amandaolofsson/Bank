using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class AdminSession : ISession
    {
        User user;

        public AdminSession(User user)
        {
            this.user = user;
        }

        public void Start()
        {
            while (true)
            {
                Console.WriteLine(@"Hello {0}. What would you like to do?
[C] Create customer 
[S] View customers
[D] Delete customers 
[L] Log out", user.Name);
            }
        }
}
}
