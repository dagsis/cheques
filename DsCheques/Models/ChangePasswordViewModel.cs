using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DsCheques.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        [Display(Name = "Contraseña Actual")]
        public string OldPassword { get; set; }

        [Required]
        [Display(Name = "Nueva Contraseña")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string Confirm { get; set; }
    }

}
