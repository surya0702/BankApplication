using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technovert.BankApp.Models.Enums
{
    public class TransactionCharges
    {
        public enum SameBank
        {
            RTGS = 0,
            IMPS = 5
        }
        public enum DifferentBank
        {
            RTGS = 2,
            IMPS = 6
        }
    }
}
