using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Services;
using Technovert.BankApp.Models;

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
            bool response=service.Transfer(bankName,accountName, amount, beneficiaryBankName, beneficiaryName);
            if (response)
            {
                Printer printer = new Printer();
                printer.ResponsePrinter("Transfer");
            }
            else
            {
                Console.WriteLine("\n");
            }
        }
    }
}
