using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarKod.Models
{
    public class RatioOnDay
    {
        public DateTime Time { get; set; }
        public double Rate { get; set; }
        public string FromTo { get; set; }
    }
}
