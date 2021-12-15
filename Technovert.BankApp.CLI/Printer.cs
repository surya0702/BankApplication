using System;
using System.Collections.Generic;
using System.Text;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Enums;
using ConsoleTables;
using System.Linq;

namespace Technovert.BankApp
{
    public class Printer
    {
        // Prints the users output string into Console
        public void ResponsePrinter(string description)
        {
            string line = new string('-', description.Length + 2);
            Console.WriteLine("\n|" + line + "|");
            Console.WriteLine("| " + description + " |");
            Console.WriteLine("|" + line + "|\n");
        }

        // Prints the Transactions in the form of a table in console.
        public string TablePrinter(List<Transaction> transactions,bool revert=false)
        {
            List<string> transactionToBeRevert = new List<string>();
            int index = 1;

            foreach(var transaction in transactions)
            {
                if (revert == true && transaction.TaxType == null)
                    continue;

                Console.WriteLine(index+" | "+transaction.Id + " | " + transaction.DestinationBankId + " | " + transaction.DestinationAccountId + " | " + transaction.Amount + " | " + transaction.TaxAmount + " | " + transaction.TransactionType + " | " + transaction.TaxType + " | " + transaction.OnTime) ;

                transactionToBeRevert.Add(transaction.Id);
                index += 1;
            }

            if (revert == false)
            {
                return " ";
            }
            Console.Write("\nEnter the S.No Corresponding to the Transaction Id which you would like to Revert : ");
            int Sno = Convert.ToInt32(Console.ReadLine());
            try
            {
                return transactionToBeRevert[Sno-1];
            }
            catch
            {
                Console.WriteLine("Enter a valid Serial Number");
                return "";
            }
        }
    }
}
