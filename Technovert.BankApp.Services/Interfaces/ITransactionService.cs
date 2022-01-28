using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Models;

namespace Technovert.BankApp.Services.Interfaces
{
    public interface ITransactionService
    {
        public Transactions GetTransaction(string id);
        public Transactions AddTransaction(Transactions transaction);
        public Transactions UpdateTransaction(Transactions transaction);
        public Transactions DeleteTransaction(Transactions transaction);
        public List<Transactions> GetAllTransactions(string bankId, string accountId);
        public Transactions Deposit(string bankId, string accountId, decimal amount);
        public Transactions Withdraw(string bankId, string accountId, decimal amount);
    }
}
