
using System.ComponentModel.DataAnnotations;

namespace Technovert.BankApp.Models
{
    public class Currency
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal InverseRate { get; set; }
    }
}
