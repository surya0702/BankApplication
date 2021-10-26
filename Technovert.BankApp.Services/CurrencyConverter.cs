using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using System.Net.Http;
using Newtonsoft.Json;

namespace Technovert.BankApp.Services
{
    public class Item
    {
        public string code;
        public string name;
        public decimal rate;
    }
    public class CurrencyConverter
    {
        public decimal Converter(decimal amount,decimal exchangeRate)
        {
            decimal actualAmount = amount * exchangeRate;
            return actualAmount;
        }
    }
}