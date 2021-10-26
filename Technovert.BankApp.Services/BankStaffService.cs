using System;
using System.Collections.Generic;
using System.Linq;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Exceptions;
using Technovert.BankApp.Models.Enums;
using System.Runtime.InteropServices;

namespace Technovert.BankApp.Services
{
    public class BankStaffService
    {
        private List<StaffAccount> staff;
        DateTime today = DateTime.Today;
        Random rand = new Random();

        private Data data;
        private BankService bankService;
        private AccountHolderService accountHolderService;
        private TransactionService transactionService;
        public BankStaffService()
        {
            this.staff = new List<StaffAccount>();
        }
        public string GeneratePassword(string name)
        {
            return name.First().ToString().ToUpper() + name.Substring(1).ToLower()+"@"+rand.Next(100,999);
        }
        public string AccountIdGenerator(string AccountHolderName)
        {
            return AccountHolderName.Substring(0, 3).ToUpper() + today.ToString("dd") + today.ToString("MM") + today.ToString("yyyy") + DateTime.Now.ToString("HH")+DateTime.Now.ToString("mm");
        }
        public void CreateStaffAccount(string name)
        {
            StaffAccount newStaff = new StaffAccount()
            {
                Name = name,
                Password = name + "@123",
                Id = name
            };
            this.staff.Add(newStaff);
        }
        public string[] CreateAccount(string name,string bankId)
        {
            Account newAccount = new Account()
            {
                Name = name,
                Id = AccountIdGenerator(name),
                Password = GeneratePassword(name),
                Balance = 0,
                Transactions = new List<Transaction>()
            };
            Bank bank = this.bankService.BankFinder(bankId);
            bank.Accounts.Add(newAccount);

            return new string[]{ newAccount.Id, newAccount.Password};
        }
        public void Login(Data data, BankService bankService, AccountHolderService accountHolderService, TransactionService transactionService, string id,string password)
        {
            if(String.IsNullOrWhiteSpace(id) || String.IsNullOrWhiteSpace(password))
            {
                throw new InvalidInputException();
            }
            StaffAccount staffAccount = this.staff.SingleOrDefault(x => x.Id == id);
            if (staffAccount == null)
            {
                throw new InvalidAccountNameException();
            }
            if (staffAccount.Password != password)
            {
                throw new InvalidAccountPasswordException();
            }
            this.data = data;
            this.bankService = bankService;
            this.accountHolderService = accountHolderService;
            this.transactionService = transactionService;
        }
        public Account ViewAccountDetails(string bankId,string accountId)
        {
            Account account = this.accountHolderService.AccountFinder(bankId, accountId);
            if (account == null)
            {
                throw new InvalidAccountNameException(); 
            }
            return account;
        }
        public void AddNewCurrency(string name,string code,decimal exchangeRate)
        {
            Currency newCurrency = new Currency()
            {
                Name = name,
                Code = code,
                ExchangeRate = exchangeRate
            };
            this.data.currencies.Add(newCurrency);
        }
        public void UpdateAccount(string accountId,string bankId,string newName=null,string newPassword=null)
        {
            Account account = this.accountHolderService.AccountFinder(bankId, accountId);
            if (account == null)
            {
                throw new InvalidAccountNameException(); // Invalid?!
            }
            if (newName != null)
            {
                account.Name = newName;
            }
            if (newPassword != null)
            {
                account.Password = newPassword;
            }
        }
        public void DeleteAccount(string accountId,string bankId)
        {
            Bank bank = this.bankService.BankFinder(bankId);
            Account account = this.accountHolderService.AccountFinder(bankId, accountId);
            if (account == null)
            {
                throw new InvalidAccountNameException();
            }
            bank.Accounts.Remove(account);
        }
        public List<Transaction> ViewTransactions(string bankId,string accountId)
        {
            Account account = this.accountHolderService.AccountFinder(bankId, accountId);
            if (account == null)
            {
                throw new InvalidAccountNameException();
            }
            return account.Transactions;
        }
        public void RevertTransaction(string bankId,string accountId,string transactionId)
        {
            Account account = this.accountHolderService.AccountFinder(bankId, accountId);
            if (account == null)
            {
                throw new InvalidAccountNameException();
            }
            List<Transaction> transaction=account.Transactions;
            if (transaction.Count == 0)
            {
                throw new InsufficientTransactions();
            }
            Transaction undoTransaction = transaction.SingleOrDefault(x => x.Id == transactionId);
            if (undoTransaction == null)
            {
                throw new InvalidInputException();
            }
            transaction.Remove(undoTransaction);
            if (undoTransaction.TaxType ==TaxType.IMPS || undoTransaction.TaxType==TaxType.RTGS)
            {
                this.transactionService.Transfer(undoTransaction.DestinationBankId, undoTransaction.DestinationAccountId, undoTransaction.Amount, undoTransaction.SourceBankId, undoTransaction.SourceAccountId,undoTransaction.TaxType,true);
            }
        }
    }
}
