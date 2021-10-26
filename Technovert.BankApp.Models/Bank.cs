using System.Collections.Generic;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.Models
{
    public class Bank
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public List<Account> Accounts { get; set; }
        public IMPSCharges Charges { get; set; }
    }
}
