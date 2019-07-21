using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DsCheques.Data.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Display(Name = "Teléfono")]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

        [NotMapped]
        [Display(Name = "Es Administrador")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Nombre")]
        public string FullName { get { return $"{this.FirstName} {this.LastName}"; } }

        [Display(Name = "Email Confirmado")]
        public override bool EmailConfirmed { get => base.EmailConfirmed; set => base.EmailConfirmed = value; }

    }
}
