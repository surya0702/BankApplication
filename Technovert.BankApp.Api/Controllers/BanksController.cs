using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Technovert.BankApp.Services;

namespace Technovert.BankApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BanksController : ControllerBase
    {
        private BankDbContext DbContext;
        public BanksController()
        {
            this.DbContext = new BankDbContext();
        }

        [HttpGet]
        public ObjectResult Get()
        {
            return Ok(this.DbContext.Banks.ToList());
        }

        [HttpGet("{id}")]
        public ObjectResult Get(string id)
        {
            return Ok(this.DbContext.Banks.SingleOrDefault(m => m.Id == id));
        }
    }
}