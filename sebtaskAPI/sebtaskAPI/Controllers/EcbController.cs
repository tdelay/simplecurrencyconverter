using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sebtaskAPI.Models;
using sebtaskAPI.Services;

namespace sebtaskAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class EcbController : ControllerBase
  {
    private readonly EcbService _ecbService;

    public EcbController(EcbService ecbService)
    {
      _ecbService = ecbService;
    }
    [HttpGet]
    public  List<ExchangeRate> GetRates() {

     var rates = _ecbService.GetRates();

      return rates;
    }
  }
}
