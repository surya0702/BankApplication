using System;
using System.Collections.Generic;
using System.Text;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.Models
{
    // Properties available for Transactions done by users
    public class Transaction
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public TaxType TaxType { get; set; }
        public string On { get; set; }
        public Decimal Tax { get; set; }

        public string SourceBankId  { get; set; }
        public string SourceAccountId { get; set; }
        public string DestinationBankId { get; set; }
        public string DestinationAccountId { get; set; }
    }
}
