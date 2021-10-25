using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technovert.BankApp.Models.Exceptions
{
    public class InsufficientTransactions : Exception
    {
        public override string Message
        {
            get
            {
                return "There are no Transactions left to Undo!";
            }
        }
    }
}
