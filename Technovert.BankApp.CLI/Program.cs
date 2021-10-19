using System;
using System.Collections.Generic;
using Technovert.BankApp.Services;
using Technovert.BankApp.Models;

namespace Technovert.BankApp.CLI
{
    public class Program
    {
        static void Main()
        {
            BankService service = new BankService();
            List<string> bankLoginOptions = new List<string>(){ "Login to Bank", "Create New Bank","Exit from Application" };
            while (true)
            {
                for (int i = 0; i < bankLoginOptions.Count; i++)
                {
                    Console.WriteLine("(" + (i + 1) + ") " + bankLoginOptions[i]);
                }
                Console.Write("\nEnter your choice : ");
                string userBankLogin = "";
                try
                {
                    userBankLogin = bankLoginOptions[Convert.ToInt32(Console.ReadLine()) - 1];
                }
                catch
                {
                    Console.WriteLine("\nInvalid Login Option!\n");
                }
                bool flag = false;
                switch (userBankLogin)
                {
                    case "Login to Bank":
                        {
                            string bankName;
                            Console.Write("Enter your Bank Name : ");
                            bankName = Console.ReadLine();
                            bool response = service.BankLogin(bankName);
                            if (response)
                            {
                                AccountLogger accountLogger = new AccountLogger();
                                accountLogger.Logger(bankName, service);
                            }
                            else
                            {
                                Console.WriteLine("\nInvalid Login Details\n");
                            }
                            break;
                        }
                    case "Create New Bank":
                        {
                            string bankName;
                            Console.Write("Enter your Bank Name : ");
                            bankName = Console.ReadLine();
                            service.CreateBank(bankName);
                            AccountLogger accountLogger = new AccountLogger();
                            accountLogger.Logger(bankName, service);
                            break;
                        }
                    case "Exit from Application":
                        {
                            flag = true;
                            break;
                        }
                }
                if (flag == true)
                {
                    break;
                }
            }
        }
    }
}
