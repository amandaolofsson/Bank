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

            //Loads in Xml-files
            um.Load();
            am.Load();

            //Port 800 because it needed to be an int, and 800 was free
            tcpListener = new TcpListener(iP, 800);
            tcpListener.Start();

            Console.WriteLine("The bank system 2.X is up and running");

            //This is the main loop where server waits for client to connect
            while (true)
            {
                try
                {
                    Console.WriteLine("Waiting for connection...");
                    Socket socket = tcpListener.AcceptSocket();
                    Console.WriteLine("Connected to: {0}", socket.RemoteEndPoint);

                    //Loop until client disconnect
                    //This is to allow clients to log in and log out of different accounts
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
