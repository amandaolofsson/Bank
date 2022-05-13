using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    enum TransactionType {Insert, Withdraw };
    
    //Transaction is a change of balance in an account
    class Transaction
    {
        string date;
        TransactionType type;
        int amount;

        public Transaction (string date, TransactionType type, int amount)
        {
            this.date = date;
            this.type = type;
            this.amount = amount;
        }

        public string Date
        {
            get
            {
                return date;
            }
        }

        public TransactionType Type
        {
            get
            {
                return type;
            }
        }

        public int Amount
        {
            get
            {
                return amount;
            }
        }
    }
}
