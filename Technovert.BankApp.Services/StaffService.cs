using System;
using System.Collections.Generic;
using System.Linq;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Exceptions;
using Technovert.BankApp.Models.Enums;
using System.Runtime.InteropServices;

namespace Technovert.BankApp.Services
{
    // Services available for staff Members
    public class StaffService
    {
        DateTime today = DateTime.Today;
        Random rand = new Random();

        private List<StaffAccount> staff;

        private Data data;
        private BankService bankService;
        private AccountHolderService accountHolderService;
        private TransactionService transactionService;

        public StaffService()
        {
            this.staff = new List<StaffAccount>();
        }

        public string GeneratePassword(string name) // Generates a random password based on user's name
        {
            return name.First().ToString().ToUpper() + name.Substring(1).ToLower()+"@"+rand.Next(100,999);
        }
        public string AccountIdGenerator(string AccountHolderName) // Generates a account id for newly created account
        {
            return AccountHolderName.Substring(0, 3).ToUpper() + today.ToString("dd") + today.ToString("MM") + today.ToString("yyyy") + DateTime.Now.ToString("HH")+DateTime.Now.ToString("mm");
        }

        public void CreateStaffAccount(string name) // Creates a staff account with default id and password
        {
            StaffAccount newStaff = new StaffAccount()
            {
                Name = name,
                Password = name + "@123",
                Id = name
            };
            this.staff.Add(newStaff);
        }

        public string[] CreateAccount(string name,string bankId) // Creates a new user account in a bank containing bankId
        {
            accountHolderService.InputValidator(name, bankId);
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

            return new string[]{ newAccount.Id, newAccount.Password}; // Returns the newly created user id and password for user
        }

        public void Login(Data data, BankService bankService, AccountHolderService accountHolderService, TransactionService transactionService, string id,string password)
        { // Logs the staff into their accounts

            accountHolderService.InputValidator(id, password);
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

        public Account ViewAccountDetails(string bankId,string accountId) // Returns the account details of user
        {
            this.accountHolderService.InputValidator(bankId, accountId);
            Account account = this.accountHolderService.AccountFinder(bankId, accountId);
            if (account == null)
            {
                throw new InvalidAccountNameException(); 
            }
            return account;
        }

        public void AddNewCurrency(string name,string code,decimal exchangeRate) // used to add new currencies into the application
        {
            this.accountHolderService.InputValidator(name, code, exchangeRate.ToString());
            Currency newCurrency = new Currency()
            {
                name = name,
                code = code,
                exchangeRate = exchangeRate
            };
            this.data.currencies.Add(newCurrency);
        }

        public void UpdateAccount(string accountId,string bankId,string newName=null,string newPassword=null)
        {// Updates the user account with either newName or newPassword
            this.accountHolderService.InputValidator(accountId, bankId);
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

        public void DeleteAccount(string accountId,string bankId)// Deletes the account from the respective bank
        {
            this.accountHolderService.InputValidator(accountId, bankId);
            Bank bank = this.bankService.BankFinder(bankId);
            Account account = this.accountHolderService.AccountFinder(bankId, accountId);
            if (account == null)
            {
                throw new InvalidAccountNameException();
            }
            bank.Accounts.Remove(account);
        }

        public List<Transaction> ViewTransactions(string bankId,string accountId) // Returns the done by the account holder
        {
            this.accountHolderService.InputValidator(accountId, bankId);
            Account account = this.accountHolderService.AccountFinder(bankId, accountId);
            if (account == null)
            {
                throw new InvalidAccountNameException();
            }
            return account.Transactions;
        }

        public void RevertTransaction(string bankId,string accountId,string transactionId)
        { // Reverts any Transaction done by the account holder
            this.accountHolderService.InputValidator(accountId, bankId,transactionId);
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

            Account beneficiaryAccount = this.accountHolderService.AccountFinder(undoTransaction.DestinationBankId, undoTransaction.DestinationAccountId);
            List<Transaction> beneficiarytransaction = beneficiaryAccount.Transactions;
            Transaction beneficiaryUndoTransaction = beneficiarytransaction.SingleOrDefault(x => x.Id == transactionId);
            beneficiarytransaction.Remove(beneficiaryUndoTransaction);

            beneficiaryAccount.Balance -= undoTransaction.Amount;
            account.Balance += undoTransaction.Amount;
        }
    }
}
