using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DsCheques.Common.Models
{
    public partial class Cliente
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cuit")]
        public string Cuit { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }
}
