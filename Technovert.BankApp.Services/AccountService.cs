using System;
using System.Collections.Generic;
using System.Linq;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Enums;
using System.Data.SqlClient;
using Technovert.BankApp.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Technovert.BankApp.Services
{
    // All the services available for account holders
    public class AccountService : IAccountService
    {
        private HashingService hashing = new HashingService();
        private BankDbContext _DbContext;
        private readonly AppSettings _appSettings;

        public AccountService(BankDbContext dbContext,IOptions<AppSettings> appSettings)
        {
            _DbContext = dbContext;
            _appSettings = appSettings.Value;
        }
        
        public string Authenticate(string bankId,string id,string password) // Logs the user into his account by checking with the available id and password
        {
            hashing.InputValidator(bankId,id,password);
            try
            {
                var info = _DbContext.Accounts.SingleOrDefault(m => m.Id == id && m.BankId == bankId);
                if (info == null)
                    return null;
                string currPassword = hashing.GetHash(password);
                if (info.Password != currPassword)
                    throw new Exception("Invalid Password");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, info.Id),
                        new Claim(ClaimTypes.Role, info.Role)
                    }),
                    Expires = DateTime.Now.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Account CreateAccount(Account account) // Creates a new user account in a bank containing bankId
        {
            try
            {
                hashing.InputValidator(account);
                _DbContext.Accounts.Add(account);
                _DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return account; // Returns the newly created user id and password for user
        }

        public Account UpdateAccount(Account account)
        {
            _DbContext.Accounts.Attach(account);
            _DbContext.SaveChanges();
            var UpdatedAccount = _DbContext.Accounts.FirstOrDefault(m => m.Id == account.Id);
            return UpdatedAccount;
        }

        public Account CloseAccount(string bankId,string accountId)
        {
            var accountToDelete = _DbContext.Accounts.SingleOrDefault(m => m.BankId == bankId && m.Id == accountId);
            if (accountToDelete.AccountStatus == Status.Closed)
                throw new Exception("Account was Already Closed");
            if (accountToDelete.Role == Roles.Admin)
                throw new Exception("Can't Delete a Administrator Account");
            accountToDelete.AccountStatus = Status.Closed;
            _DbContext.SaveChanges();
            return accountToDelete;
        }

        public Account GetAccount(string bankId,string accountId)
        {
            var account = _DbContext.Accounts.FirstOrDefault(m => m.BankId == bankId && m.Id == accountId);
            if (account == null)
                throw new Exception("Account Not Found!");
            if (account.AccountStatus == Status.Closed)
                throw new Exception("Account was Closed!");
            return account;
        }

        public List<Account> GetAllAccounts(string bankId)
        {
            return _DbContext.Accounts.Where(m => m.BankId == bankId && m.AccountStatus==Status.Active && m.Role==Roles.User).ToList();
        }
    }
}
