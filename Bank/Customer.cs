using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Customer : User
    {
        string ssn;

        public Customer(string ssn, string name, int id) : base(name, UserType.Customer, id)
        {
            this.ssn = ssn;
        }

        public string SSN
        {
            get
            {
                return ssn;
            }
        }
    }
}
