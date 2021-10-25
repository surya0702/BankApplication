using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Services;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Exceptions;

namespace Technovert.BankApp.CLI
{
    public class BankStaffChoice
    {
        public void Choice(string id,string userOption,string[] options, BankService bankService , AccountHolderService accountHolderService,BankStaffService bankStaffService)
        {
            Printer printer = new Printer();
            while (true)
            {
                bool stop = false;
                switch (userOption)
                {
                    case "Create new Account":
                        {
                            string name,bankName;
                            Console.Write("Enter the new Account Holder Name : ");
                            name = Console.ReadLine();
                            Console.Write("Enter the Name of the bank in which the account holder wants to Create an account : ");
                            bankName = Console.ReadLine();
                            string[] response=bankStaffService.CreateAccount(name,bankName);
                            string newId = response[0], password = response[1];
                            printer.ResponsePrinter("Credentials for newly Created Account are");
                            Console.WriteLine("\nAccount Id : "+ newId);
                            Console.WriteLine("Password is : " + password);
                            break;
                        }
                    case "Update Account":
                        {
                            string accountId, bankId;
                            Console.Write("Enter the Bank Id in which the account exists : ");
                            bankId = Console.ReadLine();
                            Console.Write("Enter the Account Id : ");
                            accountId = Console.ReadLine();
                            string[] updateOptions = { "Update Name", "Update Password" };
                            LoginOptions login = new LoginOptions();
                            string updateOption = login.AvailableOptions(updateOptions);
                            string newName, newPassword;
                            switch (updateOption)
                            {
                                case "Update Name":
                                    {
                                        Console.Write("Enter the new Name : ");
                                        newName = Console.ReadLine();
                                        try
                                        {
                                            bankStaffService.UpdateAccount(accountId, bankId, newName);
                                            printer.ResponsePrinter("Name Updated");
                                        }
                                        catch(Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                        break;
                                    }
                                case "Update Password":

                                    {
                                        Console.Write("Enter the new Password : ");
                                        newPassword = Console.ReadLine();
                                        try
                                        {
                                            bankStaffService.UpdateAccount(accountId, bankId, null, newPassword);
                                            printer.ResponsePrinter("Password Updated");
                                        }
                                        catch(Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                        break;
                                    }
                            }
                            break;
                        }
                    case "Delete Account":
                        {
                            string accountId, bankId;
                            Console.Write("Enter the Bank Id in which the account exists : ");
                            bankId = Console.ReadLine();
                            Console.Write("Enter the Account Id : ");
                            accountId = Console.ReadLine();
                            try
                            {
                                bankStaffService.DeleteAccount(accountId, bankId);
                                printer.ResponsePrinter("Account has been deleted");
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        }
                    case "View Transaction History":
                        {
                            string accountId, bankId;
                            Console.Write("Enter the Bank Id in which the account exists : ");
                            bankId = Console.ReadLine();
                            Console.Write("Enter the Account Id : ");
                            accountId = Console.ReadLine();
                            try
                            {
                                List<Transaction> transactions = bankStaffService.ViewTransactions(bankId, accountId);
                                printer.ResponsePrinter("Transaction Log");
                                foreach (var d in transactions)
                                {
                                    Console.WriteLine(d.Id + " " + d.SourceBankId + " " + d.SourceAccountId + " " + d.DestinationBankId + " " + d.DestinationAccountId + " " + d.Amount + " " + d.On + " " + d.Type);
                                }
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        }
                    case "Undo Transaction":
                        {
                            string accountId, bankId;
                            Console.Write("Enter the Bank Id in which the account exists : ");
                            bankId = Console.ReadLine();
                            Console.Write("Enter the Account Id : ");
                            accountId = Console.ReadLine();
                            try
                            {
                                bankStaffService.UndoTransaction(bankId, accountId);
                                printer.ResponsePrinter("Transaction has been reversed");
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        }
                    case "Add new Currency":
                        {
                            string name, code;
                            decimal exchangeRate;
                            Console.Write("Enter the name of new Currency : ");
                            name = Console.ReadLine();
                            Console.Write("Enter the new Currency code : ");
                            code = Console.ReadLine();
                            Console.Write("Enter the Exchange rate with respect to INR : ");
                            exchangeRate = Convert.ToInt32(Console.ReadLine());
                            bankStaffService.AddNewCurrency(id,name, code, exchangeRate);
                            printer.ResponsePrinter("New Currency has been Added");
                            break;
                        }
                    case "View Account Details":
                        {
                            string accountId, password, bankId;
                            Console.Write("Enter the Bank Id in which the account exists : ");
                            bankId = Console.ReadLine();
                            Console.Write("Enter the Account Id : ");
                            accountId = Console.ReadLine();
                            Account account=bankStaffService.ViewAccountDetails(bankId, accountId);
                            Console.WriteLine("Name : " + account.Name);
                            Console.WriteLine("Id : " + account.Id);
                            Console.WriteLine("Password : " + account.Password);
                            Console.WriteLine("Balance : " + account.Balance);
                            break;
                        }
                    case "LogOut":
                        {
                            printer.ResponsePrinter("Logged Out");
                            stop = true;
                            break;
                        }
                }
                if (stop)
                {
                    break;
                }
                else
                { // Displays the options until the user wants to LogOut from the Bank
                    LoginOptions option = new LoginOptions();
                    userOption = option.AvailableOptions(options);
                }
            }
        }
    }
}
