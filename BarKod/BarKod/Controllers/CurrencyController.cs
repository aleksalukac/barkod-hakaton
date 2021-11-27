using BarKod.Models;
using BarKod.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using QuickChart;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace BarKod.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConversionService _service;

        public CurrencyController(IMemoryCache memoryCache, IConversionService service)
        {
            _memoryCache = memoryCache;
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string GetGraphUrl(string jsonObject)
        {
            RequestDTO request;
            try {
                request = JsonSerializer.Deserialize<RequestDTO>(jsonObject);
            } 
            catch (Exception e) {
                return "";
            }

            var dataframe = _service.GetTimeframe(DateTime.Parse(request.Date).AddDays(-30), DateTime.Parse(request.Date), request.FromCurr, request.ToCurr);

            var labels = new List<string>();
            var data = new List<double>();

            foreach(var df in dataframe)
            {
                labels.Add(df.Time.ToString("dd-MM-yyyy"));
                data.Add(df.Rate);
            }

            Chart qc = new Chart();

            qc.Width = 500;
            qc.Height = 300;
            qc.Config = @"{
              type: 'line',
              data: {
                labels: ##LABELS##,
                datasets: [{
                  label: '##CUR##',
                  data: ##DATA##
                }]
              }
            }";

            qc.Config = qc.Config.Replace("##LABELS##", JsonSerializer.Serialize(labels));
            qc.Config = qc.Config.Replace("##CUR##", JsonSerializer.Serialize(request.FromCurr));
            qc.Config = qc.Config.Replace("##DATA##", JsonSerializer.Serialize(data));

            return qc.GetUrl();
        }

        [HttpGet]
        public bool GetIsGrowing(string jsonObject)
        {
            RequestDTO data;
            try
            {
                data = JsonSerializer.Deserialize<RequestDTO>(jsonObject);
            }
            catch (Exception e)
            {
                return false;
            }

            return _service.IsGrowing(data.FromCurr, Consts.DEFAULT_CURRENCY, 2);
        }

        [HttpGet]
        public string GetQuote(string jsonObject)
        {
            RequestDTO data;
            try
            {
                data = JsonSerializer.Deserialize<RequestDTO>(jsonObject);
            }
            catch (Exception e)
            {
                return "";
            }
            DateTime date;
            try
            {
                date = DateTime.Parse(data.Date);
            }
            catch
            {
                return _service.GetQuote(data.FromCurr, data.ToCurr);
            }


            if(date.Date != DateTime.Now.Date)
            {
                var k = _service.GetHistorical(date, data.FromCurr, data.ToCurr);
                return k;
            }

            return _service.GetQuote(data.FromCurr, data.ToCurr);
        }
    }
}
