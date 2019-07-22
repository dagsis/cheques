using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DsCheques.Common.Models
{
   public class Cheque
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fechaIngreso")]
        public DateTime FechaIngreso { get; set; }

        [JsonProperty("fechaDeposito")]
        public DateTime FechaDeposito { get; set; }

        [JsonProperty("firmante")]
        public string Firmante { get; set; }

        [JsonProperty("clienteId")]
        public int ClienteId { get; set; }

        [JsonProperty("destino")]
        public string Destino { get; set; }

        [JsonProperty("numero")]
        public string Numero { get; set; }

        [JsonProperty("importe")]
        public decimal Importe { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("cliente")]
        public Cliente Cliente { get; set; }

        [JsonProperty("imageFullPath")]
        public Uri ImageFullPath { get; set; }

        public override string ToString()
        {
            return $"{this.Firmante} {this.Importe:C2}";
        }
    }
}
