using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Bank
{
    class Program
    {
        static void Main(string[] args)
        {
            UserManager um = new UserManager();
            AccountManager am = new AccountManager();
            um.Load();
            am.Load();
            while (true)
            {
                ISession session = SessionManager.Create(um, am);
                session.Start();
            }
        }
    }
}
