﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Models;
using Technovert.BankApp.Services;

namespace Technovert.BankApp.CLI
{
    // Displays the available options and prompts for user input 
    public class LoginOptions
    {
        public string AvailableOptions(string[] options)
        {
            Console.WriteLine("\n");
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine("(" + (i + 1) + ") " + options[i]);
            }
            Console.Write("\nPlease select an option : ");
            string choosenOption = "";
            try
            {
                choosenOption = options[Convert.ToInt32(Console.ReadLine()) - 1];
            }
            catch
            {
                Console.WriteLine("Invalid Input");
            }
            return choosenOption;
        }
    }
}
