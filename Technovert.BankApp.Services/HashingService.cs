using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Technovert.BankApp.Services
{
    public class HashingService
    {
        public DateTime today = DateTime.Today;

        public string AccountIdGenerator(string AccountHolderName) // Generates a account id for newly created account
        {
            return AccountHolderName.Substring(0, 3).ToUpper() + today.ToString("dd") + today.ToString("MM") + today.ToString("yyyy") + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm");
        }

        public void InputValidator(params Object[] inputs) // Validates the user input
        {
            foreach (var input in inputs)
            {
                if (input == null)
                {
                    throw new Exception("Invalid Input!");
                }
            }
        }
        public string GetHash(string value)
        {
            var data = Encoding.ASCII.GetBytes(value);
            var hashData = new SHA1Managed().ComputeHash(data);
            var hash = string.Empty;
            foreach (var b in hashData)
            {
                hash += b.ToString("X2");
            }
            return hash;
        }
    }
}
