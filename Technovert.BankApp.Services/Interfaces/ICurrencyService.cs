using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technovert.BankApp.Services.Interfaces
{
    public interface ICurrencyService
    {
        public decimal Converter(decimal amount, decimal exchangeRate);
        public void CurrencyExchange();
    }
}
