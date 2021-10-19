using System;
using System.Collections.Generic;
using System.Text;

namespace Technovert.BankApp
{
    public class Printer
    {
        public void ResponsePrinter(string description)
        {
            string line = new string('-', description.Length + 2);
            Console.WriteLine("\n|" + line + "|");
            Console.WriteLine("| " + description + " |");
            Console.WriteLine("|" + line + "|\n");
        }
    }
}
