using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Technovert.BankApp.Models.Enums;

namespace Technovert.BankApp.WebApi.DTOs
{
    // Class to represent the properties available for Account holders
    public class StaffDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
