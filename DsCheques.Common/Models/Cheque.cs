using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DsCheques.Common.Models
{
   public class Cheque
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("fechaIngreso")]
        public DateTimeOffset FechaIngreso { get; set; }

        [JsonProperty("fechaDeposito")]
        public DateTimeOffset FechaDeposito { get; set; }

        [JsonProperty("firmante")]
        public string Firmante { get; set; }

        [JsonProperty("clienteId")]
        public long ClienteId { get; set; }

        [JsonProperty("destino")]
        public string Destino { get; set; }

        [JsonProperty("numero")]
        public string Numero { get; set; }

        [JsonProperty("cuenta")]
        public string Cuenta { get; set; }

        [JsonProperty("importe")]
        public long Importe { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("cliente")]
        public Cliente Cliente { get; set; }

        [JsonProperty("imageFullPath")]
        public object ImageFullPath { get; set; }
    }
}
