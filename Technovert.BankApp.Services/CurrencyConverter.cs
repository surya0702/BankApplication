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
using System.Data.SqlClient;

namespace Technovert.BankApp.Services
{
    // Services available currencies
    public class CurrencyConverter
    {
        private BankDbContext DbContext;
        public CurrencyConverter(BankDbContext DbContext)
        {
            this.DbContext = DbContext;
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
                int limit = 10;
                int currencyCounter = 0;
                foreach (var currency in currencies)
                {
                    if (currencyCounter == limit)
                    {
                        break;
                    }
                    var newCurrency = new Currency()
                    {
                        Code = currency.Value.Code,
                        Name = currency.Value.Name,
                        InverseRate = currency.Value.InverseRate
                    };
                    DbContext.Currencies.Add(newCurrency);

                    currencyCounter += 1;
                }
                DbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}