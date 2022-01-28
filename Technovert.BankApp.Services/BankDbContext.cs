using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technovert.BankApp.Models;

namespace Technovert.BankApp.Services
{
    public class BankDbContext : DbContext
    {
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<StaffAccount> Staff { get; set; }
        public DbSet<Transactions> Transactions { get; set; }

        public BankDbContext(DbContextOptions<BankDbContext> options) 
            : base(options)
        {

        }
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=BankAppDB;Integrated Security=True");
        }*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bank>(entity =>
            {
                entity.ToTable("Bank");
                entity.Property(m => m.Id);
                entity.Property(m => m.Name);
                entity.Property(m => m.Description);
            });
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");
                entity.Property(m => m.Id);
                entity.Property(m => m.Name);
                entity.Property(m => m.Password);
                entity.Property(m => m.Balance);
                entity.Property(m => m.BankId);
                entity.Property(m => m.Age);
                entity.Property(m => m.Gender);
                entity.Property(m => m.AccountStatus);
                entity.Property(m => m.Role);
            });
            modelBuilder.Entity<Currency>(entity =>
            {
                entity.ToTable("Currency");
                entity.Property(m => m.Code);
                entity.Property(m => m.Name);
                entity.Property(m => m.InverseRate);
            });
            modelBuilder.Entity<StaffAccount>(entity =>
            {
                entity.ToTable("StaffAccount");
                entity.Property(m => m.Id);
                entity.Property(m => m.Name);
                entity.Property(m => m.Password);
            });
            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.ToTable("Transactions");
                entity.Property(m => m.Id);
                entity.Property(m => m.BankId);
                entity.Property(m => m.AccountId);
                entity.Property(m => m.Amount);
                entity.Property(m => m.TaxAmount);
                entity.Property(m => m.TaxType);
                entity.Property(m => m.DestinationBankId);
                entity.Property(m => m.DestinationAccountId);
                entity.Property(m => m.OnTime);
                entity.Property(m => m.TransactionType);
            });
        }
    }
}
