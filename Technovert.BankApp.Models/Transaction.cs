using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.Models
{
    // Properties available for Transactions done by users
    public class Transaction
    {
        [Key]
        public string Id { get; set; }
        public string BankId { get; set; }
        public string AccountId { get; set; }
        public string DestinationBankId { get; set; }
        public string DestinationAccountId { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }
        public string TransactionType { get; set; }
        public string TaxType { get; set; }
        public DateTime OnTime { get; set; }
    }
}
