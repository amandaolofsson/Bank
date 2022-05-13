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

    abstract class SessionBase : ISession
    {
        protected Communication communication;

        public SessionBase(Communication communication)
        {
            this.communication = communication;
        }

        public abstract void Start();
    }
}
