using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Bank
{
    enum AccountOperationStatus {Success, AccountBalanceNotZero, AccountNotFound, AmountMustBePositive, NotEnoughMoney };

    class AccountManager
    {
        MyList<Account> accounts = new MyList<Account>();

        int nextAccountID;

        public int GetNextAccountID()
        {
            return nextAccountID++;
        }

        //This is to present client with list of only clients own accounts
        public IEnumerable<Account> GetAccountsForCustomer(int customerId)
        {
            //To separate clients own account(s) from list of Account
            return accounts.Where(a => a.CustomerId == customerId).Cast<Account>();
        }

        public void Add(Account account)
        {
            accounts.Add(account);
            Save();
        }

        //Unlike GetAccountsForCustomer, only returns a single account
        public Account GetAccountForCustomer(int accountId, int customerId)
        {
            //Checks if input accountId exists and if the account belongs to client
            Account account = accounts.FirstOrDefault(c => c.Id == accountId && c.CustomerId == customerId);

            return account;
        }

        public AccountOperationStatus Delete(int id, int customerId)
        {
            //Checks if account exists
            Account account = accounts.FirstOrDefault(c => c.Id == id);

            if (account != null)
            {
                //Cannot delete another customers account
                if(account.CustomerId != customerId)
                {
                    return AccountOperationStatus.AccountNotFound;
                }

                //Cannot delete while money still in account
                if(account.Balance != 0)
                {
                    return AccountOperationStatus.AccountBalanceNotZero;
                }

                accounts.Remove(account);
                Save();
                return AccountOperationStatus.Success;
            }
            return AccountOperationStatus.AccountNotFound;
        }

        public AccountOperationStatus InsertMoney(int accountId, int amount)
        {
            //Cannot insert an amount 0 or lower
            if(amount < 1)
            {
                return AccountOperationStatus.AmountMustBePositive;
            }

            Account account = accounts.FirstOrDefault(a => a.Id == accountId);

            if(account == null)
            {
                return AccountOperationStatus.AccountNotFound;
            }

            //Adds to transaction list
            //This is to keeping track of transactions regarding accounts
            account.Transactions.Add(new Transaction(DateTime.Now.ToString(), TransactionType.Insert, amount));

            account.Balance += amount;
            Save();
            return AccountOperationStatus.Success;
        }

        public AccountOperationStatus WithdrawMoney(int accountId, int amount, int customerId)
        {
            //Cannot withdraw a negative amount
            if (amount < 1)
            {
                return AccountOperationStatus.AmountMustBePositive;
            }

            //Checks if account exists
            Account account = accounts.FirstOrDefault(a => a.Id == accountId);

            if (account == null)
            {
                return AccountOperationStatus.AccountNotFound;
            }

            //Cannot withdraw money from another customers account
            if (account.CustomerId != customerId)
            {
                return AccountOperationStatus.AccountNotFound;
            }
            
            if(amount > account.Balance)
            {
                return AccountOperationStatus.NotEnoughMoney;
            }

            //Adds to transaction list
            //This is to keeping track of transactions regarding accounts
            account.Transactions.Add(new Transaction(DateTime.Now.ToString(), TransactionType.Withdraw, amount));

            account.Balance -= amount;
            Save();
            return AccountOperationStatus.Success;
        }

        public AccountOperationStatus Transfer(int fromAccountId, int toAccountId, int amount, int customerId)
        {
            //Checks if account exists
            Account toAccount = accounts.FirstOrDefault(a => a.Id == toAccountId);

            if(toAccount == null)
            {
                return AccountOperationStatus.AccountNotFound;
            }

            //BEGIN TRANSACTION
            AccountOperationStatus status = WithdrawMoney(fromAccountId, amount, customerId);

            //In case withdraw was not successfull, cancel transfer
            if (status != AccountOperationStatus.Success)
            {
                //ROLLBACK TRANSACTION
                return status;
            }

            //Adds to transaction list
            //This is to keeping track of transactions regarding accounts
            toAccount.Transactions.Add(new Transaction(DateTime.Now.ToString(), TransactionType.Insert, amount));

            toAccount.Balance += amount;
            Save();

            //COMMIT TRANSACTION
            return AccountOperationStatus.Success;
        }

        public void Save()
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = xmlDoc.DocumentElement;
            xmlDoc.InsertBefore(xmlDeclaration, root);

            XmlElement accountsElement = xmlDoc.CreateElement("", "accounts", "");
            XmlAttribute nextAccountIDAttribute = xmlDoc.CreateAttribute("", "nextaccountid", "");
            nextAccountIDAttribute.Value = nextAccountID.ToString();
            accountsElement.Attributes.Append(nextAccountIDAttribute);

            foreach (Account account in accounts)
            {
                XmlElement accountElement = xmlDoc.CreateElement("", "account", "");

                XmlAttribute accountID = xmlDoc.CreateAttribute("", "id", "");
                accountID.Value = account.Id.ToString();
                accountElement.Attributes.Append(accountID);

                XmlElement balanceElement = xmlDoc.CreateElement("", "balance", "");
                balanceElement.InnerText = account.Balance.ToString();
                accountElement.AppendChild(balanceElement);

                XmlElement customerIDElement = xmlDoc.CreateElement("", "customerid", "");
                customerIDElement.InnerText = account.CustomerId.ToString();
                accountElement.AppendChild(customerIDElement);

                XmlElement transactionsElement = xmlDoc.CreateElement("", "transactions", "");

                foreach(Transaction transaction in account.Transactions)
                {
                    XmlElement transactionElement = xmlDoc.CreateElement("", "transaction", "");

                    XmlAttribute typeAttr = xmlDoc.CreateAttribute("", "type", "");
                    typeAttr.Value = transaction.Type.ToString();
                    transactionElement.Attributes.Append(typeAttr);

                    XmlElement dateElement = xmlDoc.CreateElement("", "date", "");
                    dateElement.InnerText = transaction.Date;
                    transactionElement.AppendChild(dateElement);

                    XmlElement amountElement = xmlDoc.CreateElement("", "amount", "");
                    amountElement.InnerText = transaction.Amount.ToString();
                    transactionElement.AppendChild(amountElement);

                    transactionsElement.AppendChild(transactionElement);
                }

                accountElement.AppendChild(transactionsElement);
                accountsElement.AppendChild(accountElement);
            }

            xmlDoc.AppendChild(accountsElement);
            xmlDoc.Save("accounts.xml");
        }

        public void Load()
        {
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load("accounts.xml");
                accounts.Clear();
                XmlNodeList accountList = xmlDoc.SelectNodes("accounts/account");
                nextAccountID = int.Parse(xmlDoc.SelectSingleNode("accounts").Attributes["nextaccountid"].Value);

                foreach (XmlNode u in accountList)
                {
                    int accountID = int.Parse(u.Attributes["id"].Value);
                    int balance = int.Parse(u.SelectSingleNode("balance").InnerText);
                    int customerId = int.Parse(u.SelectSingleNode("customerid").InnerText);

                    Account account = new Account(balance, accountID, customerId);

                    foreach (XmlNode t in u.SelectNodes("transactions/transaction"))
                    {
                        string date = t.SelectSingleNode("date").InnerText;
                        int amount = int.Parse(t.SelectSingleNode("amount").InnerText);
                        TransactionType type = (TransactionType)Enum.Parse(typeof(TransactionType), t.Attributes["type"].Value);

                        account.Transactions.Add(new Transaction(date, type, amount));
                    }
                    
                    accounts.Add(account);
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                nextAccountID = 1;
            }
        }
    }
}
