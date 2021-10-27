using System;
using System.Collections.Generic;
using System.Text;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Enums;

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
            int SerialNO=1;
            Dictionary<int, string> transactionSNO = new Dictionary<int, string>();
            string line = new string('-', 160);
            Console.WriteLine(line);
            Console.WriteLine($"|{"S.No",6}|{"Transaction Id",40}|{"Amount",8}|{"Transaction Type",18}|{"Tax Type",10}|{"Tax",6}|{"SourceAccountId",20}|{"DestinationAccountId",22}|{"On",22}|");
            Console.WriteLine(line);
            foreach (var transaction in transactions)
            {
                if (transaction.TaxType==TaxType.None && revert==true && transaction.Tax==0)
                {
                    continue;
                }
                transactionSNO.Add(SerialNO,transaction.Id);
                Console.WriteLine($"|{SerialNO,6}|{transaction.Id,40}|{transaction.Amount,8}|{transaction.TransactionType,18}|{transaction.TaxType,10}|{transaction.Tax,6}|{transaction.SourceAccountId,20}|{transaction.DestinationAccountId,22}|{transaction.On,22}|");
                SerialNO += 1;
            }
            Console.WriteLine(line);
            if (revert == false)
            {
                return "";
            }
            Console.Write("\nEnter the S.No Corresponding to the Transaction Id which you would like to Revert : ");
            int Sno = Convert.ToInt32(Console.ReadLine());
            try
            {
                return transactionSNO[Sno];
            }
            catch
            {
                Console.WriteLine("Enter a valid Serial Number");
                return "";
            }
        }
    }
}
