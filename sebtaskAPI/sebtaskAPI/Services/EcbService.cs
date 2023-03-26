using sebtaskAPI.Data;
using sebtaskAPI.Models;
using System.Data.Entity;
using System.Globalization;
using System.Net.Http;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace sebtaskAPI.Services
{
  public class EcbService
  {
    private readonly DataContext _dataContext;

    public EcbService(DataContext dataContext)
    {
      _dataContext = dataContext;
    }

    public ExchangeRate? GetRateByCurrencyCode(string code) {

      return _dataContext.ExchangeRates.Where(c=> c.Currency == code).FirstOrDefault();
    }
    public async Task<bool> CreateRate(ExchangeRate rate)
    {
      if (rate == null)
        throw new ArgumentNullException($"Not found {nameof(rate)}");
      _dataContext.ExchangeRates.Add(rate);
      var saved = await _dataContext.SaveChangesAsync();

      return saved > 0;
    }

    public async Task<bool> UpdateRate(ExchangeRate rate)
    {
      if (rate == null)
        throw new ArgumentNullException($"Not found {nameof(rate)}");
      _dataContext.ExchangeRates.Update(rate);
      var updated = await _dataContext.SaveChangesAsync();

      return updated > 0;
    }

    public List<ExchangeRate> GetRates()
    {
      var rates =  _dataContext.ExchangeRates.ToList();

      if(rates.Count == 0) {
         var getRates = GetLiveCurrencyRates().Result;
         rates = getRates.ToList();
      }
      return rates;
    }

    public async Task<List<ExchangeRate>> GetLiveCurrencyRates() {

      
      //add base currency to the list
     var eurRate = new ExchangeRate
      {

        Currency = "EUR",
        Rate = 1
      };
      var ratesToEuro = new List<ExchangeRate>();
      ratesToEuro.Add(eurRate);

      var existingRate = GetRateByCurrencyCode(eurRate.Currency);
      if (existingRate == null)
      {
        await CreateRate(eurRate);
      }
      else
      {
        await UpdateRate(eurRate);
      }

      try
      {
        var httpClient = new HttpClient();
        var stream = await httpClient.GetStreamAsync("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");

        //load XML document
        var document = new XmlDocument();
        document.Load(stream);

        //add namespaces
        var namespaces = new XmlNamespaceManager(document.NameTable);
        namespaces.AddNamespace("ns", "http://www.ecb.int/vocabulary/2002-08-01/eurofxref");
        namespaces.AddNamespace("gesmes", "http://www.gesmes.org/xml/2002-08-01");

        //get daily rates
        var dailyRates = document.SelectSingleNode("gesmes:Envelope/ns:Cube/ns:Cube", namespaces);
        if (!DateTime.TryParseExact(dailyRates.Attributes["time"].Value, "yyyy-MM-dd", null, DateTimeStyles.None, out var updateDate))
          updateDate = DateTime.UtcNow;

        foreach (XmlNode currency in dailyRates.ChildNodes)
        {
          //get rate
          if (!decimal.TryParse(currency.Attributes["rate"].Value, NumberStyles.Currency, CultureInfo.InvariantCulture, out var currencyRate))
            continue;

          var rate = new ExchangeRate()
          {
            Currency = currency.Attributes["currency"].Value,
            Rate = currencyRate,

          };
          ratesToEuro.Add(rate);

          existingRate = GetRateByCurrencyCode(rate.Currency);
          if (existingRate == null) {
            await CreateRate(rate);
          }
          else {
            await UpdateRate(rate);
          }
         
        }
        
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }


      return ratesToEuro;

    }
  }
}
