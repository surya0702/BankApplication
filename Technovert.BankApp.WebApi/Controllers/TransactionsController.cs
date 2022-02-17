using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Technovert.BankApp.Models;
using Technovert.BankApp.Services;
using Technovert.BankApp.WebApi.DTOs.TransactionDTOs;
using AutoMapper;
using Technovert.BankApp.Services.Interfaces;
using Technovert.BankApp.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Technovert.BankApp.WebApi.Controllers
{
    [ApiController]
    [Authorize(Roles ="User")]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private ITransactionService transactionService;
        private IMapper mapper;
        private IAccountService accountService;

        public TransactionsController(ITransactionService transactionService,IMapper mapper,IAccountService accountService)
        {
            this.transactionService = transactionService;
            this.mapper = mapper;
            this.accountService = accountService;
        }

        [Authorize(Roles ="User")]
        [HttpGet("{bankId}/{accountId}")]
        public IActionResult Get(string bankId,string accountId)
        {
            var transactionList = transactionService.GetAllTransactions(bankId, accountId);
            if (transactionList == null)
                return NotFound();
            var transactionsDTO = mapper.Map<IEnumerable<GetTransactionDTO>>(transactionList);
            foreach(var transactionDTO in transactionsDTO)
            {
                var transaction = transactionList.SingleOrDefault(m => m.Id == transactionDTO.Id);
                transactionDTO.DestinationBankId = transaction.DestinationBankId;
                transactionDTO.DestinationAccountId = transaction.DestinationAccountId;
            }
            return Ok(transactionsDTO);
        }

        [Authorize(Roles = "User")]
        [HttpGet("{transactionId}")]
        public IActionResult Get(string transactionId)
        {
            var transaction = transactionService.GetTransaction(transactionId);
            var transactionDTO = mapper.Map<GetTransactionDTO>(transaction);
            transactionDTO.DestinationAccountId = transaction.DestinationAccountId;
            transactionDTO.DestinationBankId = transaction.DestinationBankId;
            return Ok(transactionDTO);
        }

        [Authorize(Roles = "User")]
        [HttpPost("{bankId}/{accountId}")]
        public IActionResult Post(string bankId, string accountId, CreateTransactionDTO transactionDTO)
        {
            try
            {
                if (transactionDTO == null || transactionDTO.Amount <= 0)
                    return BadRequest();
                decimal tax = 0;
                if (transactionDTO.TaxType == TaxType.IMPS)
                {
                    if (bankId == transactionDTO.DestinationBankId)
                        tax = transactionDTO.Amount * (int)IMPSCharges.SameBank;
                    else
                        tax = transactionDTO.Amount * (int)IMPSCharges.DifferentBank;
                }
                else
                {
                    if (bankId == transactionDTO.DestinationBankId)
                        tax = transactionDTO.Amount * (int)RTGSCharges.SameBank;
                    else
                        tax = transactionDTO.Amount * (int)RTGSCharges.DifferentBank;
                }
                tax /= 100;

                decimal netAmount = transactionDTO.Amount + tax;

                var sourceAccount = accountService.GetAccount(bankId, accountId);
                if (sourceAccount.AccountStatus == Status.Closed)
                    return BadRequest("Account was Closed! Can not initialize the transaction");
                if (sourceAccount.Balance < netAmount)
                    return BadRequest("Insufficient Balance");
                sourceAccount.Balance -= netAmount;
                accountService.UpdateAccount(sourceAccount);

                var destinationAccount = accountService.GetAccount(transactionDTO.DestinationBankId, transactionDTO.DestinationAccountId);
                if (destinationAccount.AccountStatus == Status.Closed)
                    return BadRequest("Destination Account was closed! Can not initialize the transaction");
                destinationAccount.Balance += transactionDTO.Amount;
                accountService.UpdateAccount(destinationAccount);

                var newTransaction = mapper.Map<Transactions>(transactionDTO);
                newTransaction.Id = Guid.NewGuid().ToString();
                newTransaction.BankId = bankId;
                newTransaction.AccountId = accountId;
                newTransaction.DestinationBankId = transactionDTO.DestinationBankId;
                newTransaction.DestinationAccountId = transactionDTO.DestinationAccountId;
                newTransaction.Amount = transactionDTO.Amount;
                newTransaction.TaxAmount = tax;
                newTransaction.TaxType = transactionDTO.TaxType;
                newTransaction.OnTime = DateTime.Now;
                newTransaction.TransactionType = TransactionType.Transfer;

                transactionService.AddTransaction(newTransaction);
                return Ok(mapper.Map<GetTransactionDTO>(newTransaction));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut("Deposit/{bankId}/{accountId}")]
        public IActionResult Deposit(string bankId,string accountId,decimal amount)
        {
            try
            {
                var transaction = transactionService.Deposit(bankId, accountId, amount);
                return Ok(mapper.Map<DefaultTransactionDTO>(transaction));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut("Withdraw/{bankId}/{accountId}")]
        public IActionResult Withdraw(string bankId,string accountId,decimal amount)
        {
            try
            {
                var transaction = transactionService.Withdraw(bankId, accountId, amount);
                return Ok(mapper.Map<DefaultTransactionDTO>(transaction));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}