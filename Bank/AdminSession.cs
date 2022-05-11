using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class AdminSession : SessionBase
    {
        User user;
        UserManager userManager;

        public AdminSession(Communication communication, User user, UserManager userManager) : base(communication)
        {
            this.user = user;
            this.userManager = userManager;
        }

        public override void Start()
        {
            while (true)
            {
                communication.Send(ServerMessageEnum.Response, @"Hello {0}. What would you like to do?
[C] Create customer 
[V] View customers
[D] Delete customers 
[L] Log out", user.Name);

                string option = communication.Get(new string[] { "C", "V", "D", "L", "c", "v", "d", "l" }).ToLower();

                switch (option)
                {
                    case "c":
                        CreateCustomer();
                        break;
                    case "v":
                        ViewCustomers();
                        break;
                    case "d":
                        DeleteCustomer();
                        break;
                    case "l":
                        return;
                }
            }
        }

        void CreateCustomer()
        {
            string name = communication.Prompt("Customer name: ");
            string ssn = communication.Prompt("Social security number: ");

            Customer customer = new Customer(ssn, name, userManager.GetNextUserID());
            userManager.Add(customer);
            communication.SendText("Customer created. Id: {0}", customer.ID);
        }

        void ViewCustomers()
        {
            foreach (Customer c in userManager.GetCustomers())
            {
                communication.SendText("Id: {0}  Name: {1}   SSN: {2}", c.ID, c.Name, c.SSN);
            }
        }

        void DeleteCustomer()
        {
            int userId = communication.PromptInt("Enter userId: ");

            if (userManager.Delete(userId))
            {
                communication.SendText("Removed!");
            }
            else
            {
                communication.SendText("User not found");
            }
        }
}
}
