using System;
using System.Collections.Generic;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Exceptions;
using Technovert.BankApp.Models.Enums;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace Technovert.BankApp.Services
{
    public class BankService
    {
        private Data data;
        public BankService(Data data)
        {
            this.data = data;
        }
        public Bank BankFinder(string bankId)
        {
            return this.data.banks.SingleOrDefault(x => x.Id == bankId);
        }
        public void CreateBank(string bankName)
        {
            if (String.IsNullOrWhiteSpace(bankName))
            {
                throw new InvalidInputException();
            }
            Bank newBank = new Bank()
            {
                Name = bankName,
                Id = bankName,
                Accounts = new List<Account>()
            };
            this.data.banks.Add(newBank);
        }
        public void BankLogin(string bankId)
        {
            if (String.IsNullOrWhiteSpace(bankId))
            {
                throw new InvalidInputException();
            }
            Bank currentBank = BankFinder(bankId);
            if (currentBank == null)
            {
                throw new InvalidBankNameException();
            }
        }
    }
}
