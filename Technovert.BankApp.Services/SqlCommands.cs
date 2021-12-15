using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.Services
{
    public class SqlCommands
    {
        public string userAccountFinder(string bankId, string accountHolderId)
        {
            return "SELECT * FROM USERDATA WHERE BANKID = '" + bankId + "' AND USERID = '" + accountHolderId + "'";
        }

        public string currencyFinder(string code)
        {
            return "SELECT * FROM CURRENCY WHERE CODE = '" + code + "'"; 
        }

        public string staffAccountFinder(string id)
        {
            return "SELECT * FROM STAFFDATA WHERE ID = '" + id + "'";
        }
        public string userTransactionFinder(string bankId,string accountId,string transactionId)
        {
            return "SELECT * FROM TRANSACTIONS WHERE TRANSACTION_ID = '" + transactionId + "' AND USER_BANK_ID = '" + 
                bankId + "' AND USER_ID = '" + accountId + "'";
        }

        public string createUserAccount(string name, string id, string password, string bankId, int? age = null, string gender = null)
        {
            return "INSERT INTO USERDATA VALUES ('" + id + "','" + name + "','" + password + "','" + age + "','" + gender
                + "','" + bankId + "','" + 0 + "')";
        }
        public string createStaffAccount(string name, string id, string password)
        {
            return "INSERT INTO STAFFDATA VALUES ('" + name + "','" + id + "','" + password + "')";
        }
        public string AddTransaction(string transactionId, string userId, string bankId, decimal amount, TransactionType type, DateTime time, TaxType taxType,decimal tax=0, string fromUserId = "NA", string fromBankId = "NA")
        {
            return "INSERT INTO TRANSACTIONS VALUES ('" + transactionId + "','" + userId + "'," + amount + ",'" + type + 
                "','" + taxType + "','" + fromUserId + "','" + fromBankId + "','" + bankId + "'," + tax +",'"+ time.ToString("f") +"')";
           
        }

        public string AddCurrency(string name, string code, decimal exchangeRate)
        {
            return "INSERT INTO CURRENCY VALUES ('" + code + "','" + name + "','" + exchangeRate + "')";
        }

        public string DeleteUserAccount(string bankId, string accountId)
        {
            return "DELETE FROM USERDATA WHERE BANKID = '" + bankId + "' AND USERID = '" + accountId + "'";
        }
        public string DeleteTransaction(string bankId, string accountId, string transactionId)
        {
            return "DELETE FROM TRANSACTIONS WHERE USER_BANK_ID = '" + bankId + "' AND USER_ID = '" + accountId + 
                "' AND TRANSACTION_ID = '" + transactionId + "'";
        }

        public string ViewTransactions(string bankId, string accountId)
        {
            return "SELECT * FROM TRANSACTIONS WHERE USER_BANK_ID = '" + bankId + "' AND USER_ID = '" + accountId + "'";
        }
    }
}
