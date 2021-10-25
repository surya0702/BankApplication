using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technovert.BankApp.Models.Enums
{
    public class TransactionCharges
    {
        public decimal SameBankRTGS = 0;
        public decimal SameBankIMPS = 5;

        public decimal DifferentBankRTGS = 2;
        public decimal DifferentBankIMPS = 6;
    }
}
