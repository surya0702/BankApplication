using System;
using System.Collections.Generic;
using System.Linq;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Enums;
using System.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Technovert.BankApp.Services.Interfaces;

namespace Technovert.BankApp.Services
{
    // Transaction Services available for Account holders
    public class TransactionService : ITransactionService
    {
        private BankDbContext _DbContext ;
        private IAccountService accountService;

        DateTime today = DateTime.Today;
        
        public TransactionService(BankDbContext dbContext,IAccountService accountService)
        {
            _DbContext = dbContext;
            this.accountService = accountService;
        }
        
        public Transactions GetTransaction(string id)
        {
            return _DbContext.Transactions.SingleOrDefault(m => m.Id == id);
        }

        public Transactions AddTransaction(Transactions transaction)
        {
            try
            {
                _DbContext.Transactions.Add(transaction);
                _DbContext.SaveChanges();
                return transaction;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Transactions UpdateTransaction(Transactions transaction)
        {
            throw new NotImplementedException();
        }

        public Transactions DeleteTransaction(Transactions transaction)
        {
            throw new NotImplementedException();
        }

        public List<Transactions> GetAllTransactions(string bankId, string accountId)
        {
            try
            {
                var info = _DbContext.Transactions.Where(m => (m.BankId == bankId && m.AccountId == accountId) || (m.DestinationBankId == bankId && m.DestinationAccountId == accountId)).ToList();

                return info;
            }
            catch
            {
                throw new Exception("No Available Transactions");
            }
        }

        public Transactions Deposit(string bankId,string accountId,decimal amount)
        {
            try
            {
                var info = accountService.GetAccount(bankId, accountId);
                info.Balance += amount;

                var newTransaction = new Transactions();
                newTransaction.Id = Guid.NewGuid().ToString();
                newTransaction.BankId = bankId;
                newTransaction.AccountId = accountId;
                newTransaction.Amount = amount;
                newTransaction.OnTime = today;
                newTransaction.TransactionType = TransactionType.Credit;

                AddTransaction(newTransaction);
                return newTransaction;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Transactions Withdraw(string bankId, string accountId, decimal amount)
        {
            try
            {
                var info = accountService.GetAccount(bankId, accountId);
                if (info.Balance <= amount)
                    throw new Exception("Insufficient funds");

                info.Balance -= amount;

                var newTransaction = new Transactions();
                newTransaction.Id = Guid.NewGuid().ToString();
                newTransaction.BankId = bankId;
                newTransaction.AccountId = accountId;
                newTransaction.Amount = amount;
                newTransaction.OnTime = today;
                newTransaction.TransactionType = TransactionType.Debit;

                AddTransaction(newTransaction);
                return newTransaction;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}