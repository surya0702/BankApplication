using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Models;
using Technovert.BankApp.Services;

namespace Technovert.BankApp.CLI
{
    public class AccountLogin
    {
        public void Logger(string bankName,BankService service)
        {
            string accountName, password;
            Console.Write("Enter your Account name: ");
            accountName = Console.ReadLine();
            Console.Write("Enter your Password : ");
            password = Console.ReadLine();
            bool response = service.AccountLogin(bankName,accountName, password);
            if (response)
            {
                Printer printer = new Printer();
                printer.ResponsePrinter("");
                LoginOptions options = new LoginOptions(); 
                string choosenOption = options.AvailableOptions();
                LoginOptionsChooser choice = new LoginOptionsChooser();
                choice.Choice(bankName,accountName, choosenOption, service);
            }
            else
            {
                Console.WriteLine("\nInvalid Details");
            }
        }
    }
}
