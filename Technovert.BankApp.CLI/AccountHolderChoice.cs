using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Services;
using Technovert.BankApp.Models;

namespace Technovert.BankApp.CLI
{
    // Responds to the User Choice in Available Account Services.
    public class AccountHolderChoice
    {
        public void Choice(string bankId,string accountId, string userOption,TransactionService transactionService,string[] options)
        {
            Printer printer = new Printer();
            while (true)
            {
                bool stop = false;
                switch (userOption)
                {
                    case "Deposit":
                        {
                            Depositer depositer = new Depositer();
                            depositer.Deposite(bankId,accountId, transactionService);
                            break;
                        }
                    case "Withdraw":
                        {
                            Withdrawer withdrawer = new Withdrawer();
                            withdrawer.Withdraw(bankId,accountId,transactionService);
                            break;
                        }
                    case "Transfer":
                        {
                            Transferer transferer = new Transferer();
                            transferer.Transfer(bankId,accountId,transactionService);
                            break;
                        }
                    case "Transaction History":
                        {
                            try
                            {
                                List<Transaction> transactions = transactionService.TransactionHistory(bankId, accountId);
                                printer.ResponsePrinter("Transaction Log");
                                printer.TablePrinter(transactions);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        }
                    case "View Balance":
                        {
                            try
                            {
                                decimal balance=transactionService.ViewBalance(bankId, accountId);
                                printer.ResponsePrinter("Available Balance is : " + balance);
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
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
                    userOption=option.AvailableOptions(options);
                }
            }
        }
    }
}
