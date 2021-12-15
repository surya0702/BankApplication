using System;
using Technovert.BankApp.Services;
using System.Data.SqlClient;

namespace Technovert.BankApp.CLI
{
    // used to login the Staff Members
    public class StaffLogin
    {
        public void Login(BankService bankService, AccountHolderService accountHolderService,
            StaffService bankStaffService,TransactionService transactionService)
        {
            string id, password;
            Console.Write("Enter your Staff ID : ");
            id = Console.ReadLine();
            Console.Write("Enter your Password : ");
            password = Console.ReadLine();
            try
            {
                //bankStaffService.Login(bankService, accountHolderService, transactionService, id, password);
                string[] options = { "Create new Account", "Update Account Details", "Delete Account","View Transaction History",
                    "Revert Transaction", "Add new Currency","LogOut" };
                LoginOptions loginOptions = new LoginOptions();
                string userOption = loginOptions.AvailableOptions(options);
                StaffChoice bankStaffChoice = new StaffChoice();
                bankStaffChoice.Choice(id,userOption, options, bankService,accountHolderService,bankStaffService,transactionService);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
