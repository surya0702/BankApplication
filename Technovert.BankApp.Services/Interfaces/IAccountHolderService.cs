using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Models;

namespace Technovert.BankApp.Services.Interfaces
{
    public interface IAccountHolderService
    {
        public string Authenticate(string bankId,string id,string password);
        public Account CreateAccount(Account account);
        public Account UpdateAccount(Account account);
        public Account CloseAccount(string bankId, string accountId);
        public Account GetAccount(string bankId, string accountId);
        public List<Account> GetAllAccounts(string bankId);
    }
}
