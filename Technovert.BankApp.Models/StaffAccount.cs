using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technovert.BankApp.Models
{
    public class StaffAccount : Account
    {
        public List<Currency> Currencies { get; set; }
    }
}
