using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Admin : User
    {
        public Admin(string name, int id) : base(name, UserType.Admin, id)
        {

        }
    }
}
