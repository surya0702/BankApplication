using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Models.Exceptions;
using Technovert.BankApp.Services;

namespace Technovert.BankApp.CLI
{
    public class AccountCreator
    {
        public void Create(string bankId,BankService service)
        {
            string accountName, password, accountId;
            while (true)
            {
                Console.Write("Enter your Name : ");
                accountName = Console.ReadLine();
                if (accountName.Length >= 3)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Name should contain at least 3 Characters");
                }
            }
            Console.Write("To setup your account, Please enter a password : ");
            password = Console.ReadLine();
            try
            {
                accountId=service.CreateAccount(bankId, accountName, password);// Account Created
                Printer printer = new Printer();
                printer.ResponsePrinter("Account Created Successfully! Your Account Id is : " + accountId);
                LoginOptions options = new LoginOptions();
                string choosenOption = options.AvailableOptions(); // Displays the Main menu
                LoginOptionsChooser choice = new LoginOptionsChooser();
                choice.Choice(bankId, accountName, choosenOption, service); // Retrieves the users choice
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
