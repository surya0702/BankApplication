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
        //DateTime today = DateTime.Today;
        public BankService(Data data)
        {
            this.data = data;
        }
        public void InputValidator(params string[] inputs)
        {
            foreach (var input in inputs)
            {
                if (String.IsNullOrWhiteSpace(input))
                {
                    throw new InvalidInputException();
                }
            }
        }
        public Bank BankFinder(string bankId)
        {
            return this.data.banks.SingleOrDefault(x => x.Id == bankId);
        }
        public Bank BankFinderByName(string bankName)
        {
            return this.data.banks.SingleOrDefault(x => x.Name == bankName);
        }
       /* public string BankIdGenerator(string bankName)
        {
            return bankName.Substring(0, 3).ToUpper() + today.ToString("dd") + today.ToString("MM") + today.ToString("yyyy") ;
        }*/
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
