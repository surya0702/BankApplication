using System.Collections.Generic;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.Models
{
    // Properties available for Banks
    public class Bank
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public List<Account> Accounts { get; set; }
    }
}
