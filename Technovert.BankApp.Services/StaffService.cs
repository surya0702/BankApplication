using System;
using System.Collections.Generic;
using System.Linq;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Exceptions;
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
        private AccountHolderService accountHolderService;
        private TransactionService transactionService;
        private HashingService hashing = new HashingService();
        private SqlCommands commands = new SqlCommands();
        private BankDbContext DbContext ;

        public StaffService(BankDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public string GeneratePassword(string name) // Generates a random password based on user's name
        {
            return name.First().ToString().ToUpper() + name.Substring(1,4).ToLower().Trim()+"@"+rand.Next(1000,9999);
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
                DbContext.Staff.Add(newStaff);
                DbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            /*this.commands.createStaffAccount(name,newId,newPassword);*/
        }

        public string[] CreateAccount(string name,string bankId,int age,string gender) // Creates a new user account in a bank containing bankId
        {
            accountHolderService.InputValidator(name, bankId);
            string newPassword = GeneratePassword(name);
            string newAccountId = AccountIdGenerator(name);
            try
            {
                var newUser = new Account()
                {
                    Id = newAccountId,
                    Name = name,
                    Password = newPassword,
                    Balance = 0,
                    BankId = bankId,
                    Age=age,
                    Gender=gender
                };
                DbContext.Accounts.Add(newUser);
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            /*this.commands.createUserAccount(name, newAccountId, newPassword, bankId, age, gender);*/
            return new string[]{ newAccountId, newPassword}; // Returns the newly created user id and password for user
        }

        public void Login(BankService bankService, AccountHolderService accountHolderService, TransactionService transactionService, string id,string password)
        { // Logs the staff into their accounts

            accountHolderService.InputValidator(id, password);
            try
            {
                var info = DbContext.Staff.SingleOrDefault(m => m.Id == id);
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
            this.accountHolderService.InputValidator(name, code, exchangeRate.ToString());
            try
            {
                var newCurrency = new Currency()
                {
                    Code = code,
                    Name = name,
                    InverseRate = exchangeRate
                };
                DbContext.Currencies.Add(newCurrency);
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateAccount(string accountId,string bankId,string newName=null,string newPassword=null,int? newAge=0,string newGender=null)
        {// Updates the user account with either newName or newPassword
            this.accountHolderService.InputValidator(accountId, bankId);
            try
            {
                var info = DbContext.Accounts.SingleOrDefault(m => m.Id == accountId && m.BankId == bankId);
                if (info == null)
                {
                    throw new Exception("Invalid Details");
                }
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
                    info.Gender = newGender;
                }
                DbContext.Accounts.Update(info);
                DbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteAccount(string accountId,string bankId)// Deletes the account from the respective bank
        {
            this.accountHolderService.InputValidator(accountId, bankId);
            try
            {
                var info = DbContext.Accounts.SingleOrDefault(m => m.Id == accountId && m.BankId == bankId);
                if (info == null)
                    throw new Exception("Invalid Details");
                DbContext.Accounts.Remove(info);
                DbContext.SaveChanges();
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
                //this.accountHolderService.InputValidator(accountId, bankId, transactionId);
                var userInfo = DbContext.Accounts.SingleOrDefault(m => m.BankId == bankId && m.Id == accountId);
                if (userInfo == null)
                    throw new Exception("User Account was not Available!");

                var userTxn = DbContext.Transactions.SingleOrDefault(m => m.Id == transactionId && m.AccountId==accountId && m.BankId==bankId);
                if (userTxn == null)
                    throw new Exception("Invalid Transaction Id!");

                var beneInfo = DbContext.Accounts.SingleOrDefault(m => m.BankId == userTxn.DestinationBankId && m.Id == userTxn.DestinationAccountId);
                if (beneInfo == null)
                    throw new Exception("Beneficiary Account was not Available!");

                string beneTxnId = "TXN" + userTxn.DestinationBankId + userTxn.DestinationAccountId + transactionId.Substring(29);

                var beneTxn = DbContext.Transactions.SingleOrDefault(m => m.Id == beneTxnId && m.BankId == userTxn.DestinationBankId && m.AccountId == userTxn.DestinationAccountId);


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

                DbContext.Transactions.Remove(userTxn);
                DbContext.Transactions.Remove(beneTxn);
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
