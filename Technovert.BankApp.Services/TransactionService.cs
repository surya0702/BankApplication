using System;
using System.Collections.Generic;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Exceptions;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.Services
{
    public class TransactionService
    {
        public int limit = 50000;
        DateTime today = DateTime.Today;
        AccountHolderService accountHolder;
        TransactionCharges charges = new TransactionCharges();
        public TransactionService(AccountHolderService accountHolder)
        {
            this.accountHolder = accountHolder;
        }
        public string TransactionIdGenerator(string bankId, string accountId)
        {
            return "TXN" + bankId + accountId + today.ToString("dd") + today.ToString("MM") + today.ToString("yyyy") + today.ToString("hh") + today.ToString("mm");
        }
        public void Deposit(string bankId, string accountHolderId, decimal amount)
        {
            this.accountHolder.InputValidator(bankId, accountHolderId);

            Account userAccount = accountHolder.AccountFinder(bankId, accountHolderId);

            if (userAccount == null)
            {
                throw new InvalidAccountNameException();
            }
            userAccount.Balance += amount;
            userAccount.Transactions.Add(new Transaction()
            {
                Id = TransactionIdGenerator(bankId, accountHolderId),
                Amount = amount,
                Type = TransactionType.CASH,
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
                Type = TransactionType.CASH,
                On = today.ToString("g")
            });
        }

        public void Transfer(string userBankId, string userAccountId, decimal amount, string beneficiaryBankId, string beneficiaryAccountId)
        {
            this.accountHolder.InputValidator(userAccountId, beneficiaryAccountId, userBankId, beneficiaryBankId);

            Account userAccount = this.accountHolder.AccountFinder(userBankId, userAccountId);

            Account beneficiaryAccount = this.accountHolder.AccountFinder(beneficiaryBankId, beneficiaryAccountId);

            decimal tax = 0;

            if (amount > limit)
            {
                if (userBankId != beneficiaryBankId)
                    tax = amount * charges.DifferentBankIMPS/100;
                else
                    tax = amount * charges.SameBankIMPS/100;
            }
            else
            {
                if (userBankId != beneficiaryBankId)
                    tax = amount * charges.DifferentBankRTGS/100;
                else
                    tax = amount * charges.SameBankRTGS/100;
            }
            if (userAccount == null || beneficiaryAccount == null)
                throw new InvalidAccountNameException();

            if (userAccount.Balance < amount+tax)
                throw new InsufficientFundsException();

            userAccount.Balance -= amount+tax;
            userAccount.Transactions.Add(new Transaction()
            {
                Id = TransactionIdGenerator(userBankId, userAccountId),
                Amount = amount,
                Type = amount > limit ? TransactionType.IMPS : TransactionType.RTGS,
                On = today.ToString("g"),
                Tax=tax,
                SourceBankId=userBankId,
                SourceAccountId=userAccountId,
                DestinationBankId=beneficiaryBankId,
                DestinationAccountId=beneficiaryAccountId
            });

            beneficiaryAccount.Balance += amount;
            beneficiaryAccount.Transactions.Add(new Transaction()
            {
                Id = TransactionIdGenerator(beneficiaryBankId, beneficiaryAccountId),
                Amount = amount,
                Type = amount > limit ? TransactionType.IMPS : TransactionType.RTGS,
                On = today.ToString("g"),
                Tax=tax,
                DestinationBankId = userBankId,
                DestinationAccountId = userAccountId,
                SourceBankId = beneficiaryBankId,
                SourceAccountId = beneficiaryAccountId
            });
        }
    }
}