using System;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Exceptions;

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

        public void InputValidator(params string[] inputs)
        {
            foreach (var input in inputs)
            {
                if (String.IsNullOrWhiteSpace(input))
                {
                    throw new InvalidInputException();
                }
            }
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

        public void CreateBank(string bankName)
        {
            string currentBankId = BankIdGenerator(bankName);
            Bank currentBank = BankFinder(currentBankId);

            if (currentBank != null)
            {
                throw new DuplicateBankNameException();
            }
            Bank newBank = new Bank()
            {
                Name = bankName,
                Id = currentBankId,
                Accounts = new List<Account>()
            };
            this.banks.Add(newBank);
        }
        public void CreateAccount(string bankName, string accountHolderName, string password)
        {
            string currentAccountId = AccountIdGenerator(accountHolderName);
            Bank currentAccount = BankFinder(currentAccountId);

            if (currentAccount != null)
            {
                throw new DuplicateAccountNameException();
            }
            Account account = new Account()
            {
                Name=accountHolderName,
                Id = AccountIdGenerator(accountHolderName),
                Password = password,
                Balance = 0,
                Transactions = new List<Transaction>()
            };

            string bankId = BankIdGenerator(bankName);
            Bank bank = BankFinder(bankId);
            bank.Accounts.Add(account);
        }

        public void BankLogin(string bankName)
        {
            string bankId = BankIdGenerator(bankName);
            foreach (var d in banks)
            {
                if (d.Id == bankId)
                {
                    return ;
                }
            }
            throw new InvalidBankNameException();
        }

        public void AccountLogin(string bankName, string accountName, string password)
        {
            InputValidator(bankName, accountName);

            string bankId = BankIdGenerator(bankName);
            string accountId = AccountIdGenerator(accountName);
            Account userAccount = AccountFinder(bankId, accountId);

            if (userAccount == null)
            {
                throw new InvalidAccountNameException();
            }
            if (userAccount.Password != password)
            {
                throw new InvalidAccountPasswordException();
            }
        }

        public void Deposit(string bankName, string accountName, decimal amount)
        {
            InputValidator(bankName, accountName);

            string bankId = BankIdGenerator(bankName);
            string accountId = AccountIdGenerator(accountName);
            Account userAccount = AccountFinder(bankId, accountId);

            if (userAccount == null)
            {
                throw new InvalidAccountNameException();
            }
            userAccount.Balance += amount;
            userAccount.Transactions.Add(new Transaction()
            {
                Id = TransactionIdGenerator(bankId, accountId),
                Amount = amount,
                Type = TransactionType.Credit,
                On = today
            });
        }

        public void Withdraw(string bankName, string accountName, decimal amount)
        {
            InputValidator(bankName, accountName);

            string bankId = BankIdGenerator(bankName);
            string accountId = AccountIdGenerator(accountName);
            Account userAccount = AccountFinder(bankId, accountId);

            if (userAccount == null)
            {
                throw new InvalidAccountNameException();
            }
            if(userAccount.Balance < amount)
            {
                throw new InsufficientFundsException();
            }
            userAccount.Balance -= amount;
            userAccount.Transactions.Add(new Transaction()
            {
                Id = TransactionIdGenerator(bankId, accountId),
                Amount = amount,
                Type = TransactionType.Debit,
                On=today
            });
        }

        public void Transfer(string userBankName, string userAccountName, decimal amount, string beneficiaryBankName, string beneficiaryAccountName)
        {
            InputValidator(userAccountName, beneficiaryAccountName, userBankName, beneficiaryBankName);

            string userBankId = BankIdGenerator(userBankName);
            string userAccountId = AccountIdGenerator(userAccountName);
            Account userAccount = AccountFinder(userBankId, userAccountId);

            string beneficiaryBankId = BankIdGenerator(beneficiaryBankName);
            string beneficiaryAccountId = AccountIdGenerator(beneficiaryAccountName);
            Account beneficiaryAccount = AccountFinder(beneficiaryBankId, beneficiaryAccountId);

            if (userAccount == null || beneficiaryAccount == null)
            {
                throw new InvalidAccountNameException();
            }
            if(userAccount.Balance < amount)
            {
                throw new InsufficientFundsException();
            }

            userAccount.Balance -= amount;
            userAccount.Transactions.Add(new Transaction()
            {
                Id = TransactionIdGenerator(userBankId, userAccountId),
                Amount = amount,
                Type = TransactionType.Debit,
                On=today
            });

            beneficiaryAccount.Balance += amount;
            beneficiaryAccount.Transactions.Add(new Transaction()
            {
                Id = TransactionIdGenerator(beneficiaryBankId, beneficiaryAccountId),
                Amount = amount,
                Type = TransactionType.Credit,
                On=today
            });
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
