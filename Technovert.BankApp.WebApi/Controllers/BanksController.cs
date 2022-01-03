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
        public IActionResult Post(PostBankDTO bank)
        {
            if (bank == null)
                return BadRequest();
            var newBank = mapper.Map<Bank>(bank);
            newBank.Description = bank.Desciption;
            newBank.BankStatus = Models.Enums.Status.Active;
            return Ok(bankService.CreateBank(newBank));
        }
    }
}