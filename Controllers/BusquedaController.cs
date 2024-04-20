using Microsoft.AspNetCore.Mvc;
using MyRestfulApp_NET.Domain.Services;

namespace MyRestfulApp_NET.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public class BusquedaController : BaseController
    {
        private readonly IBusquedaService _busquedaService;

        public BusquedaController(IBusquedaService busquedaService)
        {
            _busquedaService = busquedaService;
        }

        [HttpGet("{termino}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetBusqueda(string termino)
        {
            var searchResult = await _busquedaService.ObtenerInformacionTermino(termino);

            return Ok(searchResult);
        }
    }
}
