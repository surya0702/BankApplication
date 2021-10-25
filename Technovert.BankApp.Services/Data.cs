using System;
using System.Collections.Generic;
using System.Linq;
using Technovert.BankApp.Models;

namespace Technovert.BankApp.Services
{
    public class Data
    {
        public List<Bank> banks;
        public Data()
        {
            this.banks = new List<Bank>();
        }
    }
}
