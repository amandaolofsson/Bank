using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Bank
{
    enum UserType { Admin, Customer}
    abstract class User
    {
        protected string name;
        protected UserType type;
        protected int id;

        public User(string name, UserType type, int id)
        {
            this.name = name;
            this.type = type;
            this.id = id;
        }

        public UserType Type
        {
            get
            {
                return type;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public int ID
        {
            get
            {
                return id;
            }
        }
        
    }

}
