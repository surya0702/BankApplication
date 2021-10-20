using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Services;
using Technovert.BankApp.Models;

namespace Technovert.BankApp.CLI
{
    public class LoginOptionsChooser
    {
        public void Choice(string bankName,string accountName, string userOption,BankService service)
        {
            while (true)
            {
                bool stop = false;
                switch (userOption)
                {
                    case "Deposit":
                        {
                            Depositer depositer = new Depositer();
                            depositer.Deposite(bankName,accountName, service);
                            break;
                        }
                    case "Withdraw":
                        {
                            Withdrawer withdrawer = new Withdrawer();
                            withdrawer.Withdraw(bankName,accountName,service);
                            break;
                        }
                    case "Transfer":
                        {
                            Transferer transferer = new Transferer();
                            transferer.Transfer(bankName,accountName,service);
                            break;
                        }
                    case "Transaction History":
                        {
                            string description = "Transaction Log";
                            Printer printer = new Printer();
                            printer.ResponsePrinter(description);
                            List<Transaction> Transactions = service.TransactionLogCopy(bankName,accountName);
                            foreach(var i in Transactions)
                            {
                                Console.WriteLine(i.Id+" "+i.Type+" "+i.Amount+" "+i.On);
                            }
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
                    userOption=option.AvailableOptions();
                }
            }
        }
    }
}
