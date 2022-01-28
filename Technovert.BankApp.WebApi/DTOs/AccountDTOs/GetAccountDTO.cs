using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.WebApi.DTOs.AccountDTOs
{
    // Class to represent the properties available for Account holders
    public class GetAccountDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string BankId { get; set; }
    }
}
