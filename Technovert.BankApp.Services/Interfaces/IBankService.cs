using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Models;

namespace Technovert.BankApp.Services.Interfaces
{
    public interface IBankService
    {
        public Bank CreateBank(Bank bank);
        public Bank UpdateBank(Bank bank);
        public Bank CloseBank(string bankId);
        public Bank GetBank(string bankId);
        public List<Bank> GetAllBanks();
    }
}
