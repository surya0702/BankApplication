using System;
using System.Collections.Generic;
using System.Text;

namespace Technovert.BankApp.Models
{
    public class Bank
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public List<Account> Accounts { get; set; }
    }
}
