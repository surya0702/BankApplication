using System;
using Technovert.BankApp.Services;

namespace Technovert.BankApp.CLI
{
    public class StaffLogin
    {
        public void Login(BankService bankService, AccountHolderService accountHolderService,
            BankStaffService bankStaffService,TransactionService transactionService)
        {
            string id, password;
            Console.Write("Enter your Staff ID : ");
            id = Console.ReadLine();
            Console.Write("Enter your Password : ");
            password = Console.ReadLine();
            try
            {
                bankStaffService.Login(bankService, accountHolderService, transactionService, id, password);
                string[] options = { "Create new Account", "Update Account", "Delete Account","View Transaction History",
                    "Undo Transaction", "Add new Currency","View Account Details", "LogOut" };
                LoginOptions loginOptions = new LoginOptions();
                string userOption = loginOptions.AvailableOptions(options);
                BankStaffChoice bankStaffChoice = new BankStaffChoice();
                bankStaffChoice.Choice(id,userOption, options, bankService,accountHolderService,bankStaffService);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
