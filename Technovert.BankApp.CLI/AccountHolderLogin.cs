using System;
using Technovert.BankApp.Services;
using System.Data.SqlClient;

namespace Technovert.BankApp.CLI
{
    // Used for login purpose by Account holders.
    public class AccountHolderLogin
    {
        public void Login(BankService bankService,AccountHolderService accountHolderService,TransactionService transactionService)
        {
            string accountId, bankId, password;
            try
            {
                Console.Write("Enter your BankId : ");
                bankId = Console.ReadLine();
                Console.Write("Enter your Account ID : ");
                accountId = Console.ReadLine();
                Console.Write("Enter your Password : ");
                password = Console.ReadLine();

                accountHolderService.AccountLogin(bankId, accountId, password);

                string[] options = { "Deposit", "Withdraw", "Transfer", "Transaction History", "View Balance","LogOut" };
                
                LoginOptions loginOptions = new LoginOptions();
                string userOption = loginOptions.AvailableOptions(options);
                AccountHolderChoice optionsChooser = new AccountHolderChoice();
                optionsChooser.Choice(bankId, accountId, userOption, transactionService, options);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}