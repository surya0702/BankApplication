using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.WebApi.DTOs.AccountDTOs
{
    // Class to represent the properties available for Account holders
    public class BalanceDTO
    {
        [Display(Name ="Enter the Amount")]
        public decimal Balance { get; set; }
    }
}
