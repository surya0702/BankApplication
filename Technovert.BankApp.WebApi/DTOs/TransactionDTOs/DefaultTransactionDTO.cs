using System;
using System.ComponentModel.DataAnnotations;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.WebApi.DTOs.TransactionDTOs
{
    public class DefaultTransactionDTO
    {
        public string Id { get; set; }
        public string BankId { get; set; }
        public string AccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime OnTime { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}