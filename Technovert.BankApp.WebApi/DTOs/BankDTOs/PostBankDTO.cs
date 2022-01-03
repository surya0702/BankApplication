using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.WebApi.DTOs.BankDTOs
{
    // Class to represent the properties available for Account holders
    public class PostBankDTO
    {
        [Required]
        public string Name { get; set; }
        public string Desciption { get; set; }
    }
}
