using System;
using System.Collections.Generic;
using System.Timers;
using System.Text;

namespace Technovert.BankApp.Models
{
    public class Account
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
