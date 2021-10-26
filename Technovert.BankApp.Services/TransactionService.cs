using System;
using System.Collections.Generic;
using System.Linq;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Exceptions;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.Services
{
    public class TransactionService
    {
        public int limit = 50000;
        DateTime today = DateTime.Today;
        private AccountHolderService accountHolder;
        private Data data;
        TransactionCharges charges = new TransactionCharges();
        public TransactionService(Data data ,AccountHolderService accountHolder)
        {
            this.data = data;
            this.accountHolder = accountHolder;
        }
        public string TransactionIdGenerator(string bankId, string accountId)
        {
            return "TXN" + bankId + accountId + today.ToString("dd") + today.ToString("MM") + today.ToString("yyyy") + today.ToString("hh") + today.ToString("mm");
        }
        
        public void Deposit(string bankId, string accountHolderId, decimal amount,string code)
        {
            this.accountHolder.InputValidator(bankId, accountHolderId);

            Account userAccount = accountHolder.AccountFinder(bankId, accountHolderId);
            Currency currentCurrency = this.data.currencies.SingleOrDefault(x => x.Code == code);
            if (currentCurrency == null)
            {
                throw new InvalidCurrencyException();
            }
            if (userAccount == null)
            {
                throw new InvalidAccountNameException();
            }
            CurrencyConverter currencyConverter = new CurrencyConverter();
            userAccount.Balance += currencyConverter.Converter(amount,currentCurrency.ExchangeRate);
            userAccount.Transactions.Add(new Transaction()
            {
                Id = TransactionIdGenerator(bankId, accountHolderId),
                Amount = amount,
                TransactionType = TransactionType.Credit,
                On = today.ToString("g")
            });
        }

        public void Withdraw(string bankId, string accountHolderId, decimal amount)
        {
            this.accountHolder.InputValidator(bankId, accountHolderId);

            Account userAccount = accountHolder.AccountFinder(bankId, accountHolderId);

            if (userAccount == null)
            {
                throw new InvalidAccountNameException();
            }
            if (userAccount.Balance < amount)
            {
                throw new InsufficientFundsException();
            }
            userAccount.Balance -= amount;
            userAccount.Transactions.Add(new Transaction()
            {
                Id = TransactionIdGenerator(bankId, accountHolderId),
                Amount = amount,
                TransactionType = TransactionType.Credit,
                On = today.ToString("g")
            });
        }
        public decimal TaxCalculator(string userBankId,string beneficiaryBankId,decimal amount)
        {
            decimal tax = 0;
            if (amount > limit)
            {
                if (userBankId != beneficiaryBankId)
                    tax = amount * charges.DifferentBankIMPS / 100;
                else
                    tax = amount * charges.SameBankIMPS / 100;
            }
            else
            {
                if (userBankId != beneficiaryBankId)
                    tax = amount * charges.DifferentBankRTGS / 100;
                else
                    tax = amount * charges.SameBankRTGS / 100;
            }
            return tax;
        }
        public void Transfer(string userBankId, string userAccountId, decimal amount, string beneficiaryBankId, string beneficiaryAccountId, TaxType taxType, bool undo = false)
        {
            this.accountHolder.InputValidator(userAccountId, beneficiaryAccountId, userBankId, beneficiaryBankId);

            Account userAccount = this.accountHolder.AccountFinder(userBankId, userAccountId);

            Account beneficiaryAccount = this.accountHolder.AccountFinder(beneficiaryBankId, beneficiaryAccountId);
            
            if (userAccount == null || beneficiaryAccount == null)
                throw new InvalidAccountNameException();

            if (undo == true)
            {
                userAccount.Balance += amount;
                beneficiaryAccount.Balance -= amount;
                return;
            }

            decimal tax = TaxCalculator(userBankId, beneficiaryBankId, amount);

            if (userAccount.Balance < amount+tax)
                throw new InsufficientFundsException();

            userAccount.Balance -= amount+tax;
            userAccount.Transactions.Add(new Transaction()
            {
                Id = TransactionIdGenerator(userBankId, userAccountId),
                Amount = amount,
                TransactionType = TransactionType.Debit,
                On = today.ToString("g"),
                Tax = tax,
                TaxType = taxType,
                SourceBankId = userBankId,
                SourceAccountId = userAccountId,
                DestinationBankId = beneficiaryBankId,
                DestinationAccountId = beneficiaryAccountId
            });

            beneficiaryAccount.Balance += amount;
            beneficiaryAccount.Transactions.Add(new Transaction()
            {
                Id = TransactionIdGenerator(beneficiaryBankId, beneficiaryAccountId),
                Amount = amount,
                TransactionType = TransactionType.Credit,
                On = today.ToString("g"),
                Tax = tax,
                TaxType = taxType,
                DestinationBankId = userBankId,
                DestinationAccountId = userAccountId,
                SourceBankId = beneficiaryBankId,
                SourceAccountId = beneficiaryAccountId
            });
        }

        public List<Transaction> TransactionHistory(string bankId,string accountId)
        {
            Account account = this.accountHolder.AccountFinder(bankId, accountId);
            if (account == null)
            {
                throw new InvalidAccountNameException();
            }
            return account.Transactions;
        }
        public decimal ViewBalance(string bankId, string accountId)
        {
            Account account = this.accountHolder.AccountFinder(bankId, accountId);
            if (account == null)
            {
                throw new InvalidAccountNameException();
            }
            return account.Balance;
        }
    }
}