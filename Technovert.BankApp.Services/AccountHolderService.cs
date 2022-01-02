using System;
using System.Collections.Generic;
using System.Linq;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Enums;
using System.Data.SqlClient;

namespace Technovert.BankApp.Services
{
    // All the services available for account holders
    public class AccountHolderService
    {
        private BankService bankService;
        private HashingService hashing = new HashingService();
        private BankDbContext DbContext;
        public AccountHolderService(BankDbContext dbContext,BankService bankService)
        {
            this.bankService = bankService;
            this.DbContext = dbContext;
        }

        public void InputValidator(params string[] inputs) // Validates the user input
        {
            foreach (var input in inputs)
            {
                if (String.IsNullOrWhiteSpace(input))
                {
                    throw new Exception("Invalid Input!");
                }
            }
        }

        
        public void AccountLogin(string bankId, string accountId, string password) // Logs the user into his account by checking with the available id and password
        {
            InputValidator(bankId, accountId);
            try
            {
                var info = DbContext.Accounts.SingleOrDefault(m => m.Id == accountId && m.BankId == bankId);
                if (info == null)
                {
                    throw new Exception("Invalid Details");
                }
                if (info.AccountStatus == "Closed")
                    throw new Exception("Account Has Been Closed! Please Contact Bank Staff.");
                if (info.Password != password)
                    throw new Exception("Invalid Password!");
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
    }
}
