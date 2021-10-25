using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Services;
using Technovert.BankApp.Models;

namespace Technovert.BankApp.CLI
{
    public class AccountHolderChoice
    {
        public void Choice(string bankId,string accountId, string userOption,TransactionService transactionService,string[] options)
        {
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
                            string description = "Transaction Log";
                            Printer printer = new Printer();
                            printer.ResponsePrinter(description);
                            /*List<Transaction> Transactions = service.TransactionLogCopy(bankId,accountId);
                            foreach(var i in Transactions)
                            {
                                Console.WriteLine(i.Id+" "+i.Type+" "+i.Amount+" "+i.On);
                            }*/
                            break;
                        }
                    case "LogOut":
                        {
                            Printer printer = new Printer();
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
