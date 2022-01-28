using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.Models
{
    // Class to represent the properties available for Account holders
    public class Account
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string BankId { get; set; }
        public Status AccountStatus { get; set; }
        public string Role { get; set; }
    }
}
