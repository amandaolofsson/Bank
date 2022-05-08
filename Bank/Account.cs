using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Account
    {
        int balance;
        int id;
        int customerId;



        public Account(int balance, int id, int customerId)
        {
            this.balance = balance;
            this.id = id;
            this.customerId = customerId;
        }

        public int Balance
        {
            get
            {
                return balance;
            }

            set
            {
                balance = value;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public int CustomerId
        {
            get
            {
                return customerId;
            }
        }
    }
}
