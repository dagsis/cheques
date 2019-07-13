using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DsCheques.Data.Entities
{
    public class Cheque : IEntity
    {
        public int Id { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FechaIngreso { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FechaDeposito { get; set; }

        [Required(ErrorMessage ="Campo Requerido")]
        [Display(Name="Firmante")]
        [Column(TypeName = "varchar(50)")]
        public string Firmante { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Destino")]
        public string Destino { get; set; }
        public string Numero { get; set; }
        public string Cuenta { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Importe { get; set; }

        [Display(Name = "Foto")]
        public string ImageUrl { get; set; }

        public User User { get; set; }

    }
}
