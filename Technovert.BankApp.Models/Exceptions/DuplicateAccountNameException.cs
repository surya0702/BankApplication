﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technovert.BankApp.Models.Exceptions
{
    public class DuplicateAccountNameException : Exception
    {
        public override string Message
        {
            get
            {
                return "Account Name already taken! Enter another Account name ";
            }
        }
    }
}