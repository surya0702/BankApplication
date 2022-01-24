using System;
using System.Collections.Generic;
using System.Linq;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Enums;
using System.Data.SqlClient;
using Technovert.BankApp.Services.Interfaces;

namespace Technovert.BankApp.Services
{
    // All the services available for account holders
    public class AccountService : IAccountService
    {
        private HashingService hashing = new HashingService();
        private BankDbContext _DbContext;
        public AccountService(BankDbContext dbContext)
        {
            _DbContext = dbContext;
        }
        
        public string Authenticate(string bankId,string id,string password) // Logs the user into his account by checking with the available id and password
        {
            hashing.InputValidator(bankId,id,password);
            try
            {
                var info = _DbContext.Accounts.SingleOrDefault(m => m.Id == id && m.BankId == bankId);
                if (info == null)
                    throw new Exception("Invalid Details");
                if (info.AccountStatus == Status.Closed)
                    throw new Exception("Account Has Been Closed! Please Contact Bank Staff.");
                string currPassword = hashing.GetHash(password);
                if (info.Password != currPassword)
                    throw new Exception("Invalid Password!");
                return "Authentication Successful";
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Account CreateAccount(Account account) // Creates a new user account in a bank containing bankId
        {
            try
            {
                hashing.InputValidator(account);
                _DbContext.Accounts.Add(account);
                _DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return account; // Returns the newly created user id and password for user
        }

        public Account UpdateAccount(Account account)
        {
            _DbContext.Accounts.Attach(account);
            _DbContext.SaveChanges();
            var UpdatedAccount = _DbContext.Accounts.FirstOrDefault(m => m.Id == account.Id);
            return UpdatedAccount;
        }

        public Account CloseAccount(string bankId,string accountId)
        {
            var accountToDelete = _DbContext.Accounts.SingleOrDefault(m => m.BankId == bankId && m.Id == accountId);
            if (accountToDelete.AccountStatus == Status.Closed)
                throw new Exception("Account was Already Closed");
            accountToDelete.AccountStatus = Status.Closed;
            _DbContext.SaveChanges();
            return accountToDelete;
        }

        public Account GetAccount(string bankId,string accountId)
        {
            var account = _DbContext.Accounts.FirstOrDefault(m => m.BankId == bankId && m.Id == accountId);
            if (account == null)
                throw new Exception("Account Not Found!");
            if (account.AccountStatus == Status.Closed)
                throw new Exception("Account was Closed!");
            return account;
        }

        public List<Account> GetAllAccounts(string bankId)
        {
            return _DbContext.Accounts.Where(m => m.BankId == bankId && m.AccountStatus==Status.Active).ToList();
        }
    }
}
