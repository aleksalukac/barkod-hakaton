using BarKod.Models;
using BarKod.Services;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BarKodTest
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            // ARRANGE 
            MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            ConversionService conversionService = new ConversionService(cache);

            var uri = "https://api.currencylayer.com/convert?access_key=96c7a03cce11e464756d645302fbb324&from=USD&to=RSD&amount=1";
            var data = conversionService.GetApiResponse(uri);

            // ACT 
            var obj = JsonConvert.DeserializeObject<CurrencyResponse>(data);

            // ASSERT
            Assert.IsTrue(obj.Success);
        }

        [Test]
        public void Test2()
        {
            MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            ConversionService conversionService = new ConversionService(cache);
            var uri = "https://api.currencylayer.com/convert?access_key=96c7a03cce11e464756d645302fbb324&from=JPY&to=RSD&amount=1";

            var data = conversionService.GetApiResponse(uri);
            var obj = JsonConvert.DeserializeObject<CurrencyResponse>(data);

            Assert.IsTrue(obj.Success);
        }

        [Test]
        public void Test3() // caching
        {
            MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            ConversionService conversionService = new ConversionService(cache);

            var obj = conversionService.GetQuote("RSD", "USD");

            Assert.That(obj, Is.Not.EqualTo("Error").IgnoreCase);

        }
    }
}