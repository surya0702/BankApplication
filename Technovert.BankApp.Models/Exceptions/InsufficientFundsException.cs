using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technovert.BankApp.Models.Exceptions
{
    public class InsufficientFundsException : Exception
    {
        public override string Message
        {
            get
            {
                return "Insufficient funds in your bank account!";
            }
        }
    }
}
