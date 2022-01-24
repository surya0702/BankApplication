using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Technovert.BankApp.Models;
using Technovert.BankApp.Services;
using Technovert.BankApp.WebApi.DTOs.AccountDTOs;
using AutoMapper;
using Technovert.BankApp.Services.Interfaces;

namespace Technovert.BankApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private HashingService hashing = new HashingService();
        private IMapper mapper;
        private IAccountService accountService;
        public AccountsController(IAccountService AccountService,IMapper Mapper)
        {
            this.mapper = Mapper;
            this.accountService = AccountService;
        }

        [HttpGet("{bankId}")]
        public IActionResult Get(string bankId)
        {
            try
            {
                var accounts = accountService.GetAllAccounts(bankId);
                var accountDTOs = mapper.Map<IEnumerable<GetAccountDTO>>(accounts);
                return Ok(accountDTOs);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{bankId}/{id}")]
        public IActionResult Get(string bankId,string id)
        {
            try
            {
                var account = accountService.GetAccount(bankId,id);
                var accountDTO = mapper.Map<GetAccountDTO>(account);
                return Ok(accountDTO);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{bankId}")]
        public IActionResult Post(string bankId,PostAccountDTO newAccountDTO)
        {
            try 
            {
                if (newAccountDTO == null)
                    return BadRequest();
                var acc = mapper.Map<Account>(newAccountDTO);
                acc.Password = hashing.GetHash(newAccountDTO.Password);
                acc.AccountStatus = Models.Enums.Status.Active;
                acc.Id = Guid.NewGuid().ToString();
                acc.BankId = bankId;
                var newAcc=accountService.CreateAccount(acc);
                return Ok(newAcc);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut("{bankId}/{id}")]
        public IActionResult Put(string bankId,string id,PutAccountDTO accountDTO)
        {
            try
            {
                if (accountDTO == null)
                    return BadRequest();
                var account = accountService.GetAccount(bankId, id);
                account.Name = accountDTO.Name;
                account.Age = accountDTO.Age;
                account.Gender = accountDTO.Gender;
                var updatedAccount=accountService.UpdateAccount(account);
                return Ok(mapper.Map<GetAccountDTO>(updatedAccount));
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{bankId}/{id}")]
        public IActionResult Delete(string bankId, string id)
        {
            try
            {
                Account acc = accountService.CloseAccount(bankId, id);
                return Ok(mapper.Map<DeleteAccountDTO>(acc));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("balance/{bankId}/{id}")]
        public IActionResult GetBalance(string bankId,string id)
        {
            try
            {
                var acc = accountService.GetAccount(bankId, id);
                if (acc == null)
                    return BadRequest();
                return Ok(mapper.Map<BalanceDTO>(acc));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("balance/{bankId}/{id}")]
        public IActionResult PutBalance(string bankId,string id,BalanceDTO balanceDTO)
        {
            try
            {
                if (balanceDTO == null)
                    return BadRequest();
                var acc = accountService.GetAccount(bankId, id);
                acc.Balance += balanceDTO.Balance;
                accountService.UpdateAccount(acc);
                return Ok("Balance has been Updated!");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       /* [HttpPost("auth/{bankId}")]
        public IActionResult Authenticate(string bankId,AuthenticateDTO authDTO)
        {
            try
            {
                var acc = accountService.Authenticate(bankId, authDTO.Id, authDTO.Password);
                return Ok(acc);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/
    }
}