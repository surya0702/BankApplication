using System;
using System.Collections.Generic;
using System.Linq;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Enums;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.SqlClient;
using Technovert.BankApp;

namespace Technovert.BankApp.Services
{
    // Services available for staff Members
    public class StaffService
    {
        DateTime today = DateTime.Today;
        Random rand = new Random();


        private BankService bankService;
        private AccountService accountHolderService;
        private TransactionService transactionService;
        private HashingService hashing = new HashingService();
        private BankDbContext _DbContext ;

        public StaffService(BankDbContext dbContext)
        {
            _DbContext = dbContext;
            CreateStaffAccount("Admin");
        }

        public string GeneratePassword(string name) // Generates a random password based on user's name
        {
            string temp = "";
            foreach(var letter in name)
            {
                if (letter != ' ')
                    temp += letter;
            }
            return temp.First().ToString().ToUpper() + temp.Substring(1,4).ToLower().Trim()+"@"+rand.Next(1000,9999);
        }
        public string AccountIdGenerator(string AccountHolderName) // Generates a account id for newly created account
        {
            return AccountHolderName.Substring(0, 3).ToUpper() + today.ToString("dd") + today.ToString("MM") + today.ToString("yyyy") + DateTime.Now.ToString("HH")+DateTime.Now.ToString("mm");
        }

        public void CreateStaffAccount(string name) // Creates a staff account with default id and password
        {
            string newId = name;
            string newPassword = name + "@123";
            try
            {
                var newStaff = new StaffAccount()
                {
                    Id = newId,
                    Name = name,
                    Password = newPassword
                };
                _DbContext.Staff.Add(newStaff);
                _DbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        

        public void Login(BankService bankService, AccountService accountHolderService, TransactionService transactionService, string id,string password)
        { // Logs the staff into their accounts

            hashing.InputValidator(id, password);
            try
            {
                var info = _DbContext.Staff.SingleOrDefault(m => m.Id == id);
                if (info == null)
                {
                    throw new Exception("Invalid Staff Id");
                }
                if (info.Password != password)
                    throw new Exception("Invalid Password");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            this.bankService = bankService;
            this.accountHolderService = accountHolderService;
            this.transactionService = transactionService;
        }

        public void AddNewCurrency(string name,string code,decimal exchangeRate) // used to add new currencies into the application
        {
            try
            {
                var newCurrency = new Currency()
                {
                    Code = code,
                    Name = name,
                    InverseRate = exchangeRate
                };
                _DbContext.Currencies.Add(newCurrency);
                _DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateAccount(string accountId,string bankId,string newName=null,string newPassword=null,int? newAge=0,string newGender=null)
        {// Updates the user account with either newName or newPassword
            try
            {
                var info = _DbContext.Accounts.SingleOrDefault(m => m.Id == accountId && m.BankId == bankId);
                if (info == null)
                {
                    throw new Exception("Invalid Details");
                }
                if (info.AccountStatus == Status.Closed)
                    throw new Exception("Account was Closed! Can't Update the Account Details");
                if (newAge != 0)
                {
                    info.Age = (int)newAge;
                }
                else if (String.IsNullOrEmpty(newName)==false)
                {
                    info.Name = newName;
                }
                else if (String.IsNullOrEmpty(newPassword) == false)
                {
                    info.Password = newPassword;
                }
                else if (String.IsNullOrEmpty(newGender) == false)
                {
                    info.Gender = Gender.Male;
                }
                _DbContext.Accounts.Update(info);
                _DbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteAccount(string accountId,string bankId)// Deletes the account from the respective bank
        {
            try
            {
                var info = _DbContext.Accounts.SingleOrDefault(m => m.BankId == bankId && m.Id == accountId);
                if (info == null)
                    throw new Exception("Invalid Details");
                if (info.AccountStatus == Status.Closed)
                    throw new Exception("Account was already Closed!");
                info.AccountStatus = Status.Closed;
                _DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RevertTransaction(string bankId,string accountId,string transactionId)
        { // Reverts any Transaction done by the account holder
            try
            {
                var userInfo = _DbContext.Accounts.SingleOrDefault(m => m.BankId == bankId && m.Id == accountId);
                if (userInfo == null || userInfo.AccountStatus==Status.Closed)
                    throw new Exception("User Account was not Available!");

                var userTxn = _DbContext.Transactions.SingleOrDefault(m => m.Id == transactionId && m.AccountId==accountId && m.BankId==bankId);
                if (userTxn == null)
                    throw new Exception("Invalid Transaction Id!");

                var beneInfo = _DbContext.Accounts.SingleOrDefault(m => m.BankId == userTxn.DestinationBankId && m.Id == userTxn.DestinationAccountId);
                if (beneInfo == null || beneInfo.AccountStatus== Status.Closed)
                    throw new Exception("Beneficiary Account was not Available!");

                string beneTxnId = "TXN" + userTxn.DestinationBankId + userTxn.DestinationAccountId + transactionId.Substring(29);

                var beneTxn = _DbContext.Transactions.SingleOrDefault(m => m.Id == beneTxnId && m.BankId == userTxn.DestinationBankId && m.AccountId == userTxn.DestinationAccountId);

/*
                if (userTxn.TransactionType == "Debit")
                {
                    userInfo.Balance += userTxn.Amount;
                    beneInfo.Balance -= userTxn.Amount;
                }
                else
                {
                    userInfo.Balance -= userTxn.Amount;
                    beneInfo.Balance += userTxn.Amount;
                }

                userTxn.TxnStatus = "Reversed";
                beneTxn.TxnStatus = "Reversed";*/
                _DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
