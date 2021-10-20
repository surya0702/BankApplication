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
        public void Transfer(string bankName,string accountName,BankService service)
        {
            decimal amount;
            string beneficiaryBankName,beneficiaryName;
            Console.Write("Enter the Beneficiary's Bank Name : ");
            beneficiaryBankName = Console.ReadLine();
            Console.Write("Enter the Beneficiary Name : ");
            beneficiaryName = Console.ReadLine();
            Console.Write("Enter the amount to be transfered : ");
            amount = Convert.ToInt32(Console.ReadLine());
            try
            {
                service.Transfer(bankName, accountName, amount, beneficiaryBankName, beneficiaryName);
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
