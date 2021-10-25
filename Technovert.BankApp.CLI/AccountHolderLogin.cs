using System;
using Technovert.BankApp.Services;

namespace Technovert.BankApp.CLI
{
    public class AccountHolderLogin
    {
        public void Login(BankService bankService,AccountHolderService accountHolderService,TransactionService transactionService)
        {
            string accountId, bankId, password;
            Console.Write("Enter your BankId : ");
            bankId = Console.ReadLine();
            try
            {
                bankService.BankLogin(bankId);

                Console.Write("Enter your Account ID : ");
                accountId = Console.ReadLine();
                Console.Write("Enter your Password : ");
                password = Console.ReadLine();

                accountHolderService.AccountLogin(bankId, accountId, password);

                string[] options = { "Deposit", "Withdraw", "Transfer", "Transaction History", "LogOut" };
                LoginOptions loginOptions = new LoginOptions();
                string userOption = loginOptions.AvailableOptions(options);
                AccountHolderChoice optionsChooser = new AccountHolderChoice();
                optionsChooser.Choice(bankId, accountId, userOption, transactionService, options);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}