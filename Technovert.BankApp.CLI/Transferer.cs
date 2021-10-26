using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Services;
using Technovert.BankApp.Models.Exceptions;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.CLI
{
    public class Transferer
    {
        public void Transfer(string bankId,string accountId,TransactionService transactionService)
        {
            decimal amount;
            string beneficiaryBankId,beneficiaryAccountId;
            Console.Write("Enter the Beneficiary's Bank Id : ");
            beneficiaryBankId = Console.ReadLine();
            Console.Write("Enter the Beneficiary Account Id : ");
            beneficiaryAccountId = Console.ReadLine();
            Console.Write("Enter the amount to be transfered in INR : ");
            amount = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Available Tax Charges for Transaction are : ");
            Console.WriteLine("\n1. IMPS\n2. RTGS");
            Console.Write("\nEnter the Tax Type in which you would like to transfer the money : ");
            string tax = Console.ReadLine();
            TaxType taxType = (TaxType)Convert.ToInt32(tax);
            try
            {
                transactionService.Transfer(bankId, accountId, amount, beneficiaryBankId, beneficiaryAccountId,taxType);
                Printer printer = new Printer();
                printer.ResponsePrinter("Transfer Completed");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
