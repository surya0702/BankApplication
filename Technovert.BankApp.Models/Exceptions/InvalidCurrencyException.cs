using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technovert.BankApp.Models.Exceptions
{
    public class InvalidCurrencyException : Exception
    {
        public override string Message
        {
            get
            {
                return "Currency not available in the Bank.Please contact the Bank Staff!";
            }
        }
    }
}
