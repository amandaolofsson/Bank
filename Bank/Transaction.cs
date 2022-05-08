using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    enum TransactionType { Insert, Withdraw };

    class Transaction
    {
        string date;
        TransactionType type;
        int amount;
    }
}
