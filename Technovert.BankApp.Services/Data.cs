using System;
using System.Collections.Generic;
using System.Linq;
using Technovert.BankApp.Models;

namespace Technovert.BankApp.Services
{
    public class Data
    {
        public List<Bank> banks;
        public List<Currency> currencies;
        public Data()
        {
            this.banks = new List<Bank>();
            this.currencies = new List<Currency>();
            this.currencies.Add(new Currency() { code = "INR", name = "Rupee", exchangeRate = 1 });
        }
    }
}
