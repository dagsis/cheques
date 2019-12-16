using Microsoft.AspNetCore.Mvc.Rendering;
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

        [Required(ErrorMessage = "Campo Requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name ="Fecha de Ingreso")]
        [Column(TypeName = "smalldatetime")]
        public DateTime FechaIngreso { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Deposito")]
        [Column(TypeName = "smalldatetime")]
        public DateTime? FechaDeposito { get; set; }

        [Required(ErrorMessage ="Campo Requerido")]
        [Display(Name="Firmante")]
        [Column(TypeName = "varchar(50)")]
        public string Firmante { get; set; }

        //[Required(ErrorMessage = "Campo Requerido")]

        //[Display(Name = "Cliente")]
        //[Range(1, int.MaxValue, ErrorMessage = "Seleccione un Cliente.")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Destino")]
        public string Destino { get; set; }
        public string Numero { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Importe { get; set; }

        [Display(Name = "Foto")]
        public string ImageUrl { get; set; }

        public User User { get; set; }

        public Cliente Cliente { get; set; }

        public string ImageFullPath {
            get
            {
                if(string.IsNullOrEmpty(this.ImageUrl))
                {
                    return null;
                }

                return $"http://www.cheques.dagsis.com.ar{this.ImageUrl.Substring(1)}";
            }
        }

    }
}
