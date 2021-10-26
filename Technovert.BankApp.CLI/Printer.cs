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
        public string TablePrinter(List<Transaction> transactions,bool revert=false)
        {
            int SerialNO=1;
            Dictionary<int, string> transactionSNO = new Dictionary<int, string>();
            string line = new string('-', 158);
            Console.WriteLine(line);
            Console.WriteLine($"|{"Transaction Id",40}|{"Amount",8}|{"Transaction Type",18}|{"Tax Type",10}|{"Tax",6}|{"SourceAccountId",20}|{"DestinationAccountId",22}|{"On",22}|");
            Console.WriteLine(line);
            foreach (var d in transactions)
            {
                if (d.TaxType==TaxType.None && revert==true)
                {
                    continue;
                }
                transactionSNO.Add(SerialNO,d.Id);
                Console.WriteLine($"|{d.Id,40}|{d.Amount,8}|{d.TransactionType,18}|{d.TaxType,10}|{d.Tax,6}|{d.SourceAccountId,20}|{d.DestinationAccountId,22}|{d.On,22}|");
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
