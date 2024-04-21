using Microsoft.AspNetCore.Mvc;
using MyRestfulApp_NET_API.Domain.Services;
using MyRestfulApp_NET_API.Domain.Services.Communication;

namespace MyRestfulApp_NET_API.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public class PaisesController : BaseController
    {
        private readonly IPaisesService _paisesService;

        public PaisesController(IPaisesService paisesService)
        {
            _paisesService = paisesService;
        }

        [HttpGet("{pais}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(BasicMessageResponse), 401)]
        public async Task<IActionResult> GetPais(string pais)
        {
            var paisUpper = pais.ToUpper();

            if (paisUpper.Equals("BR") || paisUpper.Equals("CO"))
                return Unauthorized(new BasicMessageResponse(false, "País inválido para la consulta.", 1));

            var countryInfo = await _paisesService.ObtenerInformacionPais(paisUpper);

            return Ok(countryInfo);
        }
    }
}
