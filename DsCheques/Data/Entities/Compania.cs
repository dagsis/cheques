using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DsCheques.Data.Entities
{
    public class Compania
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Campo Requerido")]
        [Display(Name="Razón Social")]
        [Column(TypeName = "varchar(50)")]

        public string Name { get; set; }
    }
}
