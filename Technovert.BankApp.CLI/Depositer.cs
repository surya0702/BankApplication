using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Services;
using Technovert.BankApp.Models.Exceptions;

namespace Technovert.BankApp.CLI
{
    public class Depositer
    {
        public void Deposite(string bankId,string accountId,BankService service)
        {
            decimal amount;
            Console.Write("Please Enter the amount to be Deposited : ");
            amount = Convert.ToDecimal(Console.ReadLine());
            try
            {
                service.Deposit(bankId, accountId, amount); // Deposited the amount into user's Account
                Printer printer = new Printer();
                printer.ResponsePrinter("Deposit");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
