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
            um.Load();
            while (true)
            {
                Session s = new Session();
                s.Start(um);

                Console.ReadLine();
            }
        }
    }
}
