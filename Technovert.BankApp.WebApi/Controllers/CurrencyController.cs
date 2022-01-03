using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Technovert.BankApp.Services;
using Technovert.BankApp.Services.Interfaces;

namespace Technovert.BankApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private BankDbContext DbContext;
        private ICurrencyService CurrencyService;
        public CurrencyController(BankDbContext context,ICurrencyService currencyService)
        {
            DbContext = context;
            CurrencyService = currencyService;
        }

        [HttpGet]
        public ObjectResult Get()
        {
            return Ok(this.DbContext.Currencies.ToList());
        }

        [HttpGet("{code}")]
        public ObjectResult Get(string code)
        {
            return Ok(this.DbContext.Currencies.SingleOrDefault(m => m.Code == code));
        }
    }
}