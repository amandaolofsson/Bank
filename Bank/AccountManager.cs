using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Bank
{
    enum AccountOperationStatus {Success, AccountBalanceNotZero, AccountNotFound };

    class AccountManager
    {
        List<Account> accounts = new List<Account>();

        int nextAccountID;

        public int GetNextAccountID()
        {
            return nextAccountID++;
        }

        public void Add(Account account)
        {
            accounts.Add(account);
            Save();
        }

        public AccountOperationStatus Delete(int id)
        {
            Account account = accounts.FirstOrDefault(c => c.Id == id);

            if (account != null)
            {
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
                    
                    accounts.Add(new Account(balance, accountID, customerId));
                }
            }
            catch (System.IO.FileNotFoundException)
            {

            }
        }
    }
}
