using System;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Models;

namespace Technovert.BankApp.Services
{

    public class BankService
    {
        private List<Bank> banks;
        DateTime today = DateTime.Today;
        public BankService()
        {
            this.banks = new List<Bank>();
        }

        public bool InputValidator(params string[] inputs)
        {
            foreach (var input in inputs)
            {
                if (String.IsNullOrWhiteSpace(input))
                {
                    return false;
                }
            }
            return true;
        }

        public Bank BankFinder(string bankId)
        {
            foreach (var d in banks)
            {
                if (d.Id == bankId)
                {
                    return d;
                }
            }
            return null;
        }

        public Account AccountFinder(string bankId, string accountId)
        {
            Bank bank = BankFinder(bankId);
            foreach(var d in bank.Accounts)
            {
                if (d.Id == accountId)
                {
                    return d;
                }
            }
            return null;
        }
        
        public string BankIdGenerator(string bankName)
        {
            return bankName.Substring(0,3).ToUpper() + today.ToString("dd")+today.ToString("MM")+today.ToString("yyyy");
        }
        public string AccountIdGenerator(string AccountHolderName)
        {
            return AccountHolderName.Substring(0,3).ToUpper() + today.ToString("dd") + today.ToString("MM") + today.ToString("yyyy");
        }
        public string TransactionIdGenerator(string bankId, string accountId)
        {
            return "TXN" + bankId + accountId + today.ToString("dd") + today.ToString("MM") + today.ToString("yyyy");
        }

        public string CreateBank(string bankName)
        {
            Bank newBank = new Bank()
            {
                Name = bankName,
                Id = BankIdGenerator(bankName),
                Accounts = new List<Account>()
            };
            this.banks.Add(newBank);
            return newBank.Id;
        }

<<<<<<< HEAD
        public void CreateAccount(string accountHolderName, string bankId, string password)
        {
            Account account = new Account()
            {
                Name= accountHolderName,
=======
        public void CreateAccount(string bankName, string accountHolderName, string password)
        {
            Account account = new Account()
            {
                Name=accountHolderName,
>>>>>>> master
                Id = AccountIdGenerator(accountHolderName),
                Password = password,
                Balance = 0,
                Transactions = new List<Transaction>()
            };
            string bankId = BankIdGenerator(bankName);
            Bank bank = BankFinder(bankId);
            bank.Accounts.Add(account);
        }

        public bool BankLogin(string bankName)
        {
            string bankId = BankIdGenerator(bankName);
            foreach (var d in banks)
            {
                if (d.Id == bankId)
                {
                    return true;
                }
            }
            return false;
        }

        public bool AccountLogin(string bankName, string accountName, string password)
        {
            if (InputValidator(bankName,accountName))
            {
                string bankId = BankIdGenerator(bankName);
                string accountId = AccountIdGenerator(accountName);
                Account userAccount = AccountFinder(bankId, accountId);
                if (userAccount == null || userAccount.Password != password)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool Deposit(string bankName, string accountName, decimal amount)
        {
            if (InputValidator(bankName,accountName))
            {
                string bankId = BankIdGenerator(bankName);
                string accountId = AccountIdGenerator(accountName);
                Account userAccount = AccountFinder(bankId, accountId);
                if (userAccount == null)
                {
                    return false;
                }
                userAccount.Balance += amount;
                userAccount.Transactions.Add(new Transaction()
                {
                    Id = TransactionIdGenerator(bankId,accountId),
                    Amount = amount,
                    Type = TransactionType.Credit
                });
                return true;
            }
            return false;
        }

        public bool Withdraw(string bankName, string accountName, decimal amount)
        {
            if (InputValidator(bankName,accountName))
            {
                string bankId = BankIdGenerator(bankName);
                string accountId = AccountIdGenerator(accountName);
                Account userAccount = AccountFinder(bankId, accountId);
                if (userAccount == null || userAccount.Balance < amount)
                {
                    return false;
                }
                userAccount.Balance -= amount;
                userAccount.Transactions.Add(new Transaction()
                {
                    Id = TransactionIdGenerator(bankId, accountId),
                    Amount = amount,
                    Type = TransactionType.Debit
                });
                return true;
            }
            return false;
        }

        public bool Transfer(string userBankName, string userAccountName, decimal amount, string beneficiaryBankName, string beneficiaryAccountName)
        {
            try
            {
                if (InputValidator(userAccountName,beneficiaryAccountName,userBankName,beneficiaryBankName))
                {
                    string userBankId = BankIdGenerator(userBankName);
                    string userAccountId = AccountIdGenerator(userAccountName);
                    Account userAccount = AccountFinder(userBankId, userAccountId);
                    string beneficiaryBankId = BankIdGenerator(beneficiaryBankName);
                    string beneficiaryAccountId = AccountIdGenerator(beneficiaryAccountName);
                    Account beneficiaryAccount = AccountFinder(beneficiaryBankId, beneficiaryAccountId);
                    if (userAccount == null || beneficiaryAccount == null || userAccount.Balance < amount)
                    {
                        return false;
                    }
                    userAccount.Balance -= amount;
                    userAccount.Transactions.Add(new Transaction()
                    {
                        Id = TransactionIdGenerator(userBankId, userAccountId),
                        Amount = amount,
                        Type = TransactionType.Debit
                    });
                    beneficiaryAccount.Balance += amount;
                    beneficiaryAccount.Transactions.Add(new Transaction()
                    {
                        Id = TransactionIdGenerator(beneficiaryBankId, beneficiaryAccountId),
                        Amount = amount,
                        Type = TransactionType.Credit
                    });
                    return true;
                }
            }
            catch { }
            return false;
        }

        public List<Transaction> TransactionLogCopy(string bankName, string accountName)
        {
            string bankId = BankIdGenerator(bankName);
            string accountId = AccountIdGenerator(accountName);
            List<Transaction> CopiedTransactions = new List<Transaction>();
            foreach (var i in banks)
            {
                if (i.Id == bankId)
                {
                    foreach (var c in i.Accounts)
                    {
                        if (c.Id == accountId)
                        {
                            CopiedTransactions = c.Transactions;
                        }
                    }
                }
            }
            return CopiedTransactions;
        }
    }
}
