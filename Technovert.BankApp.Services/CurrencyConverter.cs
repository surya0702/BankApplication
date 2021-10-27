using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Technovert.BankApp.Models;

namespace Technovert.BankApp.Services
{
    // Services available currencies
    public class CurrencyConverter
    {
        private Data data;
        public CurrencyConverter(Data data)
        {
            this.data = data;
            CurrencyExchange();  // Adds the default accepted currencies into the Application
        }

        public decimal Converter(decimal amount,decimal exchangeRate) // Converts the amount into INR using exchange rate
        {
            decimal actualAmount = amount * exchangeRate;
            return actualAmount;
        }

        public void CurrencyExchange() // Adds new currencies into the data
        {
            try
            {
                string url = "http://www.floatrates.com/daily/inr.json";
                string json = new WebClient().DownloadString(url);
                var currencies = JsonConvert.DeserializeObject<Dictionary<string, Currency>>(json);
                foreach(var currency in currencies)
                {
                    this.data.currencies.Add(currency.Value);
                }
            }
            catch { }
        }

    }
}