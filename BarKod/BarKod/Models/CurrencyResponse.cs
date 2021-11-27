using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BarKod.Models
{
    public class CurrencyResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("terms")]
        public string Terms { get; set; }
        [JsonPropertyName("privacy")]
        public string Privacy { get; set; }
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
        [JsonPropertyName("source")]
        public string Source { get; set; }
        [JsonPropertyName("historical")]
        public bool Historical { get; set; }
        [JsonPropertyName("date")]
        public string Date { get; set; }
        [JsonPropertyName("quotes")]
        public Dictionary<string, double> Quotes { get; set; }
        [JsonPropertyName("info")]
        public Info Info { get; set; }
    }

    public class Info
    {
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("quote")]
        public double Quote { get; set; }
    }
}
