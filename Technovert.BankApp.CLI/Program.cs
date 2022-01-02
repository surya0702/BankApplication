using System;
using System.Collections.Generic;
using Technovert.BankApp.Services;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Technovert.BankApp.Models;
using System.Data.SqlClient;
using System.Data;

namespace Technovert.BankApp.CLI
{
    public class Program
    {
        static void Main()
        {
            BankDbContext DbContext = new BankDbContext();
            CurrencyConverter currencyConverter = new CurrencyConverter(DbContext);
            
            BankService bankService = new BankService(DbContext);
            StaffService bankStaffService = new StaffService(DbContext);
            AccountHolderService accountHolderService = new AccountHolderService(DbContext,bankService);

            TransactionService transactionService = new TransactionService(DbContext,accountHolderService,currencyConverter);

            try
            {
                bankStaffService.CreateStaffAccount("Admin"); // Default Staff Account , Id : Admin, Password : Admin@123
                currencyConverter.CurrencyExchange();
                bankService.CreateBank("SBI", "State Bank Of India"); // Default banks
                bankService.CreateBank("YesBank", "Yes Bank of India");
                bankService.CreateBank("HDFC", "Housing Development Finance Corporation Limited");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

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
                            staff.Login(bankService, accountHolderService, bankStaffService, transactionService);
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
