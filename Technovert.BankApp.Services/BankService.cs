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
using Technovert.BankApp.Services.Interfaces;

namespace Technovert.BankApp.Services
{
    // Services available for Banks
    public class BankService : IBankService
    {
        private HashingService hashing = new HashingService();
        DateTime today = DateTime.Today;
        private BankDbContext _DbContext ;
        
        public BankService(BankDbContext dbContext)
        {
            _DbContext = dbContext;
        }
        public string BankIdGenerator(string bankName)
        {
            return bankName.Substring(0, 3).ToUpper() + today.ToString("dd") + today.ToString("MM") + today.ToString("yyyy");
        }

        public Bank CreateBank(Bank bank) // Creates a new Bank and adds the bank to data
        {
            try
            {
                bank.Id = Guid.NewGuid().ToString();
                var duplicateBank = _DbContext.Banks.FirstOrDefault(m => m.Name == bank.Name && m.Description == bank.Description);
                if (duplicateBank != null)
                    throw new Exception("Bank already exists!");
                _DbContext.Banks.Add(bank);
                _DbContext.SaveChanges();
                return bank;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Bank CloseBank(string bankId)
        {
            try
            {
                var bankToDelete = _DbContext.Banks.SingleOrDefault(m => m.Id == bankId);
                if (bankToDelete == null)
                    throw new Exception("Invalid Bank Id");
                if (bankToDelete.BankStatus == Status.Closed)
                    throw new Exception("Bank Already Closed!");
                bankToDelete.BankStatus = Status.Closed;
                _DbContext.SaveChanges();
                return bankToDelete;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Bank> GetAllBanks()
        {
            return _DbContext.Banks.Where(m=>m.BankStatus==Status.Active).ToList();
        }

        public Bank GetBank(string bankId)
        {
            var bank = _DbContext.Banks.FirstOrDefault(m => m.Id == bankId);
            if (bank == null)
                throw new Exception("Bank Not Found!");
            if (bank.BankStatus == Status.Closed)
                throw new Exception("Bank was Closed!");
            return bank;
        }

        public Bank UpdateBank(Bank bank)
        {
            _DbContext.Banks.Attach(bank);
            _DbContext.SaveChanges();
            var UpdatedBank = _DbContext.Banks.FirstOrDefault(m => m.Id == bank.Id);
            return UpdatedBank;
        }
    }
}
