using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Technovert.BankApp.Models;
using Technovert.BankApp.Models.Enums;
using Technovert.BankApp.Services;
using Technovert.BankApp.Services.Interfaces;
using Technovert.BankApp.WebApi.DTOs.BankDTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Technovert.BankApp.WebApi.Controllers
{
    [ApiController]
    [Authorize]
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(bankService.GetAllBanks());
        }

        [Authorize(Roles ="Staff")]
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

        [Authorize(Roles="Staff")]
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
    }
}