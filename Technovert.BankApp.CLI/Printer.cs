using System;
using System.Collections.Generic;
using System.Text;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Enums;

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
        public string TablePrinter(List<Transaction> transactions)
        {
            int SerialNO=1;
            Dictionary<string, string> transactionSNO = new Dictionary<string, string>();
            string line = new string('-', 158);
            Console.WriteLine(line);
            Console.WriteLine($"|{"Transaction Id",40}|{"Amount",8}|{"Transaction Type",18}|{"Tax Type",10}|{"Tax",6}|{"SourceBankId",17}|{"SourceAccountId",20}|{"DestinationBankId",21}|{"DestinationAccountId",22}|{"On",22}|");
            Console.WriteLine(line);
            foreach (var d in transactions)
            {
                if (d.Tax==0)
                {
                    continue;
                }
                transactionSNO.Add(SerialNO.ToString(),d.Id);
                Console.WriteLine($"|{d.Id,40}|{d.Amount,8}|{d.TransactionType,18}|{d.TaxType,10}|{d.Tax,6}|{d.SourceBankId,17}|{d.SourceAccountId,20}|{d.DestinationBankId,21}|{d.DestinationAccountId,22}|{d.On,22}|");
            }
            Console.WriteLine(line);
            Console.Write("\nEnter the S.No Corresponding to the Transaction Id which you would like to Revert : ");
            string Sno = Console.ReadLine();
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
