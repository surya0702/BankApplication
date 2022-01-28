using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.WebApi.DTOs.AccountDTOs
{
    // Class to represent the properties available for Account holders
    public class PostAccountDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password",ErrorMessage ="Passwords do not match")]
        public string ConformPassword { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
    }
}
