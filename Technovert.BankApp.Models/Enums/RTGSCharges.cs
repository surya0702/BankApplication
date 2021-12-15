using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technovert.BankApp.Models.Enums
{
    // Transaction charges based on RTGS
    public enum RTGSCharges
    {
        SameBank = 0,
        DifferentBank = 2
    }
}
