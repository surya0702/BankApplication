using System;
using System.Collections.Generic;
using System.Linq;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Exceptions;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.Services
{
    // All the services available for account holders
    public class AccountHolderService
    {
        private BankService bankService;
        public AccountHolderService(BankService bankService)
        {
            this.bankService = bankService;
        }

        public void InputValidator(params string[] inputs) // Validates the user input
        {
            foreach (var input in inputs)
            {
                if (String.IsNullOrWhiteSpace(input))
                {
                    throw new InvalidInputException();
                }
            }
        }

        public Account AccountFinder(string bankId, string accountId) // Returns the user account based on bankid and accountid
        {
            Bank bank = this.bankService.BankFinder(bankId);
            return bank.Accounts.SingleOrDefault(x => x.Id == accountId);
        }
        
        public void AccountLogin(string bankId, string accountHolderId, string password) // Logs the user into his account by checking with the available id and password
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
        
    }
}
