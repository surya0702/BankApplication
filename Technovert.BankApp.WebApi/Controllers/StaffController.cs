using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Technovert.BankApp.Services;
using Technovert.BankApp.WebApi.DTOs;

namespace Technovert.BankApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : ControllerBase
    {
        private BankDbContext DbContext;
        public StaffController(BankDbContext context)
        {
            this.DbContext = context;
        }

        [HttpGet]
        public ObjectResult Get()
        {
            return Ok(this.DbContext.Staff
                .ToList()
                .Select(p => new StaffDTO
                {
                    Name = p.Name,
                    Id = p.Id
                }));
        }

        [HttpGet("{id}")]
        public ObjectResult Get(string id)
        {
            return Ok(this.DbContext.Staff
                .Select(p => new StaffDTO
                {
                    Name = p.Name,
                    Id = p.Id
                })
                .SingleOrDefault(m => m.Id == id)
                );
        }
    }
}