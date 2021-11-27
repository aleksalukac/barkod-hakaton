using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarKod.Models
{
    public class RequestDTO
    {
        public string Date { get; set; }
        public string EndDate { get; set; }
        public double Amount { get; set; }
        public string FromCurr { get; set; }
        public string ToCurr { get; set; } 
    }
}
