using System.Collections.Generic;

namespace Technovert.BankApp.Models
{
    public class StaffAccount
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Password { get; set; }
        public List<Currency> Currencies { get; set; }
    }
}
