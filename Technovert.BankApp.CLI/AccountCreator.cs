using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Models;
using Technovert.BankApp.Services;

namespace Technovert.BankApp.CLI
{
    public class AccountCreator
    {
        public void Create(string bankName,BankService service)
        {
            string accountName, password;
            Console.Write("Enter your Name : ");
            accountName = Console.ReadLine();
            Console.Write("To setup your account, Please enter a password : ");
            password = Console.ReadLine();
            service.CreateAccount(bankName,accountName,password);// Account Created
            Console.WriteLine("\nAccount Created".ToUpper());
            LoginOptions options = new LoginOptions();
            string choosenOption = options.AvailableOptions(); // Displays the Main menu
            LoginOptionsChooser choice = new LoginOptionsChooser();
            choice.Choice(bankName,accountName, choosenOption, service); // Retrieves the users choice
        }
    }
}
