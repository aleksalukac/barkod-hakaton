using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarKod.Models
{
    public class Consts
    {
        public static string API_KEY { get; } = "96c7a03cce11e464756d645302fbb324";
        public static string API_URI { get; } = "https://api.currencylayer.com/";
        public static string CONVERT_ENDPOINT { get; } = "convert";
        public static string HISTORICAL_ENDPOINT { get; } = "historical";
        public static string CHANGE_ENDPOINT { get; } = "change";
        public static string TIMEFRAME_ENDPOINT { get; } = "timeframe";
        public static string API_AMOUNT { get; } = "1";
        public static string DEFAULT_CURRENCY { get; } = "RSD";
    }
}
