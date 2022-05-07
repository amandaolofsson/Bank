using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class AdminSession : ISession
    {
        User user;
        UserManager userManager;

        public AdminSession(User user, UserManager userManager)
        {
            this.user = user;
            this.userManager = userManager;
        }

        public void Start()
        {
            while (true)
            {
                Console.WriteLine(@"Hello {0}. What would you like to do?
[C] Create customer 
[V] View customers
[D] Delete customers 
[L] Log out", user.Name);

                string option = UserInput.Get(new string[] { "C", "V", "D", "L", "c", "v", "d", "l" }).ToLower();

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
            string name = UserInput.Prompt("Customer name: ");
            string ssn = UserInput.Prompt("Social security number: ");

            Customer customer = new Customer(ssn, name, userManager.GetNextUserID());
            userManager.Add(customer);
            Console.WriteLine("Customer created. Id: {0}", customer.ID);
        }

        void ViewCustomers()
        {
            foreach (Customer c in userManager.GetCustomers())
            {
                Console.WriteLine("Id: {0}  Name: {1}   SSN: {2}", c.ID, c.Name, c.SSN);
            }
        }

        void DeleteCustomer()
        {
            int userId = UserInput.PromptInt("Enter userId: ");
            if (userManager.Delete(userId))
            {
                Console.WriteLine("Removed!");
            }
            else
            {
                Console.WriteLine("User not found");
            }
        }
}
}
