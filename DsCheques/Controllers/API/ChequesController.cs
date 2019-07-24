using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DsCheques.Data.Entities;
using DsCheques.Data.Repositories.Interfaces;
using DsCheques.Helpers;
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
    public class ChequesController : ControllerBase
    {
        private readonly IChequeRepository chequesRepository;
        private readonly IUserHelper userHerlper;

        public ChequesController(IChequeRepository chequesRepository, IUserHelper userHerlper)
        {
            this.chequesRepository = chequesRepository;
            this.userHerlper = userHerlper;
        }

        [HttpGet("{userName}")]
        public IActionResult GetCheques(string userName)
        {
            var a =  this.chequesRepository.GetAll().Include(c=>c.Cliente).Include(u=>u.User).Where(u => u.User.UserName == userName).OrderByDescending(i=>i.Id);
            return Ok(a);
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Common.Models.Cheque cheque)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var user = await this.userHerlper.GetUserByEmailAsync(cheque.User.Email);
            if (user == null)
            {
                return this.BadRequest("Invalid user");
            }

            var imageUrl = string.Empty;
            if (cheque.ImageArray != null && cheque.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(cheque.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "wwwroot\\images\\Cheques";
                var fullPath = $"~/images/Cheques/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    imageUrl = fullPath;
                }
            }

            var entityCheque = new Cheque
            {
                ClienteId = cheque.ClienteId,
                //Cliente = cheque.Cliente,
                FechaDeposito = cheque.FechaDeposito,
                FechaIngreso = cheque.FechaIngreso,
                Destino = cheque.Destino,
                Firmante = cheque.Firmante,
                Importe = cheque.Importe,
                Numero = cheque.Numero,
                User = user,
                ImageUrl = imageUrl
            };

            var newCheque = await this.chequesRepository.CreateAsync(entityCheque);
            return Ok(newCheque);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Cheque cheque)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            if (id != cheque.Id)
            {
                return BadRequest();
            }

            var oldCheque = await this.chequesRepository.GetByIdAsync(id);
            if (oldCheque == null)
            {
                return this.BadRequest("Cheque Id don't exists.");
            }

            //TODO: Upload images
            oldCheque.ClienteId = cheque.ClienteId;
            oldCheque.FechaDeposito = cheque.FechaDeposito;
            oldCheque.FechaIngreso = cheque.FechaIngreso;
            oldCheque.Destino = cheque.Destino;
            oldCheque.Firmante = cheque.Firmante;
            oldCheque.Importe = cheque.Importe;
            oldCheque.Numero = cheque.Numero;
            oldCheque.Cliente = cheque.Cliente;

            var updatedCheque = await this.chequesRepository.UpdateAsync(oldCheque);
            return Ok(updatedCheque);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var cheque = await this.chequesRepository.GetByIdAsync(id);
            if (cheque == null)
            {
                return this.NotFound();
            }

            await this.chequesRepository.DeleteAsync(cheque);
            return Ok(cheque);
        }


    }
}