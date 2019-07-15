using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DsCheques.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DsCheques.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChequesController : ControllerBase
    {
        private readonly IChequeRepository chequesRepository;

        public ChequesController(IChequeRepository chequesRepository)
        {
            this.chequesRepository = chequesRepository;
        }

        [HttpGet]
        public IActionResult GetCheques()
        {
            return Ok(this.chequesRepository.GetAll());
        }

    }
}