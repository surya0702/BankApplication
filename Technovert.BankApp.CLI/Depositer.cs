using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Services;
using Technovert.BankApp.Models.Exceptions;

namespace Technovert.BankApp.CLI
{
    // Deposits the amount into users Account
    public class Depositer
    {
        public void Deposite(string bankId,string accountId,TransactionService transactionService)
        {
            string code; decimal amount;
            Console.Write("Enter the Currency Code : ");
            code = Console.ReadLine();
            Console.Write("Please Enter the amount to be Deposited : ");
            amount = Convert.ToDecimal(Console.ReadLine());
            try
            {
                transactionService.Deposit(bankId, accountId, amount,code); // Deposited the amount into user's Account
                Printer printer = new Printer();
                printer.ResponsePrinter(String.Format("{0:n}",amount)+" "+code+" has been Deposited Successfully");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
