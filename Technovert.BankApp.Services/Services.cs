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
        Random rand = new Random();
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
            return this.banks.SingleOrDefault(x => x.Id == bankId);
        }

        public Account AccountFinder(string bankId, string accountId)
        {
            Bank bank = BankFinder(bankId);
            return bank.Accounts.SingleOrDefault(x => x.Id == accountId);
        }
        
        public string BankIdGenerator(string bankName)
        {
            return bankName.Substring(0,3).ToUpper() + today.ToString("dd")+today.ToString("MM")+today.ToString("yyyy") + today.ToString("hh") + today.ToString("mm");
        }
        public string AccountIdGenerator(string AccountHolderName)
        {
            return AccountHolderName.Substring(0,3).ToUpper() + today.ToString("dd") + today.ToString("MM") + today.ToString("yyyy") + today.ToString("hh") + today.ToString("mm");
        }
        public string TransactionIdGenerator(string bankId, string accountId)
        {
            return "TXN" + bankId + accountId + today.ToString("dd") + today.ToString("MM") + today.ToString("yyyy") + today.ToString("hh") + today.ToString("mm");
        }

        public string CreateBank(string bankName)
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

            return currentBankId;
        }
        public string CreateAccount(string bankId, string accountHolderName, string password)
        {
            string currentAccountId = AccountIdGenerator(accountHolderName);
            Bank currentAccount = BankFinder(currentAccountId);

            if (currentAccount != null)
            {
                throw new DuplicateAccountNameException();
            }
            Account account = new Account()
            {
                Name = accountHolderName,
                Id = AccountIdGenerator(accountHolderName),
                Password = password,
                Balance = 0,
                Transactions = new List<Transaction>()
            };

            Bank bank = BankFinder(bankId);
            bank.Accounts.Add(account);

            return currentAccountId;
        }

        public void BankLogin(string bankId)
        {
            Bank currentBank = BankFinder(bankId);
            if (currentBank == null)
            {
                throw new InvalidBankNameException();
            }
        }

        public void AccountLogin(string bankId, string accountHolderId, string password)
        {
            InputValidator(bankId, accountHolderId);

            Account userAccount = AccountFinder(bankId, accountHolderId);

            if (userAccount == null)
            {
                throw new InvalidAccountNameException();
            }
            if (userAccount.Password != password)
            {
                throw new InvalidAccountPasswordException();
            }
        }

        public void Deposit(string bankId, string accountHolderId, decimal amount)
        {
            InputValidator(bankId, accountHolderId);

            Account userAccount = AccountFinder(bankId, accountHolderId);

            if (userAccount == null)
            {
                throw new InvalidAccountNameException();
            }
            userAccount.Balance += amount;
            userAccount.Transactions.Add(new Transaction()
            {
                Id = TransactionIdGenerator(bankId, accountHolderId),
                Amount = amount,
                Type = TransactionType.Credit,
                On = today
            });
        }

        public void Withdraw(string bankId, string accountHolderId, decimal amount)
        {
            InputValidator(bankId, accountHolderId);

            Account userAccount = AccountFinder(bankId, accountHolderId);

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
                Id = TransactionIdGenerator(bankId, accountHolderId),
                Amount = amount,
                Type = TransactionType.Debit,
                On=today
            });
        }

        public void Transfer(string userBankId, string userAccountId, decimal amount, string beneficiaryBankId, string beneficiaryAccountId)
        {
            InputValidator(userAccountId, beneficiaryAccountId, userBankId, beneficiaryBankId);

            Account userAccount = AccountFinder(userBankId, userAccountId);

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

        public List<Transaction> TransactionLogCopy(string bankId, string accountId)
        {
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
