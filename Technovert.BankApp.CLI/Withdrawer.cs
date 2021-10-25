using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Services;
using Technovert.BankApp.Models;

namespace Technovert.BankApp.CLI
{
    public class Withdrawer
    {
        public void Withdraw(string bankId,string accountId,TransactionService transactionService)
        {
            decimal amount;
            Console.Write("Please Enter the amount to be Withdrawn in INR : ");
            amount = Convert.ToDecimal(Console.ReadLine());
            try
            {
                transactionService.Withdraw(bankId, accountId, amount);
                Printer printer = new Printer();
                printer.ResponsePrinter("Withdraw");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}