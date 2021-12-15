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
    // Transfers the amount from user account to beneficiary account
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
            string[] taxTypes = { "IMPS", "RTGS" };
            LoginOptions options = new LoginOptions();
            string userTaxType=options.AvailableOptions(taxTypes);
            try
            {
                transactionService.Transfer(bankId, accountId, amount, beneficiaryBankId, beneficiaryAccountId,userTaxType);
                Printer printer = new Printer();
                printer.ResponsePrinter(String.Format("{0:n}", amount) + " INR has been Transfered Successfully");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
