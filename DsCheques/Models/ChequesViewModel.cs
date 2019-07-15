using DsCheques.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DsCheques.Models
{
    public class ChequesViewModel : Cheque
    {
        public IEnumerable<SelectListItem> Clientes { get; set; }

        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

    }
}
