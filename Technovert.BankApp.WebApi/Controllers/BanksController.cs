using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Technovert.BankApp.Models;
using Technovert.BankApp.Services;
using Technovert.BankApp.Services.Interfaces;
using Technovert.BankApp.WebApi.DTOs.BankDTOs;

namespace Technovert.BankApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BanksController : ControllerBase
    {
        private IMapper mapper;
        private IBankService bankService;
        public BanksController(IMapper mapper,IBankService bankService)
        {
            this.mapper = mapper;
            this.bankService = bankService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(bankService.GetAllBanks());
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return Ok(bankService.GetBank(id));
        }

        [HttpPost]
        public IActionResult Post(BankDTO bank)
        {
            if (bank == null)
                return BadRequest();
            var newBank = mapper.Map<Bank>(bank);
            newBank.Description = bank.Desciption;
            newBank.BankStatus = Models.Enums.Status.Active;
            return Ok(bankService.CreateBank(newBank));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var bank = bankService.CloseBank(id);
                var bankToDelete = mapper.Map<BankDTO>(bank);
                bankToDelete.Desciption = bank.Description;
                return Ok(bankToDelete);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id,BankDTO bankDTO)
        {
            try
            {
                if (bankDTO == null)
                    return BadRequest("No values are specified");
                var bank = bankService.GetBank(id);
                bank.Name = bankDTO.Name;
                bank.Description = bankDTO.Desciption;
                var updatedBank = bankService.UpdateBank(bank);
                return Ok(mapper.Map<BankDTO>(updatedBank));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}