using System;
using System.Collections.Generic;
using System.Linq;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Enums;
using System.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Technovert.BankApp.Services
{
    // Transaction Services available for Account holders
    public class TransactionService
    {
        private AccountHolderService accountHolder;
        private CurrencyService currencyConverter;
        private BankDbContext _DbContext ;

        DateTime today = DateTime.Today;
        
        public TransactionService(BankDbContext dbContext,AccountHolderService accountHolder,CurrencyService currencyConverter)
        {
            this.accountHolder = accountHolder;
            this.currencyConverter = currencyConverter;
            _DbContext = dbContext;
        }

        public string CurrentTime()
        {
            return today.ToString("dd") + today.ToString("MM") + today.ToString("yyyy") + today.ToString("hh") + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");
        }

        public string TransactionIdGenerator(string bankId, string accountId) // Generates a unique transaction id for each transaction done by account holder
        {
            return "TXN" + bankId + accountId + CurrentTime();
        }
        
        public void Deposit(string bankId, string accountHolderId, decimal amount,string code) // Deposits the amount into account holder account
        {
            try
            {
                var UserInfo = _DbContext.Accounts.SingleOrDefault(m => m.Id == accountHolderId && m.BankId == bankId);
                if (UserInfo == null)
                    throw new Exception("Invalid User Details");
                if (UserInfo.AccountStatus == Status.Closed)
                    throw new Exception("User Account Has been Closed!");

                var CurrencyInfo = _DbContext.Currencies.SingleOrDefault(m => m.Code == code);
                if (CurrencyInfo == null)
                    throw new Exception("Invalid Currency Code");

                Decimal newBalance= currencyConverter.Converter(amount, CurrencyInfo.InverseRate);
                UserInfo.Balance += newBalance;
                _DbContext.Accounts.Update(UserInfo);

                var newTransaction = new Transactions()
                {
                    Id = TransactionIdGenerator(bankId, accountHolderId),
                    BankId = bankId,
                    AccountId = accountHolderId,
                    Amount = newBalance,
                    TransactionType = "Credit",
                    OnTime = DateTime.Now
                };
                _DbContext.Transactions.Add(newTransaction);

                _DbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message); 
            }
        }

        public void Withdraw(string bankId, string accountHolderId, decimal amount) // Withdraws the amount from account holders account
        {
            try
            {
                var UserInfo = _DbContext.Accounts.SingleOrDefault(m => m.Id == accountHolderId && m.BankId == bankId);

                if (UserInfo == null)
                    throw new Exception("Invalid User Details");
                if (UserInfo.AccountStatus == Status.Closed)
                    throw new Exception("User Account has been Closed!");

                if (UserInfo.Balance < amount)
                    throw new Exception("Invalid Funds");

                UserInfo.Balance -= amount;
                _DbContext.Accounts.Update(UserInfo);

                var newTransaction = new Transactions()
                {
                    Id = TransactionIdGenerator(bankId, accountHolderId),
                    BankId = bankId,
                    AccountId = accountHolderId,
                    Amount = amount,
                    TransactionType = "Debit",
                    OnTime = DateTime.Now
                };
                _DbContext.Transactions.Add(newTransaction);

                _DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public decimal TaxCalculator(string userBankId,string beneficiaryBankId,decimal amount,string taxType)
        { // Calculates the tax for transferring the amount from one account to another account
            decimal tax = 0;
            if (taxType == "IMPS")
            {
                if (userBankId == beneficiaryBankId)
                {
                    tax = amount * (int)IMPSCharges.SameBank;
                }
                else
                {
                    tax = amount * (int)IMPSCharges.DifferentBank;
                }
            }
            else
            {
                if (userBankId == beneficiaryBankId)
                {
                    tax = amount * (int)RTGSCharges.SameBank;
                }
                else
                {
                    tax = amount * (int)RTGSCharges.DifferentBank;
                }
            }
            return tax/100;
        }

        public void Transfer(string userBankId, string userAccountId, decimal amount, string beneficiaryBankId, string beneficiaryAccountId, string taxType)
        { // Transfers the amount from one account to another account

            if (userBankId == beneficiaryBankId && userAccountId == beneficiaryAccountId)
            {
                throw new Exception("Self Transfer is not Allowed!");
            }

            try
            {
                amount = Math.Round(amount, 2);
                decimal tax = TaxCalculator(userBankId, beneficiaryBankId, amount, taxType);
                tax = Math.Round(tax, 2);

                var userInfo = _DbContext.Accounts.SingleOrDefault(m => m.Id == userAccountId && m.BankId == userBankId);
                if (userInfo == null)
                    throw new Exception("Invalid User Details");
                if (userInfo.AccountStatus == Status.Closed)
                    throw new Exception("User Account Has been Closed!");

                if (userInfo.Balance < amount + tax)
                    throw new Exception("Invalid Funds in Your Account!");

                var beneInfo = _DbContext.Accounts.SingleOrDefault(m => m.Id == beneficiaryAccountId && m.BankId == beneficiaryBankId);
                if (beneInfo == null)
                    throw new Exception("Invalid Beneficiary Account Details");
                if (beneInfo.AccountStatus == Status.Closed)
                    throw new Exception("Beneficiary Account has been Closed!");

                userInfo.Balance -= amount + tax;
                beneInfo.Balance += amount;

                string userTxnId = TransactionIdGenerator(userBankId, userAccountId);
                string beneTxnId = TransactionIdGenerator(beneficiaryBankId, beneficiaryAccountId);
                var userTXN = new Transactions()
                {
                    Id = userTxnId,
                    BankId = userBankId,
                    AccountId = userAccountId,
                    Amount = amount,
                    TaxAmount = tax,
                    TransactionType = "Debit",
                    TaxType = taxType,
                    DestinationBankId = beneficiaryBankId,
                    DestinationAccountId = beneficiaryAccountId,
                    OnTime = DateTime.Now
                };
                _DbContext.Transactions.Add(userTXN);

                var beneTXN = new Transactions()
                {
                    Id = beneTxnId,
                    BankId = beneficiaryBankId,
                    AccountId = beneficiaryAccountId,
                    Amount = amount,
                    TaxAmount = tax,
                    TransactionType = "Credit",
                    TaxType = taxType,
                    DestinationBankId = userBankId,
                    DestinationAccountId = userAccountId,
                    OnTime = DateTime.Now
                };
                _DbContext.Transactions.Add(beneTXN);

                _DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Transactions> TransactionHistory(string bankId,string accountId)
        { // returns the transactions done by the account holder
            try
            {
                var info = _DbContext.Transactions.Where(m => m.BankId == bankId && m.AccountId == accountId).ToList();

                List<Transactions> transactions = new List<Transactions>();
                
                foreach(var row in info)
                {
                    if (row.TxnStatus == "Reversed")
                        continue;
                    transactions.Add(row);
                }
                return transactions;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public decimal ViewBalance(string bankId, string accountId)
        { // returns the balance available in account
            try
            {
                var info = _DbContext.Accounts.SingleOrDefault(m => m.Id == accountId && m.BankId == bankId);
                return info.Balance;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}