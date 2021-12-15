using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Technovert.BankApp.Models
{
    // Properties available for Staff Accounts
    public class StaffAccount
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
