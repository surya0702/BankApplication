using System;
using System.Collections.Generic;
using System.Text;
using Technovert.BankApp.Models;

namespace Technovert.BankApp
{
    public class Printer
    {
        public void ResponsePrinter(string description)
        {
            string line = new string('-', description.Length + 2);
            Console.WriteLine("\n|" + line + "|");
            Console.WriteLine("| " + description + " |");
            Console.WriteLine("|" + line + "|\n");
        }
        public void TablePrinter(List<Transaction> transactions)
        {
            string line = new string('-', 158);
            Console.WriteLine(line);
            Console.WriteLine($"|{"Transaction Id",40}|{"Amount",8}|{"SourceBankId",17}|{"SourceAccountId",20}|{"DestinationBankId",21}|{"DestinationAccountId",22}|{"On",22}|");
            foreach (var d in transactions)
            {

                Console.WriteLine($"|{d.Id,40}|{d.Amount,8}|{d.SourceBankId,17}|{d.SourceAccountId,20}|{d.DestinationBankId,21}|{d.DestinationAccountId,22}|{d.On,22}|");
            }
            Console.WriteLine(line);
        }
    }
}
