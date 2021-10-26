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
                var currency = JsonConvert.DeserializeObject<dynamic>(json);
                this.data.currencies.Add(new Currency()
                {
                    code = currency.usd.code,
                    name = currency.usd.name,
                    exchangeRate = currency.usd.inverseRate
                });
                this.data.currencies.Add(new Currency()
                {
                    code = currency.eur.code,
                    name = currency.eur.name,
                    exchangeRate = currency.eur.inverseRate
                });
            }
            catch { }
        }

    }
}