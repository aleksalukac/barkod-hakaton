using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BarKod.Models
{
    public class TimeframeResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("quotes")]
        public Dictionary<string, Dictionary<string, double>> Quotes { get; set; }
    }
}
