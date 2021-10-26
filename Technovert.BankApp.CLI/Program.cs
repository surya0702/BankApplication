using System;
using System.Collections.Generic;
using Technovert.BankApp.Services;
using Technovert.BankApp.Models.Exceptions;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Technovert.BankApp.Models;

namespace Technovert.BankApp.CLI
{
    public class Program
    {
        static void Main()
        {
            BankStaffService bankStaffService = new BankStaffService();
            Data data = new Data();
            CurrencyConverter currencyConverter = new CurrencyConverter(data);
            BankService bankService = new BankService(data);
            AccountHolderService accountHolderService = new AccountHolderService(bankService);
            TransactionService transactionService = new TransactionService(data,accountHolderService,currencyConverter);

            bankStaffService.CreateStaffAccount("Admin");

            currencyConverter.CurrencyExchange();

            bankService.CreateBank("SBI");
            bankService.CreateBank("YesBank");
            bankService.CreateBank("HDFC");

            string[] loginOptions = { "Account Holder Login", "Staff Login","Exit from Application" };
            while (true)
            {
                Console.WriteLine();
                for (int i = 0; i < loginOptions.Length; i++)
                {
                    Console.WriteLine("(" + (i + 1) + ") " + loginOptions[i]);
                }
                Console.Write("\nEnter your choice : ");
                string userBankLogin = "";
                try
                {
                    userBankLogin = loginOptions[Convert.ToInt32(Console.ReadLine()) - 1];
                }
                catch
                {
                    Console.WriteLine("\nInvalid Login Option!\n");
                }
                bool flag = false;
                switch (userBankLogin)
                {
                    case "Account Holder Login":
                        {
                            AccountHolderLogin accountHolder = new AccountHolderLogin();
                            accountHolder.Login(bankService, accountHolderService,transactionService);
                            break;
                        }
                    case "Staff Login":
                        {
                            StaffLogin staff = new StaffLogin();
                            staff.Login(data,bankService, accountHolderService, bankStaffService, transactionService);
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
