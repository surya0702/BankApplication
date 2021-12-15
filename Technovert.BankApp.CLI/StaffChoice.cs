using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Services;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Exceptions;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.CLI
{
    // Responds in-accordance to the Staff chosen Services.
    public class StaffChoice
    {
        public void Choice(string id,string userOption,string[] options, BankService bankService , AccountHolderService accountHolderService,
            StaffService bankStaffService,TransactionService transactionService)
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
                            int age=0;
                            Console.Write("Enter the new Account Holder Name : ");
                            name = Console.ReadLine();
                            Console.Write("Enter the Bank Id in which the Account should be Created : ");
                            bankName = Console.ReadLine();
                            Console.Write("Enter the Gender : ");
                            string gender = Console.ReadLine();
                            Console.Write("Enter the Age : ");
                            string tempAge = Console.ReadLine();
                            if (String.IsNullOrEmpty(tempAge) == false)
                            {
                                age = Convert.ToInt32(tempAge);
                            }
                            try
                            {
                                string[] response = bankStaffService.CreateAccount(name, bankName,age,gender);
                                string newId = response[0], password = response[1];
                                printer.ResponsePrinter("Credentials for newly Created Account are");
                                Console.WriteLine("\nAccount Id : " + newId);
                                Console.WriteLine("Password is : " + password);
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        }
                    case "Update Account Details":
                        {
                            string accountId, bankId;
                            Console.Write("Enter the Bank Id in which the account exists : ");
                            bankId = Console.ReadLine();
                            Console.Write("Enter the Account Id : ");
                            accountId = Console.ReadLine();
                            while (true) {
                                bool flag = false;
                                string[] updateOptions = { "Update Name", "Update Password", "Update Age", "Update Gender","BACK" };
                                LoginOptions login = new LoginOptions();
                                string updateOption = login.AvailableOptions(updateOptions);
                                string newName, newPassword ;
                                int newAge = 0;
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
                                            catch (Exception ex)
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
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                            break;
                                        }
                                    case "Update Age":
                                        {
                                            Console.Write("Enter the new Age : ");
                                            newAge = Convert.ToInt32(Console.ReadLine());
                                            try
                                            {
                                                bankStaffService.UpdateAccount(accountId, bankId, null, null, newAge);
                                                printer.ResponsePrinter("Age Updated");
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                            break;
                                        }
                                    case "Update Gender":
                                        {
                                            Console.Write("Enter the Gender : ");
                                            string gender = Console.ReadLine();
                                            try
                                            {
                                                bankStaffService.UpdateAccount(accountId, bankId, null, null, 0, gender);
                                                printer.ResponsePrinter("Gender Updated");
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex);
                                            }
                                            break;
                                        }
                                    case "BACK":
                                        {
                                            flag = true;
                                            break;
                                        }
                                }
                                if (flag)
                                {
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
                                List<Transaction> transactions = transactionService.TransactionHistory(bankId, accountId);
                                printer.ResponsePrinter("Transaction Log");
                                printer.TablePrinter(transactions);
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        }
                    case "Revert Transaction":
                        {
                            string accountId, bankId;
                            Console.Write("Enter the Bank Id in which the account exists : ");
                            bankId = Console.ReadLine();
                            Console.Write("Enter the Account Id : ");
                            accountId = Console.ReadLine();
                            try
                            {
                                List<Transaction> transactions = transactionService.TransactionHistory(bankId, accountId);
                                printer.ResponsePrinter("Transaction Log");
                                string transactionId = printer.TablePrinter(transactions, true);

                                bankStaffService.RevertTransaction(bankId, accountId,transactionId);
                                printer.ResponsePrinter("Transaction has been reversed");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
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
                            bankStaffService.AddNewCurrency(name, code, exchangeRate);
                            printer.ResponsePrinter("New Currency has been Added");
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
