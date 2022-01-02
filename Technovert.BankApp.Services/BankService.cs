 using System;
using System.Text;
using System.Collections.Generic;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Enums;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Data;

namespace Technovert.BankApp.Services
{
    // Services available for Banks
    public class BankService
    {
        private HashingService hashing = new HashingService();
        DateTime today = DateTime.Today;
        private BankDbContext DbContext ;
        
        public BankService(BankDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public void BankFinder(string bankId) // Finds the bank using bankId in data
        {
            var info = DbContext.Banks.SingleOrDefault(m => m.Id == bankId);
            if(info == null)
            {
                throw new Exception("Invalid Bank Id");
            }
        }
        public string BankIdGenerator(string bankName)
        {
            return bankName.Substring(0, 3).ToUpper() + today.ToString("dd") + today.ToString("MM") + today.ToString("yyyy");
        }

        public void CreateBank(string bankName,string description) // Creates a new Bank and adds the bank to data
        {
            if (String.IsNullOrWhiteSpace(bankName))
            {
                throw new Exception("Invalid Input");
            }
            string newBankId = BankIdGenerator(bankName);
            var newBank = new Bank()
            {
                Id = newBankId,
                Name = bankName,
                Description = description
            };
            try
            {
                DbContext.Banks.Add(newBank);
                DbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
