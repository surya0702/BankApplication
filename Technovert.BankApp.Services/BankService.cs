 using System;
using System.Text;
using System.Collections.Generic;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Exceptions;
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
        private SqlCommands commands=new SqlCommands();
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
            /*bool flag = false;
            try
            {
                string temp= "SELECT * FROM BANKDATA WHERE BANKID = '" + bankId + "'";
                using (SqlConnection newConnection = new SqlConnection(conn.ConnectionString))
                {
                    SqlCommand sql = new SqlCommand(temp, newConnection);
                    newConnection.Open();
                    SqlDataReader reader=sql.ExecuteReader();
                    if (reader.HasRows==false)
                    {
                        flag = true;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            if (flag)
            {
                throw new Exception("Invalid Bank Id");
            }*/
        }
        public string BankIdGenerator(string bankName)
        {
            return bankName.Substring(0, 3).ToUpper() + today.ToString("dd") + today.ToString("MM") + today.ToString("yyyy");
        }

        public void CreateBank(string bankName,string description) // Creates a new Bank and adds the bank to data
        {
            if (String.IsNullOrWhiteSpace(bankName))
            {
                throw new InvalidInputException();
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
            /*string temp = "INSERT INTO BANKDATA VALUES ('" + newBankId + "','" + bankName + "')";
            try
            {
                using(SqlConnection newConnection = new SqlConnection(conn.ConnectionString))
                {
                    SqlCommand sql = new SqlCommand(temp, newConnection);
                    newConnection.Open();
                    sql.ExecuteNonQuery();
                }
            }
            catch
            {
                throw new Exception("Invalid bank Name for creation");
            }*/
        }

    }
}
