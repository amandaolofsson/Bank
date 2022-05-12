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
            TcpListener tcpListener;

            IPAddress iP = IPAddress.Parse("127.0.0.1");

            um.Load();
            am.Load();

            tcpListener = new TcpListener(iP, 800);
            tcpListener.Start();

            Console.WriteLine("The bank system 2.X is up and running");

            while (true)
            {
                try
                {
                    Console.WriteLine("Waiting for connection...");
                    Socket socket = tcpListener.AcceptSocket();
                    Console.WriteLine("Connected to: {0}", socket.RemoteEndPoint);

                    while (true)
                    {
                        ISession session = SessionManager.Create(socket, um, am);
                        session.Start();
                    }
                }
                catch (ClientDisconnectException)
                {
                    Console.WriteLine("Client disconnected");
                }
            }
        }
    }
}
