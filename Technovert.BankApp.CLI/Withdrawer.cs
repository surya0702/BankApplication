using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Services;
using Technovert.BankApp.Models;

namespace Technovert.BankApp.CLI
{
    // used to withdraw the amount from users account
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
                printer.ResponsePrinter(String.Format("{0:n}", amount) + " INR has been Withdrawn Successfully");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}