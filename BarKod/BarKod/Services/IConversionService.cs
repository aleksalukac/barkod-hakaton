using BarKod.Models;
using System;
using System.Collections.Generic;

namespace BarKod.Services
{
    public interface IConversionService
    {
        public List<RatioOnDay> GetTimeframe(DateTime startDate, DateTime endDate, string from, string to = "");
        public string GetHistorical(DateTime date, string from, string to);
        public string GetApiResponse(string uri);
        public string GetQuote(string from, string to);
        public bool IsGrowing(string currency, string defaultCurrency = "", int daysCount = 30);
    }
}