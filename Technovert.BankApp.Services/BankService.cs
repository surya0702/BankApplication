using System;
using System.Collections.Generic;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Exceptions;
using Technovert.BankApp.Models.Enums;
using System.Linq;

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
            Bank currentBank = BankFinder(bankId);
            if (currentBank == null)
            {
                throw new InvalidBankNameException();
            }
        }
    }
}
