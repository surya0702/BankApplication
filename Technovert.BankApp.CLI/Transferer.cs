using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Services;
using Technovert.BankApp.Models.Exceptions;

namespace Technovert.BankApp.CLI
{
    public class Transferer
    {
        public void Transfer(string bankId,string accountId,BankService service)
        {
            decimal amount;
            string beneficiaryBankId,beneficiaryAccountId;
            Console.Write("Enter the Beneficiary's Bank Id : ");
            beneficiaryBankId = Console.ReadLine();
            Console.Write("Enter the Beneficiary Account Id : ");
            beneficiaryAccountId = Console.ReadLine();
            Console.Write("Enter the amount to be transfered : ");
            amount = Convert.ToInt32(Console.ReadLine());
            try
            {
                service.Transfer(bankId, accountId, amount, beneficiaryBankId, beneficiaryAccountId);
                Printer printer = new Printer();
                printer.ResponsePrinter("Transfer");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
