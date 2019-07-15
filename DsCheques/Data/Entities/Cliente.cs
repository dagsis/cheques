using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DsCheques.Data.Entities
{
    public class Cliente : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Cliente")]
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Cuit")]
        [Column(TypeName = "varchar(11)")]
        public string Cuit { get; set; }
        public User User { get; set; }

    }
}
