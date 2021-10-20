using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technovert.BankApp.Models.Exceptions
{
    public class InvalidAccountPasswordException : Exception
    {
        public override string Message
        {
            get
            {
                return "User Password was not matching with User Id";
            }
        }
    }
}
