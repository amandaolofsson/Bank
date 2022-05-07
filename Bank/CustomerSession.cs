using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class CustomerSession : ISession
    {
        User user;

        public CustomerSession(User user)
        {
            this.user = user;
        }

        public void Start()
        {
            while (true)
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
}
