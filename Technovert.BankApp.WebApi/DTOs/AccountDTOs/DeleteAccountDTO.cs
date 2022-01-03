using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.WebApi.DTOs.AccountDTOs
{
    // Class to represent the properties available for Account holders
    public class DeleteAccountDTO
    {
        public string BankId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public Status AccountStatus { get; set; }
    }
}
