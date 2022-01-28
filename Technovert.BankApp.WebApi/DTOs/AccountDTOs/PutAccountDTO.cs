using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.WebApi.DTOs.AccountDTOs
{
    // Class to represent the properties available for Account holders
    public class PutAccountDTO
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
    }
}
