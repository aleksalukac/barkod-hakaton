using BarKod.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace BarKod.Services
{
    public class ConversionService : IConversionService
    {
        private readonly IMemoryCache _memoryCache;

        public ConversionService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public List<RatioOnDay> GetTimeframe(DateTime startDate, DateTime endDate, string from, string to = "")
        {
            if (to == "")
                to = Consts.DEFAULT_CURRENCY;

            string content = null;

            var uri = Consts.API_URI +
                Consts.TIMEFRAME_ENDPOINT +
                "?access_key=" + Consts.API_KEY +
                "&start_date=" + startDate.ToString("yyyy-MM-dd") +
                "&end_date=" + endDate.ToString("yyyy-MM-dd") +
                "&currencies=" + from + "," + to;

            content = GetApiResponse(uri);

            TimeframeResponse output;
            try
            {
                output = JsonSerializer.Deserialize<TimeframeResponse>(content);
                if (!output.Success)
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }

            var timeframe = output.Quotes;

            var currencyExchangeTimeSeries = new List<RatioOnDay>();

            foreach (var key in timeframe.Keys)
            {
                var ratio = new RatioOnDay();
                ratio.Time = DateTime.Parse(key);
                ratio.Rate = timeframe[key]["USD" + to] / timeframe[key]["USD" + from];
                ratio.FromTo = from + to;

                currencyExchangeTimeSeries.Add(ratio);
            }

            return currencyExchangeTimeSeries;
        }

        public string GetHistorical(DateTime date, string from, string to)
        {
            string content = null;

            var uri = Consts.API_URI +
                Consts.CONVERT_ENDPOINT +
                "?access_key=" + Consts.API_KEY +
                "&date=" + date.ToString("yyyy-MM-dd") +
                "&from=" + from +
                "&to=" + to +
                "&amount=" + Consts.API_AMOUNT;

            content = GetApiResponse(uri);

            CurrencyResponse output;
            try
            {
                output = JsonSerializer.Deserialize<CurrencyResponse>(content);
                if (!output.Success)
                {
                    return "Error";
                }
            }
            catch (Exception e)
            {
                return "Error";
            }

            return output.Info.Quote.ToString();
        }

        public string GetApiResponse(string uri)
        {
            string content = null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader sr = new StreamReader(stream))
            {
                content = sr.ReadToEnd();
            }

            return content;
        }

        public bool IsGrowing(string currency, string defaultCurrency = "", int daysCount = 30)
        {
            if (defaultCurrency == "")
                defaultCurrency = Consts.DEFAULT_CURRENCY;

            var timeframe = GetTimeframe(DateTime.Now.AddDays(-daysCount), DateTime.Now, currency, defaultCurrency);

            var avg = timeframe.Average(x => x.Rate);

            return timeframe.Where(x => x.Time.Date == DateTime.Now.Date).FirstOrDefault().Rate > avg;
        }

        public string GetQuote(string from, string to)
        {
            if (!_memoryCache.TryGetValue(from + to, out double cacheValue))
            {
                if (!_memoryCache.TryGetValue(to + from, out double backupCacheValue))
                {
                    string content = null;

                    var uri = Consts.API_URI +
                        Consts.CONVERT_ENDPOINT +
                        "?access_key=" + Consts.API_KEY +
                        "&from=" + from +
                        "&to=" + to +
                        "&amount=" + Consts.API_AMOUNT;

                    content = GetApiResponse(uri);

                    CurrencyResponse output;
                    try
                    {
                        output = JsonSerializer.Deserialize<CurrencyResponse>(content);
                        if (!output.Success)
                        {
                            return "Error";
                        }
                    }
                    catch (Exception e)
                    {
                        return "Error";
                    }

                    cacheValue = output.Info.Quote;

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(30));

                    _memoryCache.Set(from + to, cacheValue, cacheEntryOptions);
                }
                else
                {
                    if (backupCacheValue != 0)
                        cacheValue = 1 / backupCacheValue;
                }
            }

            return cacheValue.ToString();
        }
    }
}
