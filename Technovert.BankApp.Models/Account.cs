using System.Collections.Generic;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.Models
{
    // Class to represent the properties available for Account holders
    public class Account
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
