using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DsCheques.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DsCheques.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteRepository clienteRepository;

        public ClientesController(IClienteRepository clienteRepository)
        {
            this.clienteRepository = clienteRepository;
        }

        [HttpGet("{userName}")]
        public IActionResult GetClientes(string userName)
        {
            var a = this.clienteRepository.GetAll().Where(u => u.User.UserName == userName).OrderBy(c=>c.Name);
            return Ok(a);
        }
    }
}