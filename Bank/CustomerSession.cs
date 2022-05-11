using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class CustomerSession : SessionBase
    {
        User user;
        UserManager userManager;
        AccountManager accountManager;

        public CustomerSession(Communication communication, User user, UserManager um, AccountManager am) : base(communication)
        {
            this.user = user;
            this.userManager = um;
            this.accountManager = am;
        }

        public override void Start()
        {
            while (true)
            {
                communication.Send(@"Hello {0}. What would you like to do?
[C] Create new account
[D] Delete account
[V] View balance
[A] View account
[P] Put money in
[W] Withdraw money
[T] Transfer money
[L] Log out", user.Name);

                string option = communication.Get(new string[] { "C", "V", "D", "L", "P", "W", "T", "A", "c", "v", "d", "l", "p", "w", "t", "a" }).ToLower();

                switch (option)
                {
                    case "c":
                        CreateAccount();
                        break;
                    case "d":
                        DeleteAccount();
                        break;
                    case "v":
                        ViewBalance();
                        break;
                    case "a":
                        ViewAccount();
                        break;
                    case "p":
                        Insert();
                        break;
                    case "w":
                        Withdraw();
                        break;
                    case "t":
                        Transfer();
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
            communication.SendText("Account created. Id: {0} Balance: {1}", account.Id, account.Balance);
        }

        void DeleteAccount()
        {
            int accountId = communication.PromptInt("Enter accountId: ");

            AccountOperationStatus status = accountManager.Delete(accountId, user.ID);

            switch (status)
            {
                case AccountOperationStatus.Success:
                    communication.SendText("Account successfully removed");
                    break;
                case AccountOperationStatus.AccountNotFound:
                    communication.SendText("Account not found");
                    break;
                case AccountOperationStatus.AccountBalanceNotZero:
                    communication.SendText("Money still left in account");
                    break;
                
            }
        }

        void ViewBalance()
        {
            foreach (Account a in accountManager.GetAccountsForCustomer(user.ID))
            {
                Console.WriteLine("Id: {0}  Balance: {1}", a.Id, a.Balance);
            }
        }

        void ViewAccount()
        {
            int accountId = UserInput.PromptInt("Enter accountId: ");

            Account account = accountManager.GetAccountForCustomer(accountId, user.ID);

            if(account == null)
            {
                Console.WriteLine("Account not found");
                return;
            }

            Console.WriteLine("Id: {0}  Balance: {1}", account.Id, account.Balance);

            foreach (Transaction t in account.Transactions)
            {
                Console.WriteLine("Date: {0}    Type: {1}   Amount: {2}", t.Date, t.Type, t.Amount);
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
                    Console.WriteLine("Insert successfull");
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
                case AccountOperationStatus.NotEnoughMoney:
                    Console.WriteLine("Not enough money on account");
                    break;
            }
        }

        void Transfer()
        {
            int fromAccountId = UserInput.PromptInt("Enter from accountId: ");
            int toAccountId = UserInput.PromptInt("Enter to accountId: ");
            int amount = UserInput.PromptInt("Enter amount: ");

            AccountOperationStatus status = accountManager.Transfer(fromAccountId, toAccountId, amount, user.ID);

            switch (status)
            {
                case AccountOperationStatus.Success:
                    Console.WriteLine("Transfer successfull");
                    break;
                case AccountOperationStatus.AccountNotFound:
                    Console.WriteLine("Account not found");
                    break;
                case AccountOperationStatus.AmountMustBePositive:
                    Console.WriteLine("Amount must be positive");
                    break;
                case AccountOperationStatus.NotEnoughMoney:
                    Console.WriteLine("Not enough money on account");
                    break;
            }
        }
    }
}
