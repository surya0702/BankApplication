using System;
using System.ComponentModel.DataAnnotations;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.WebApi.DTOs.TransactionDTOs
{
    public class CreateTransactionDTO
    {
        [Required]
        public string DestinationBankId { get; set; }
        [Required]
        public string DestinationAccountId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public TaxType TaxType { get; set; }
    }
}