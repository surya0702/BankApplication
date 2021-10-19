using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Services;
using Technovert.BankApp.Models;

namespace Technovert.BankApp.CLI
{
    public class Depositer
    {
        public void Deposite(string bankName,string accountName,BankService service)
        {
            decimal amount;
            Console.Write("Please Enter the amount to be Deposited : ");
            amount = Convert.ToDecimal(Console.ReadLine());
            bool response=service.Deposit(bankName,accountName, amount); // Deposited the amount into user's Account
            if (response)
            {
                Printer printer = new Printer();
                printer.ResponsePrinter("Deposit");
            }
            else
            {
                Console.WriteLine("\nInvalid Details!");
            }
        }
    }
}
