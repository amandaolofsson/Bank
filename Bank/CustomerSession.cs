using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class CustomerSession : ISession
    {
        User user;
        UserManager userManager;
        AccountManager accountManager;

        public CustomerSession(User user, UserManager um, AccountManager am)
        {
            this.user = user;
            this.userManager = um;
            this.accountManager = am;
        }

        public void Start()
        {
            while (true)
            {
                Console.WriteLine(@"Hello {0}. What would you like to do?
[C] Create new account
[D] Delete account
[V] View balance
[P] Put money in
[W] Withdraw money
[T] Transfer money
[L] Log out", user.Name);

                string option = UserInput.Get(new string[] { "C", "V", "D", "L", "P", "W", "T", "c", "v", "d", "l", "p", "w", "t" }).ToLower();

                switch (option)
                {
                    case "c":
                        CreateAccount();
                        break;
                    case "d":
                        DeleteAccount();
                        break;
                    case "v":
                        ViewAccounts();
                        break;
                    case "p":
                        Insert();
                        break;
                    case "w":
                        Withdraw();
                        break;
                    case "t":
                        break;
                    case "l":
                        return;
                }
            }
        }

        void CreateAccount()
        {
            Account account = new Account(0, accountManager.GetNextAccountID(), user.ID);
            accountManager.Add(account);
            Console.WriteLine("Account created. Id: {0} Balance: {1}", account.Id, account.Balance);
        }

        void DeleteAccount()
        {
            int accountId = UserInput.PromptInt("Enter accountId: ");

            AccountOperationStatus status = accountManager.Delete(accountId, user.ID);

            switch (status)
            {
                case AccountOperationStatus.Success:
                    Console.WriteLine("Account successfully removed");
                    break;
                case AccountOperationStatus.AccountNotFound:
                    Console.WriteLine("Account not found");
                    break;
                case AccountOperationStatus.AccountBalanceNotZero:
                    Console.WriteLine("Money still left in account");
                    break;
                
            }
        }

        void ViewAccounts()
        {
            foreach (Account a in accountManager.GetAccountsForCustomer(user.ID))
            {
                Console.WriteLine("Id: {0}  Balance: {1}", a.Id, a.Balance);
            }
        }

        void Insert()
        {
            int accountId = UserInput.PromptInt("Enter accountId: ");
            int amount = UserInput.PromptInt("Enter amount: ");

            AccountOperationStatus status = accountManager.InsertMoney(accountId, amount);

            switch (status)
            {
                case AccountOperationStatus.Success:
                    Console.WriteLine("Withdraw successfull");
                    break;
                case AccountOperationStatus.AccountNotFound:
                    Console.WriteLine("Account not found");
                    break;
                case AccountOperationStatus.AmountMustBePositive:
                    Console.WriteLine("Amount must be positive");
                    break;
            }
        }

        void Withdraw()
        {
            int accountId = UserInput.PromptInt("Enter accountId: ");
            int amount = UserInput.PromptInt("Enter amount: ");

            AccountOperationStatus status = accountManager.WithdrawMoney(accountId, amount, user.ID);

            switch (status)
            {
                case AccountOperationStatus.Success:
                    Console.WriteLine("Withdraw successfull");
                    break;
                case AccountOperationStatus.AccountNotFound:
                    Console.WriteLine("Account not found");
                    break;
                case AccountOperationStatus.AmountMustBePositive:
                    Console.WriteLine("Amount must be positive");
                    break;
            }
        }
    }
}
