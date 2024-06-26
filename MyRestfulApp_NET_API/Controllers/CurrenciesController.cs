using Microsoft.AspNetCore.Mvc;
using MyRestfulApp_NET_API.Domain.Services;
using MyRestfulApp_NET_API.Resources;

namespace MyRestfulApp_NET_API.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public class CurrenciesController : BaseController
    {
        private readonly ICurrencyService _currencyService;

        public CurrenciesController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet()]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<CurrencyResource>>> GetCurrencies()
        {
            var currencies = await _currencyService.GetCurrencies();
            return Ok(currencies);
        }
    }
}
