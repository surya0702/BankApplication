using System;
using System.Collections.Generic;
using System.Text;
using Technovert.BankApp.Services;

namespace Technovert.BankApp.CLI
{
    public class AccountLogger
    {
        public void Logger(string bankName,BankService service)
        {
            List<string> LoginOptions = new List<string>() { "Login to Account", "CreateAccount", "Exit from Bank" };
            while (true)
            {
                bool stop = false;
                string userLoginOption = "";
                for (int i = 0; i < LoginOptions.Count; i++)
                {
                    Console.WriteLine("(" + (i + 1) + ") " + LoginOptions[i]);
                }
                Console.Write("\nPlease choose an Option : ");
                try
                {
                    userLoginOption = LoginOptions[Convert.ToInt32(Console.ReadLine()) - 1];
                }
                catch
                {
                    Console.WriteLine("\nEnter a valid Number\n");
                }

                switch (userLoginOption)
                {
                    case "Login to Account":
                        {
                            AccountLogin login = new AccountLogin();
                            login.Logger(bankName,service);
                            break;
                        }
                    case "CreateAccount":
                        {
                            AccountCreator account = new AccountCreator();
                            account.Create(bankName,service);
                            break;
                        }
                    case "Exit from Bank":
                        {
                            Printer printer = new Printer();
                            printer.ResponsePrinter("Exited");
                            stop = true;
                            break;
                        }
                }
                if (stop)
                {
                    break;
                }
            }
        }
    }
}
